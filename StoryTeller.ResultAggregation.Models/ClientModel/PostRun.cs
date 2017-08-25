using System;

namespace StoryTeller.ResultAggregation.Models.ClientModel
{
    public class PostRun
    {
        public string RunName { get; set; }
        public DateTime RunDateTime { get; set; }
        public string HtmlResults { get; set; }
    }
}
