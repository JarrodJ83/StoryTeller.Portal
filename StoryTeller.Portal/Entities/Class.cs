using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StoryTeller.ResultAggregator.Entities
{
    public class Run
    {
        public int Id { get; set; }
        public DateTime RunDateTime { get; set; }
        public string Name { get; set; }
        public string HtmlResult { get; set; }
    }
}
