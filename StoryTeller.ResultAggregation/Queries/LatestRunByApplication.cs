using System;
using System.Collections.Generic;
using System.Text;
using StoryTeller.Portal.CQRS;
using StoryTeller.ResultAggregation.Models;

namespace StoryTeller.ResultAggregation.Queries
{
    public class LatestRunByApplication : ApplicationScoped, IQuery<Run>
    {
        public LatestRunByApplication(int appId) : base(appId)
        {
        }
    }
}
