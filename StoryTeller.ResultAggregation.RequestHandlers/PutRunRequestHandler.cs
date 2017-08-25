using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using StoryTeller.Portal.CQRS;
using StoryTeller.ResultAggregation.Commands;
using StoryTeller.ResultAggregation.Models;
using StoryTeller.ResultAggregation.Requests;

namespace StoryTeller.ResultAggregation.RequestHandlers
{
    public class PutRunRequestHandler : IRequestHandler<Requests.PutRunRequest>
    {
        private ICommandHandler<Commands.UpdateRun> _updateRunCommandHandler;

        public PutRunRequestHandler(ICommandHandler<UpdateRun> updateRunCommandHandler)
        {
            _updateRunCommandHandler = updateRunCommandHandler;
        }

        public async Task HandleAsync(PutRunRequest request, CancellationToken cancellationToken)
        {
            var updateRunCmd = new UpdateRun(request.AppId, request.Run);
            await _updateRunCommandHandler.ExecuteAsync(updateRunCmd, cancellationToken);
        }
    }
}
