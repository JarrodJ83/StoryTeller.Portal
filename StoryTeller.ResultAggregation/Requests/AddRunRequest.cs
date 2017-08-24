using System;
using Newtonsoft.Json;
using StoryTeller.Portal.CQRS;
using StoryTeller.ResultAggregation.ClientModel;

namespace StoryTeller.ResultAggregation.Requests
{
    public class AddRunRequest : ApplicationScoped, IRequest<int>
    {
        public PostRun PostedRun { get; }
        public AddRunRequest(int applicationId, PostRun postedRun) : base(applicationId)
        {
            PostedRun = postedRun;
        }
    }
}
