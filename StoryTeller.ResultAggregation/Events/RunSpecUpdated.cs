using MediatR;

namespace StoryTeller.ResultAggregation.Events
{
    public class RunSpecUpdated : INotification
    {
        public int RunId { get; }
        public int SpecId { get; }
        public RunSpecUpdated(int runId, int specId)
        {
            RunId = runId;
            SpecId = specId;
        }
    }
}
