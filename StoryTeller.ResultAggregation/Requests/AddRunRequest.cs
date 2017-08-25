using StoryTeller.Portal.CQRS;
using StoryTeller.ResultAggregation.Models.ClientModel;
using StoryTeller.ResultAggregation.Models;

namespace StoryTeller.ResultAggregation.Requests
{
    public class AddRunRequest : ApplicationScoped, IRequest<Run>
    {
        public PostRun PostedRun { get; }
        public AddRunRequest(int applicationId, PostRun postedRun) : base(applicationId)
        {
            PostedRun = postedRun;
        }
    }
}
