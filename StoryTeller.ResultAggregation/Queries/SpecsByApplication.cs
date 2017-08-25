using System.Collections.Generic;
using StoryTeller.Portal.CQRS;
using StoryTeller.ResultAggregation.Models;

namespace StoryTeller.ResultAggregation.Queries
{
    public class SpecsByApplication : ApplicationScoped, IQuery<List<Spec>>
    {
        public SpecsByApplication(int appId) : base(appId)
        {
        }
    }
}
