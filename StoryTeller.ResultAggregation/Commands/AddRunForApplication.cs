using System;
using StoryTeller.Portal.CQRS;
using StoryTeller.ResultAggregation.Models;

namespace StoryTeller.ResultAggregation.Commands
{
    public class AddRunForApplication : ICommand<int>
    {
        public int ApplicationId { get; }
        public string RunName { get; }
        public DateTime RunDate { get; }

        public AddRunForApplication(int applicationId, string runName, DateTime runDate)
        {
            ApplicationId = applicationId;
            RunName = runName;
            RunDate = runDate;
        }
    }
}
