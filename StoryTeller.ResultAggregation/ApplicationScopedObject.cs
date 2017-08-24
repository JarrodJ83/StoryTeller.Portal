using System;
using System.Collections.Generic;
using System.Text;

namespace StoryTeller.ResultAggregation
{
    public abstract class ApplicationScoped
    {
        public int ApplicationId { get; }

        public ApplicationScoped(int applicationId)
        {
            ApplicationId = applicationId;
        }
    }
}
