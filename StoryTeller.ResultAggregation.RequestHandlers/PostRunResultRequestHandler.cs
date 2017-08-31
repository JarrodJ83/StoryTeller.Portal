using System.Threading;
using System.Threading.Tasks;
using MediatR;
using StoryTeller.Portal.CQRS;
using StoryTeller.ResultAggregation.Commands;
using StoryTeller.ResultAggregation.Events;
using StoryTeller.ResultAggregation.Models;
using StoryTeller.ResultAggregation.Requests;

namespace StoryTeller.ResultAggregation.RequestHandlers
{
    public class PostRunResultRequestHandler : Portal.CQRS.IRequestHandler<PostRunResultRequest>
    {
        private readonly IMediator _mediator;
        private readonly ICommandHandler<AddRunResult> _addRunResultCommandHandler;

        public PostRunResultRequestHandler(IMediator mediator, ICommandHandler<AddRunResult> addRunResultCommandHandler)
        {
            _mediator = mediator;
            _addRunResultCommandHandler = addRunResultCommandHandler;
        }

        public async Task HandleAsync(PostRunResultRequest request, CancellationToken cancellationToken)
        {
            var runResult = new RunResult
            {
                RunId = request.RunId,
                HtmlResults = request.PostedRunResult.HtmlResults,
                Passed = request.PostedRunResult.Passed
            };
            await _addRunResultCommandHandler.ExecuteAsync(new AddRunResult(request.AppId, runResult), cancellationToken);
            
            _mediator.Publish(new RunCompleted(request.RunId, runResult.Passed));
        }
    }
}
