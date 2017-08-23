using StoryTeller.Portal.CQRS;
using StoryTeller.ResultAggregation.Models;

namespace StoryTeller.ResultAggregation.Commands
{
    public class AddRunForApplication : ICommand
    {
        public ApplicationRun ApplicationRun { get; }

        public AddRunForApplication(ApplicationRun applicationRun)
        {
            ApplicationRun = applicationRun;
        }
    }
}
