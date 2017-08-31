using System.Threading;
using System.Threading.Tasks;
using MediatR;
using StoryTeller.Portal.CQRS;
using StoryTeller.ResultAggregation.Commands;
using StoryTeller.ResultAggregation.Events;
using StoryTeller.ResultAggregation.Requests;

namespace StoryTeller.ResultAggregation.RequestHandlers
{
    public class PutRunSpecRequestHandler : Portal.CQRS.IRequestHandler<PutRunSpecRequest>
    {
        private readonly IMediator _mediator;
        private readonly ICommandHandler<Commands.UpdateRunSpec> _passFailRunSpecCommandHandler;

        public PutRunSpecRequestHandler(IMediator mediator, ICommandHandler<UpdateRunSpec> passFailRunSpecCommandHandler)
        {
            _mediator = mediator;
            _passFailRunSpecCommandHandler = passFailRunSpecCommandHandler;
        }

        public async Task HandleAsync(PutRunSpecRequest request, CancellationToken cancellationToken)
        {
            var passFailRunSpecCmd = new UpdateRunSpec(request.AppId, request.RunSpec.RunId, request.RunSpec.SpecId, request.RunSpec.Success.Value);

            await _passFailRunSpecCommandHandler.ExecuteAsync(passFailRunSpecCmd, cancellationToken);

            _mediator.Publish(new RunSpecUpdated(request.RunSpec.RunId, request.RunSpec.SpecId));
        }
    }
}
