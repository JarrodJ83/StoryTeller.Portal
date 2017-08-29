using System;

namespace StoryTeller.Portal.Models.Views
{
    public class RunSummary
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int AppId { get; set; }
        public string AppName { get; set; }
        public DateTime RunDateTime { get; set; }
        public bool Passed => Finished && FailureCount == 0;
        public bool Finished => !string.IsNullOrEmpty(HtmlResults);
        public int SuccessfulCount { get; set; }
        public int FailureCount { get; set; }
        public int TotalCount { get; set; }
        public string HtmlResults { get; set; }
    }
}
