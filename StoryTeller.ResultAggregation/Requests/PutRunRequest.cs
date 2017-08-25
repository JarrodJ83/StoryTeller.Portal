using System;
using System.Collections.Generic;
using System.Text;
using StoryTeller.Portal.CQRS;
using StoryTeller.ResultAggregation.Models;
using StoryTeller.ResultAggregation.Models.ClientModel;

namespace StoryTeller.ResultAggregation.Requests
{
    public class PutRunRequest : ApplicationScoped, IRequest
    {
        public Run Run;

        public PutRunRequest(int applicationId, Run run) : base(applicationId)
        {
            Run = run;
        }
    }
}
