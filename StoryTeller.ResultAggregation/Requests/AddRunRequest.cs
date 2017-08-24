using System;
using Newtonsoft.Json;
using StoryTeller.Portal.CQRS;

namespace StoryTeller.ResultAggregation.Requests
{
    public class AddRunRequest : IRequest<int>
    {
        [JsonIgnore]
        public int ApplicationId { get; set; }
        public string RunName { get; set; }
        public DateTime RunDateTime { get; set; }
    }
}
