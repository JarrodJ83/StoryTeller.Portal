using MediatR;

namespace StoryTeller.ResultAggregation.Events
{
    public class RunSpecUpdated : INotification
    {
        public int RunId { get; }
        public int SpecId { get; }
        public bool Passed { get; }
        public RunSpecUpdated(int runId, int specId, bool passed)
        {
            RunId = runId;
            SpecId = specId;
            Passed = passed;    
        }
    }
}
