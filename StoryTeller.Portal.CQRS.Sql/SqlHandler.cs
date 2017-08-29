using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using StoryTeller.Portal.CQRS.Sql;

namespace StoryTeller.ResultAggregation.CommandHandlers
{
    public abstract class SqlHandler
    {
        protected ISqlSettings _sqlSettings { get; }

        protected SqlHandler(ISqlSettings sqlSettings)
        {
            _sqlSettings = sqlSettings;
        }

        protected async Task<TResult> ExecuteScalarAsync<TResult>(string query, object parameters, CancellationToken cancellationToken)
        {
            using (var conn = new SqlConnection(_sqlSettings.ResultsDbConnStr))
            {
                await conn.OpenAsync(cancellationToken);

                return await conn.ExecuteScalarAsync<TResult>(query, parameters);
            }
        }

        protected async Task<List<TResult>> QueryAsync<TResult>(string query, object parameters, CancellationToken cancellationToken)
        {
            using (var conn = new SqlConnection(_sqlSettings.ResultsDbConnStr))
            {
                await conn.OpenAsync(cancellationToken);

                var results = await conn.QueryAsync<TResult>(query, parameters);

                return results.ToList();
            }
        }

        protected async Task<TResult> QuerySingleOrDefaultAsync<TResult>(string query, object parameters, CancellationToken cancellationToken)
        {
            using (var conn = new SqlConnection(_sqlSettings.ResultsDbConnStr))
            {
                await conn.OpenAsync(cancellationToken);

                return await conn.QuerySingleOrDefaultAsync<TResult>(query, parameters);
            }
        }
    }
}
