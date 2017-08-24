using System;
using System.Collections.Generic;
using System.Text;

namespace StoryTeller.ResultAggregation.Models
{
    public class Spec
    {
        public int Id { get; set; }
        public Guid StoryTellerId { get; set; }
        public string Name { get; set; }
    }
}
