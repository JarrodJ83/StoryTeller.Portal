using System.Collections.Generic;
using StoryTeller.Portal.CQRS;
using StoryTeller.ResultAggregation.Models;

namespace StoryTeller.ResultAggregation.Requests
{
    public class AddSpecBatchToRunRequest : ApplicationScoped, IRequest<List<RunSpec>>
    {
        public int RunId { get; }
        public List<int> SpecIds { get; }

        public AddSpecBatchToRunRequest(int applicationId, int runId, List<int> specIds)
            : base(applicationId)
        {
            RunId = runId;
            SpecIds = specIds;
        }
    }
}
