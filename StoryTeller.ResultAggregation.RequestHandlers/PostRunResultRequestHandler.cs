using System.Threading;
using System.Threading.Tasks;
using StoryTeller.Portal.CQRS;
using StoryTeller.ResultAggregation.Commands;
using StoryTeller.ResultAggregation.Models;
using StoryTeller.ResultAggregation.Requests;

namespace StoryTeller.ResultAggregation.RequestHandlers
{
    public class PostRunResultRequestHandler : IRequestHandler<PostRunResultRequest>
    {
        private readonly ICommandHandler<AddRunResult> _addRunResultCommandHandler;

        public PostRunResultRequestHandler(ICommandHandler<AddRunResult> addRunResultCommandHandler)
        {
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
        }
    }
}
