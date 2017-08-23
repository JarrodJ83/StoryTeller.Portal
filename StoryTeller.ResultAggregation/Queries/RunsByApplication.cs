using System.Collections.Generic;
using StoryTeller.Portal.CQRS;
using StoryTeller.ResultAggregation.Models;

namespace StoryTeller.ResultAggregation.Queries
{
    public class RunsByApplication : IQuery<List<ApplicationRun>>
    {
        public int ApplicationId { get; }

        public RunsByApplication(int applicationId)
        {
            ApplicationId = applicationId;
        }
    }
}
