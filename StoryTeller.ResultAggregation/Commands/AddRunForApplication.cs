using StoryTeller.Portal.CQRS;
using StoryTeller.ResultAggregation.Models;

namespace StoryTeller.ResultAggregation.Commands
{
    public class AddRunForApplication : ICommand
    {
        public int ApplicationId { get; }
        public Run Run { get; }

        public AddRunForApplication(int applicationId, Run run)
        {
            ApplicationId = applicationId;
            Run = run;
        }
    }
}
