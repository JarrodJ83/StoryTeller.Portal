using Newtonsoft.Json;

namespace StoryTeller.ResultAggregation.Models
{
    public class RunSpec
    {
        [JsonIgnore]
        public int Id { get; set; }
        public int RunId { get; set; }
        public int SpecId { get; set; }
        public bool? Success { get; set; }
    }
}
