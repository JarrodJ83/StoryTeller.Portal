using System.Collections.Generic;
using System.Threading.Tasks;
using StoryTeller.ResultAggregation.Models.ClientModel;
using StoryTeller.ResultAggregation.Models;

namespace StoryTeller.Portal.ResultsAggregator.Client
{
    public interface IPortalResultsAggregatorClient
    {
        Task<List<Spec>> GetSpecsAsync();
        Task<Spec> AddSpecAsync(PostSpec spec);
        Task<Run> AddRunAsync(PostRun run);
        Task AddSpecsToRunAsync(int runId, PostRunSpecBatch runSpecBatch);
        Task UpdateRunSpecAsync(int runId, int specId, PutRunSpec runSpec);
        Task UpdateRunAsync(Run run);
    }
}