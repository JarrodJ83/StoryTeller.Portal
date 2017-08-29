using System.Collections.Generic;
using System.Threading.Tasks;
using StoryTeller.ResultAggregation.Models.ClientModel;
using StoryTeller.ResultAggregation.Models;

namespace StoryTeller.Portal.ResultsAggregator.Client
{
    public interface IPortalResultsAggregatorClient
    {
        Task<List<Spec>> GetAllSpecsAsync();
        Task<Spec> AddSpecAsync(PostSpec spec);
        Task<Run> StartNewRunAsync(StartNewRun run);
        Task PassFailRunSpecAsync(PassFailRunSpec runSpec);
        Task UpdateRunAsync(Run run);
    }
}