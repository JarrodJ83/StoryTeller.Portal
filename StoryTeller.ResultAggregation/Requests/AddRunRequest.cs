using System;
using System.Collections.Generic;
using StoryTeller.Portal.CQRS;

namespace StoryTeller.ResultAggregation.Requests
{
    public class AddRunRequest : IRequest<int>
    {
        public int ApplicationId { get; set; }
        public string RunName { get; set; }
        public DateTime RunDateTime { get; set; }
        public Dictionary<Guid, string> Specifications { get; set; }
    }
}
