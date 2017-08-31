using StoryTeller.Portal.CQRS;
using StoryTeller.ResultAggregation.Models.ClientModel;

namespace StoryTeller.ResultAggregation.Requests
{
    public class PostRunResultRequest : ApplicationScoped, IRequest
    {
        public PostRunResult PostedRunResult { get; }
        public int RunId { get; }
        public PostRunResultRequest(int appId, int runId, PostRunResult postedRunResult) : base(appId)
        {
            RunId = runId;
            PostedRunResult = postedRunResult;
        }
    }
}
