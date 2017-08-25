using StoryTeller.Portal.CQRS;
using StoryTeller.ResultAggregation.Models;

namespace StoryTeller.ResultAggregation.Requests
{
    public class PutRunSpecRequest : ApplicationScoped, IRequest
    {
        public RunSpec RunSpec { get; }

        public PutRunSpecRequest(int appId, RunSpec runSpec) : base(appId)
        {
            RunSpec = runSpec;
        }
    }
}
