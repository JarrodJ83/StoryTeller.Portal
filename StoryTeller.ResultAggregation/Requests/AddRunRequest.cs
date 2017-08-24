using System;
using Newtonsoft.Json;
using StoryTeller.Portal.CQRS;

namespace StoryTeller.ResultAggregation.Requests
{
    public class AddRunRequest : ApplicationScopedRequest, IRequest<int>
    {
        public AddRunRequest(int applicationId) : base(applicationId)
        {
        }

        public string RunName { get; set; }
        public DateTime RunDateTime { get; set; }
    }
}
