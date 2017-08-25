using System;

namespace StoryTeller.ResultAggregation.Models
{
    public class Run
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime RunDateTime { get; set; }
        public string HtmlResults { get; set; }
    }
}
