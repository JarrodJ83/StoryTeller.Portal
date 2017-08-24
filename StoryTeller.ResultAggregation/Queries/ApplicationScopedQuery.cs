using System;
using System.Collections.Generic;
using System.Text;

namespace StoryTeller.ResultAggregation.Queries
{
    public abstract class ApplicationScopedQuery
    {
        public int ApplicationId { get; }

        public ApplicationScopedQuery(int applicationId)
        {
            ApplicationId = applicationId;
        }
    }
}
