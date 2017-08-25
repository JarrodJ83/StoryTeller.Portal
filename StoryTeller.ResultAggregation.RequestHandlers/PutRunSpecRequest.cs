using System.Threading;
using System.Threading.Tasks;
using StoryTeller.Portal.CQRS;
using StoryTeller.ResultAggregation.Commands;
using StoryTeller.ResultAggregation.Requests;

namespace StoryTeller.ResultAggregation.RequestHandlers
{
    public class PutRunSpecRequestHandler : IRequestHandler<PutRunSpecRequest>
    {
        private readonly ICommandHandler<Commands.UpdateRunSpec> _passFailRunSpecCommandHandler;

        public PutRunSpecRequestHandler(ICommandHandler<UpdateRunSpec> passFailRunSpecCommandHandler)
        {
            _passFailRunSpecCommandHandler = passFailRunSpecCommandHandler;
        }

        public async Task HandleAsync(PutRunSpecRequest request, CancellationToken cancellationToken)
        {
            var passFailRunSpecCmd = new UpdateRunSpec(request.AppId, request.RunSpec.RunId, request.RunSpec.SpecId, request.RunSpec.Success.Value);

            await _passFailRunSpecCommandHandler.ExecuteAsync(passFailRunSpecCmd, cancellationToken);
        }
    }
}
