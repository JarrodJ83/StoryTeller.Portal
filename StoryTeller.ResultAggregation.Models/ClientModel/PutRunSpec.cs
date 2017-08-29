namespace StoryTeller.ResultAggregation.Models.ClientModel
{
    public class PassFailRunSpec
    {
        public int RunId { get; private set; }
        public int SpecId { get; private set; }
        public bool Passed { get; private set; }

        public PassFailRunSpec(int runId, int specId, bool passed)
        {
            RunId = runId;
            SpecId = specId;
            Passed = passed;
        }
    }
}
