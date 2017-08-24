using System;
using System.Collections.Generic;
using System.Text;
using StoryTeller.Portal.CQRS;

namespace StoryTeller.ResultAggregation.Requests
{
    public class AddSpecToRunRequest : ApplicationScoped, IRequest
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
