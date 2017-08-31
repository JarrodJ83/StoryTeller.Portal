using StoryTeller.Portal.CQRS;
using StoryTeller.ResultAggregation.Models;

namespace StoryTeller.ResultAggregation.Requests
{
    public class PutRunRequest : ApplicationScoped, IRequest
    {
        public Run Run;

        public PutRunRequest(int appId, Run run) : base(appId)
        {
            Run = run;
        }
    }
}
