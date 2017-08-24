using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using StoryTeller.Portal.CQRS;
using StoryTeller.ResultAggregation.CommandHandlers;
using StoryTeller.ResultAggregation.Commands;
using StoryTeller.ResultAggregation.Requests;
using StoryTeller.ResultAggregation.Settings;

namespace StoryTeller.ResultAggregation.RequestHandlers
{
    public class AddSpecBatchToRunRequestHandler : IRequestHandler<AddSpecBatchToRunRequest>
    {
        private ICommandHandler<AddSpecToRun> _addSpecToRunCommandHandler;

        public AddSpecBatchToRunRequestHandler(ICommandHandler<AddSpecToRun> addSpecToRunCommandHandler)
        {
            _addSpecToRunCommandHandler = addSpecToRunCommandHandler;
        }

        public async Task HandleAsync(AddSpecBatchToRunRequest request, CancellationToken cancellationToken)
        {
            request.SpecIds.ForEach(async specId => 
                await _addSpecToRunCommandHandler.ExecuteAsync(new AddSpecToRun(request.AppId, request.RunId, specId), cancellationToken));
        }
    }
}
