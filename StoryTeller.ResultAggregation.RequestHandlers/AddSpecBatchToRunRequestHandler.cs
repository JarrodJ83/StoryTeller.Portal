using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using StoryTeller.Portal.CQRS;
using StoryTeller.ResultAggregation.Commands;
using StoryTeller.ResultAggregation.Models;
using StoryTeller.ResultAggregation.Requests;

namespace StoryTeller.ResultAggregation.RequestHandlers
{
    public class AddSpecBatchToRunRequestHandler : IRequestHandler<AddSpecBatchToRunRequest, List<RunSpec>>
    {
        private ICommandHandler<AddSpecToRun> _addSpecToRunCommandHandler;

        public AddSpecBatchToRunRequestHandler(ICommandHandler<AddSpecToRun> addSpecToRunCommandHandler)
        {
            _addSpecToRunCommandHandler = addSpecToRunCommandHandler;
        }

        public async Task<List<RunSpec>> HandleAsync(AddSpecBatchToRunRequest request, CancellationToken cancellationToken)
        {
            var runSpecs = await Task.WhenAll(request.SpecIds.Select(async specId =>
            {
                var runSpec = new RunSpec {RunId = request.RunId, SpecId = specId};
                await _addSpecToRunCommandHandler.ExecuteAsync(new AddSpecToRun(request.AppId, runSpec),
                    cancellationToken);
                return runSpec;
            }));

            return runSpecs.ToList();
        }
    }
}
