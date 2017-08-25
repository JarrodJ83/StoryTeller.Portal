using StoryTeller.Portal.CQRS;
using StoryTeller.ResultAggregation.Models;

namespace StoryTeller.ResultAggregation.Requests
{
    public class AddSpecToRunRequest : ApplicationScoped, IRequest<RunSpec>
    {
        public int RunId { get; }
        public int SpecId { get; }

        public AddSpecToRunRequest(int applicationId, int runId, int specId)
            : base(applicationId)
        {
            RunId = runId;
            SpecId = specId;
        }
    }
}
