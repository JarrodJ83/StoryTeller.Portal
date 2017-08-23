using System;
using StoryTeller.Portal.CQRS;

namespace StoryTeller.ResultAggregation.Commands
{
    public class AddApplication : ICommand
    {
        public int ApplicationId { get; protected set; }
        public string ApplicationName { get; }
        public Guid ApiKey { get; }

        public AddApplication(string applicationName, Guid apiKey)
        {
            ApplicationName = applicationName;
            ApiKey = apiKey;
        }
    }
}
