
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using StoryTeller.Portal.Models;
using StoryTeller.Portal.Queries;
using StoryTeller.Portal.Requests;
using StoryTeller.Portal.CQRS;

namespace StoryTeller.Portal.RequestHandlers
{
    public class AllAppsRequestHandler : IRequestHandler<Requests.AllAppsRequest, List<App>>
    {
        private readonly IQueryHandler<Queries.AllApps, List<App>> _allAppsQueryHandler;

        public AllAppsRequestHandler(IQueryHandler<AllApps, List<App>> allAppsQueryHandler)
        {
            _allAppsQueryHandler = allAppsQueryHandler;
        }

        public async Task<List<App>> HandleAsync(AllAppsRequest request, CancellationToken cancellationToken)
        {
            return await _allAppsQueryHandler.FetchAsync(new AllApps(), cancellationToken);
        }
    }
}
