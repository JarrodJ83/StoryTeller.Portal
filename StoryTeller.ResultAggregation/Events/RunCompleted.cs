using MediatR;

namespace StoryTeller.ResultAggregation.Events
{
    public class RunCompleted : INotification
    {
        public int RunId { get; }
        public RunCompleted(int runId)
        {
            RunId = runId;
        }
    }
}
