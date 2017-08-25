using StoryTeller.Portal.CQRS;
using StoryTeller.ResultAggregation.Models;

namespace StoryTeller.ResultAggregation.Commands
{
    public class AddSpecToRun : ApplicationScoped, ICommand
    {
        public RunSpec RunSpec { get; }
        public AddSpecToRun(int appId, RunSpec runSpec) : base(appId)
        {
            RunSpec = runSpec;
        }
    }
}
