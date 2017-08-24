using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using StoryTeller.Portal.CQRS;
using StoryTeller.ResultAggregation.Models;
using StoryTeller.ResultAggregation.Queries;
using StoryTeller.ResultAggregation.Settings;
using Dapper;

namespace StoryTeller.ResultAggregation.QueryHandlers
{
    public class GetAllSpecsViaSql : IQueryHandler<Queries.SpecsByApplication, List<Spec>>
    {
        private readonly ISqlSettings _sqlSettings;

        public GetAllSpecsViaSql(ISqlSettings sqlSettings)
        {
            _sqlSettings = sqlSettings;
        }

        public async Task<List<Spec>> FetchAsynx(SpecsByApplication qry, CancellationToken cancellationToken)
        {
            using (var conn = new SqlConnection(_sqlSettings.ResultsDbConnStr))
            {
                await conn.OpenAsync(cancellationToken);
                IEnumerable<Spec> specs = await conn.QueryAsync<Spec>(
                    @"select id, storytellerid, name, appId
                      from spec where appId = @applicationId", new { qry.ApplicationId });

                return specs.ToList();
            }
        }
    }
}
