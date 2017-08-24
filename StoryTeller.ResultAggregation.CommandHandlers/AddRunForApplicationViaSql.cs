using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using StoryTeller.Portal.CQRS;
using StoryTeller.ResultAggregation.Commands;
using StoryTeller.ResultAggregation.Settings;

namespace StoryTeller.ResultAggregation.CommandHandlers
{
    public class AddRunForApplicationViaSql : ICommandHandler<Commands.AddRunForApplication, int>
    {
        private readonly ISqlSettings _sqlSettings;

        public AddRunForApplicationViaSql(ISqlSettings sqlSettings)
        {
            _sqlSettings = sqlSettings;
        }
        public async Task ExecuteAsync(AddRunForApplication cmd, CancellationToken cancellationToken)
        {
            using (var conn = new SqlConnection(_sqlSettings.ResultsDbConnStr))
            {
                await conn.OpenAsync(cancellationToken);
                cmd.Key = await conn.ExecuteScalarAsync<int>(
                    @"insert into Run ([ApplicationId], [Name],[RunDateTime]) values (@ApplicationId, @RunName, @RunDate)
                      select @@identity", new { cmd.ApplicationId, cmd.RunName, cmd.RunDate });
            }
        }
    }
}
