using System;
using System.Collections.Generic;
using System.Text;
using StoryTeller.Portal.CQRS;

namespace StoryTeller.ResultAggregation.Requests
{
    public class AddSpecBatchToRunRequest : ApplicationScoped, IRequest
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
