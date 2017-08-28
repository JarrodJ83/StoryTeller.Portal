
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using StoryTeller.Portal.AppManagment.Models;
using StoryTeller.Portal.AppManagment.Queries;
using StoryTeller.Portal.AppManagment.Requests;
using StoryTeller.Portal.CQRS;

namespace StoryTeller.Portal.AppManagment.RequestHandlers
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
