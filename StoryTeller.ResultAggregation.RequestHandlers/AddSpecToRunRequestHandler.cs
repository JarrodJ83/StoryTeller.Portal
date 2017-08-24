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
    public class AddSpecToRunRequestHandler : IRequestHandler<AddSpecToRunRequest>
    {
        private ICommandHandler<AddSpecToRun> _addSpecToRunCommandHandler;

        public AddSpecToRunRequestHandler(ICommandHandler<AddSpecToRun> addSpecToRunCommandHandler)
        {
            _addSpecToRunCommandHandler = addSpecToRunCommandHandler;
        }

        public async Task HandleAsync(AddSpecToRunRequest request, CancellationToken cancellationToken)
        {
            await _addSpecToRunCommandHandler.ExecuteAsync(new AddSpecToRun(request.AppId, request.RunId, request.SpecId), cancellationToken);
        }
    }
}
