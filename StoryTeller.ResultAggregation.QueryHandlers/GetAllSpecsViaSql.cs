using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using StoryTeller.Portal.CQRS;
using StoryTeller.ResultAggregation.Models;
using StoryTeller.ResultAggregation.Queries;
using Dapper;
using StoryTeller.Portal.CQRS.Sql;
using StoryTeller.ResultAggregation.CommandHandlers;

namespace StoryTeller.ResultAggregation.QueryHandlers
{
    public class GetAllSpecsViaSql : SqlHandler, IQueryHandler<Queries.SpecsByApplication, List<Spec>>
    {
        public GetAllSpecsViaSql(ISqlSettings sqlSettings) : base(sqlSettings)
        {
        }

        public async Task<List<Spec>> FetchAsync(SpecsByApplication qry, CancellationToken cancellationToken)
        {
            return await QueryAsync<Spec>($@"select id, storytellerid, name, appId
                      from spec where appId = @{nameof(qry.AppId)}", new {qry.AppId}, cancellationToken);
        }
    }
}
