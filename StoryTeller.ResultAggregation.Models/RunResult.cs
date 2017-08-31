namespace StoryTeller.ResultAggregation.Models
{
    public class RunResult
    {
        public int RunId { get; set; }
        public string HtmlResults { get; set; }
        public bool Passed { get; set; }
    }
}
