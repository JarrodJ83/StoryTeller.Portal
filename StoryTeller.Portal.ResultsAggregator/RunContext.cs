namespace StoryTeller.Portal.ResultsAggregator
{
    public class RunContext
    {
        private static RunContext _instance;
        public static RunContext Current => _instance;
        public int RunId { get; }

        internal RunContext(int runId)
        {
            RunId = runId;
        }

        public static void Create(int runId)
        {
            _instance = new RunContext(runId);
        }

        public static void Destroy()
        {
            _instance = null;
        }
    }
}