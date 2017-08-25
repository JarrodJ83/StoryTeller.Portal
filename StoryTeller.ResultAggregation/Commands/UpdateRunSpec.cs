using StoryTeller.Portal.CQRS;

namespace StoryTeller.ResultAggregation.Commands
{
    public class UpdateRunSpec : ApplicationScoped, ICommand
    {
        public int RunId { get; }
        public int SpecId { get; }
        public bool? Passed { get; }

        public UpdateRunSpec(int appId, int runId, int specId, bool? passed) : base(appId)
        {
            RunId = runId;
            SpecId = specId;
            Passed = passed;
        }
    }
}
