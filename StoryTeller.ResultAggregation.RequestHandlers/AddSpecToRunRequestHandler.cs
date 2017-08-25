using System.Threading;
using System.Threading.Tasks;
using StoryTeller.Portal.CQRS;
using StoryTeller.ResultAggregation.Commands;
using StoryTeller.ResultAggregation.Models;
using StoryTeller.ResultAggregation.Requests;

namespace StoryTeller.ResultAggregation.RequestHandlers
{
    public class AddSpecToRunRequestHandler : IRequestHandler<AddSpecToRunRequest, RunSpec>
    {
        private ICommandHandler<AddSpecToRun> _addSpecToRunCommandHandler;

        public AddSpecToRunRequestHandler(ICommandHandler<AddSpecToRun> addSpecToRunCommandHandler)
        {
            _addSpecToRunCommandHandler = addSpecToRunCommandHandler;
        }

        public async Task<RunSpec> HandleAsync(AddSpecToRunRequest request, CancellationToken cancellationToken)
        {
            var runSpec = new RunSpec
            {
                RunId = request.RunId,
                SpecId = request.SpecId
            };
            var addSpecToRunCmd = new AddSpecToRun(request.AppId, runSpec);
            await _addSpecToRunCommandHandler.ExecuteAsync(addSpecToRunCmd, cancellationToken);

            return runSpec;
        }
    }
}
