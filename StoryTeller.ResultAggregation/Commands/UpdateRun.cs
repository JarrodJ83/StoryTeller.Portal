using StoryTeller.Portal.CQRS;
using StoryTeller.ResultAggregation.Models;

namespace StoryTeller.ResultAggregation.Commands
{
    public class UpdateRun : ApplicationScoped, ICommand
    {
        public Run Run { get; }

        public UpdateRun(int applicationId, Run run) : base(applicationId)
        {
            Run = run;
        }
    }
}
