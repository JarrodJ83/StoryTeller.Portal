using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using StoryTeller.Portal.AppManagment.Models;
using StoryTeller.Portal.AppManagment.Queries;
using StoryTeller.Portal.CQRS;
using StoryTeller.Portal.CQRS.Sql;
using StoryTeller.ResultAggregation.CommandHandlers;

namespace StoryTeller.Portal.AppManagment.QueryHandlers
{
    public class AllAppsViaSql : SqlHandler, IQueryHandler<Queries.AllApps, List<App>>
    {
        public AllAppsViaSql(ISqlSettings sqlSettings) : base(sqlSettings)
        {
        }

        public async Task<List<App>> FetchAsync(AllApps qry, CancellationToken cancellationToken)
        {
            return await Query<App>("select id, name from App", new { }, cancellationToken);
        }
    }
}
