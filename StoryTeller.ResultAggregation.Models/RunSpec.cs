using System;
using System.Collections.Generic;
using System.Text;

namespace StoryTeller.ResultAggregation.Models
{
    public class RunSpec
    {
        public int Id { get; set; }
        public int RunId { get; set; }
        public int SpecId { get; set; }
        public bool? Success { get; set; }
    }
}
