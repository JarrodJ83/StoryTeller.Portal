using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace StoryTeller.ResultAggregation.Models
{
    public class Spec
    {
        public int Id { get; set; }
        public Guid StoryTellerId { get; set; }
        public string Name { get; set; }
        [JsonIgnore]
        public int AppId { get; set; }
    }
}
