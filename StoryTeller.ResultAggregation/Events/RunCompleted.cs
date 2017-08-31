using MediatR;

namespace StoryTeller.ResultAggregation.Events
{
    public class RunCompleted : INotification
    {
        public int RunId { get; }
        public bool Passed { get; }

        public RunCompleted(int runId, bool passed)
        {
            RunId = runId;
            Passed = passed;
        }
    }
}
