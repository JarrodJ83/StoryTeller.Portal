using System;
using StoryTeller.Portal.CQRS;

namespace StoryTeller.ResultAggregation.Commands
{
    public class AddRunForApplication : ICommand<int>
    {
        public int ApplicationId { get; }
        public string RunName { get; }
        public DateTime RunDate { get; }
        public int Key { get; set; }

        public AddRunForApplication(int applicationId, string runName, DateTime runDate)
        {
            ApplicationId = applicationId;
            RunName = runName;
            RunDate = runDate;
        }
    }
}
