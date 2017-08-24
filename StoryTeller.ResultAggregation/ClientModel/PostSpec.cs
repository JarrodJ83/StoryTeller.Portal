using System;
using System.Collections.Generic;
using System.Text;

namespace StoryTeller.ResultAggregation.ClientModel
{
    public class PostSpec
    {
        public string Name { get; set; }
        public Guid StoryTellerId { get; set; }
    }
}
