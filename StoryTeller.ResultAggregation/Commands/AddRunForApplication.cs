using StoryTeller.Portal.CQRS;
using StoryTeller.ResultAggregation.Models;

namespace StoryTeller.ResultAggregation.Commands
{
    public class AddRunForApplication : ICommand
    {
        public int AppId { get; }
        public Run Run { get; }

        public AddRunForApplication(int appId, Run run)
        {
            AppId = appId;
            Run = run;
        }
    }
}
