using System.Collections.Generic;
using StoryTeller.ResultAggregation.ClientModel;
using StoryTeller.ResultAggregation.Models;

namespace StoryTeller.Portal.ResultsAggregator
{
    public interface IPortalResultsAggregatorClient
    {
        List<Spec> GetSpecs();
        Spec AddSpec(PostSpec spec);
        Run AddRun(PostRun run);
        void AddSpecToRun(PostRunSpec runSpec);
    }
}
