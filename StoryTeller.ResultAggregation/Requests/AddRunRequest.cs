using StoryTeller.Portal.CQRS;
using StoryTeller.ResultAggregation.Models.ClientModel;
using StoryTeller.ResultAggregation.Models;

namespace StoryTeller.ResultAggregation.Requests
{
    public class AddRunRequest : ApplicationScoped, IRequest<Run>
    {
        public StartNewRun PostedRun { get; }
        public AddRunRequest(int appId, StartNewRun postedRun) : base(appId)
        {
            PostedRun = postedRun;
        }
    }
}
