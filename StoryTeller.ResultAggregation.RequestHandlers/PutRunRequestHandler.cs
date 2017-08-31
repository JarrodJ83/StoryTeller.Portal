using System.Threading;
using System.Threading.Tasks;
using MediatR;
using StoryTeller.Portal.CQRS;
using StoryTeller.ResultAggregation.Commands;
using StoryTeller.ResultAggregation.Events;
using StoryTeller.ResultAggregation.Requests;

namespace StoryTeller.ResultAggregation.RequestHandlers
{
    public class PutRunRequestHandler : Portal.CQRS.IRequestHandler<Requests.PutRunRequest>
    {
        private readonly IMediator _mediator;
        private ICommandHandler<Commands.UpdateRun> _updateRunCommandHandler;

        public PutRunRequestHandler(IMediator mediator, ICommandHandler<UpdateRun> updateRunCommandHandler)
        {
            _mediator = mediator;
            _updateRunCommandHandler = updateRunCommandHandler;
        }

        public async Task HandleAsync(PutRunRequest request, CancellationToken cancellationToken)
        {
            var updateRunCmd = new UpdateRun(request.AppId, request.Run);
            await _updateRunCommandHandler.ExecuteAsync(updateRunCmd, cancellationToken);

            await _mediator.Publish(new RunCompleted(request.Run.Id), cancellationToken);
        }
    }
}
