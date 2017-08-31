using StoryTeller.Portal.CQRS;
using StoryTeller.ResultAggregation.Models;

namespace StoryTeller.ResultAggregation.Commands
{
    public class AddRunResult : ApplicationScoped, ICommand
    {
        public RunResult RunResult { get; }

        public AddRunResult(int appId, RunResult runResult) : base(appId)
        {
            RunResult = runResult;
        }
    }
}
