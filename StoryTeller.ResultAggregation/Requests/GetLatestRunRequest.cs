using StoryTeller.Portal.CQRS;
using StoryTeller.ResultAggregation.Models;

namespace StoryTeller.ResultAggregation.Requests
{
    public class GetLatestRunRequest : ApplicationScoped, IRequest<Run>
    {
        public GetLatestRunRequest(int appId) : base(appId)
        {
        }
    }
}
