using System;
using System.Collections.Generic;

namespace StoryTeller.ResultAggregation.Models.ClientModel
{
    public class StartNewRun
    {
        public string RunName { get; set; }
        public DateTime RunDateTime { get; set; }
        public List<int> SpecIds { get; set; }
    }
}
