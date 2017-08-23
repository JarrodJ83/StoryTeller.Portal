using System;

namespace StoryTeller.ResultAggregation.Models
{
    public class ApplicationRun
    {
        public string Application { get; set; }
        public int ApplicationId { get; set; }
        public int RunId { get; set; }
        public string RunName { get; set; }
        public DateTime RunDateTime { get; set; }
    }
}
