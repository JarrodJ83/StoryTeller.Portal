using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using StoryTeller.ResultAggregation.Settings;

namespace StoryTeller.ResultAggregation.CommandHandlers
{
    public abstract class SqlCommandHandler
    {
        protected ISqlSettings _sqlSettings { get; }

        protected SqlCommandHandler(ISqlSettings sqlSettings)
        {
            _sqlSettings = sqlSettings;
        }

        protected async Task<TResult> ExecuteScalar<TResult>(string query, object parameters, CancellationToken cancellationToken)
        {
            using (var conn = new SqlConnection(_sqlSettings.ResultsDbConnStr))
            {
                await conn.OpenAsync(cancellationToken);

                return await conn.ExecuteScalarAsync<TResult>(query, parameters);
            }
        }
    }
}
