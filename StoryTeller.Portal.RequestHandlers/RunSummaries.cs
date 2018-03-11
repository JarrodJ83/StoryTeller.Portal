using StoryTeller.Portal.CQRS;
using StoryTeller.Portal.Models.Views;
using StoryTeller.Portal.Queries;
using Requests = StoryTeller.Portal.Requests;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace StoryTeller.Portal.RequestHandlers
{
    public class RunSummaries : IRequestHandler<Requests.RunSummaries, List<RunSummary>>
    {
        public IQueryHandler<LatestRunSumarries, List<RunSummary>> LatestRunSummaries { get; }

        public RunSummaries(IQueryHandler<LatestRunSumarries, List<RunSummary>> latestRunSummaries)
        {
            LatestRunSummaries = latestRunSummaries;
        }

        public async Task<List<RunSummary>> HandleAsync(Requests.RunSummaries request, CancellationToken cancellationToken)
        {
            return await LatestRunSummaries.FetchAsync(new LatestRunSumarries(), cancellationToken);
        }
    }
}