using System;
using System.Collections.Generic;
using System.Text;
using StoryTeller.Portal.CQRS;
using StoryTeller.ResultAggregation.ClientModel;
using StoryTeller.ResultAggregation.Models;

namespace StoryTeller.ResultAggregation.Requests
{
    public class AddSpecRequest : ApplicationScoped, IRequest<Spec>
    {
        public PostSpec PostedSpec { get; private set; }

        public AddSpecRequest(int applicationId, PostSpec postedSpec) : base(applicationId)
        {
            PostedSpec = postedSpec;
        }
    }
}
