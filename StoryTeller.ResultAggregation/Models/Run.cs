using System;
using System.Collections.Generic;

namespace StoryTeller.ResultAggregation.Models
{
    public class Run
    {
        public App Application { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime RunDateTime { get; set; }
        public List<Spec> Specs { get; set; }
    }
}
