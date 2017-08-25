using System;

namespace StoryTeller.ResultAggregation.Models.ClientModel
{
    public class PostSpec
    {
        public string Name { get; set; }
        public Guid StoryTellerId { get; set; }
    }
}
