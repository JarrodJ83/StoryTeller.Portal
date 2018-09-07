using MediatR;
using StoryTeller.ResultAggregation.Models;

namespace StoryTeller.ResultAggregation.Events
{
    public class RunCreated : INotification
    {
        public Run Run { get; }

        public RunCreated(Run run)
        {
            Run = run;
        }
    }
}
