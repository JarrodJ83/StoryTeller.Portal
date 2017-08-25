using System.Collections.Generic;
using StoryTeller.Portal.CQRS;
using StoryTeller.ResultAggregation.Models;

namespace StoryTeller.ResultAggregation.Queries
{
    public class RunsByApplication : IQuery<List<Run>>
    {
        public int AppId { get; }

        public RunsByApplication(int appId)
        {
            AppId = appId;
        }
    }
}
