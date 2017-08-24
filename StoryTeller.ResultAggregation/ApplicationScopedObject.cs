using System;
using System.Collections.Generic;
using System.Text;

namespace StoryTeller.ResultAggregation
{
    public abstract class ApplicationScoped
    {
        public int AppId { get; }

        public ApplicationScoped(int applicationId)
        {
            AppId = applicationId;
        }
    }
}
