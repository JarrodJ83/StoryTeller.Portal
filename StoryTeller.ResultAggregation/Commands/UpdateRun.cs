using StoryTeller.Portal.CQRS;
using StoryTeller.ResultAggregation.Models;

namespace StoryTeller.ResultAggregation.Commands
{
    public class UpdateRun : ApplicationScoped, ICommand
    {
        public Run Run { get; }

        public UpdateRun(int appId, Run run) : base(appId)
        {
            Run = run;
        }
    }
}
