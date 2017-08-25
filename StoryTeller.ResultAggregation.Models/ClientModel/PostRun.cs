using System;
using System.Collections.Generic;
using System.Text;

namespace StoryTeller.ResultAggregation.ClientModel
{
    public class PostRun
    {
        public string RunName { get; set; }
        public DateTime RunDateTime { get; set; }
        public string HtmlResults { get; set; }
    }
}
