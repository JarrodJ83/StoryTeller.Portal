using MediatR;

namespace StoryTeller.ResultAggregation.Events
{
    public class RunCreated : INotification
    {
        public int RunId { get; }

        public RunCreated(int runId)
        {
            RunId = runId;
        }
    }
}
