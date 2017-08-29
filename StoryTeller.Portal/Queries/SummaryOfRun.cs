using StoryTeller.Portal.CQRS;

namespace StoryTeller.Portal.Queries
{
    public class SummaryOfRun : IQuery<Models.Views.RunSummary>
    {
        public int RunId { get; private set; }
        public SummaryOfRun(int runId)
        {
            RunId = runId;
        }
    }
}
