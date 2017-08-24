using System;
using System.Collections.Generic;
using System.Text;

namespace StoryTeller.ResultAggregation.Requests
{
    public class ApplicationScopedRequest
    {
        public int ApplicationId { get; }

        public ApplicationScopedRequest(int applicationId)
        {
            ApplicationId = applicationId;
        }
    }
}
