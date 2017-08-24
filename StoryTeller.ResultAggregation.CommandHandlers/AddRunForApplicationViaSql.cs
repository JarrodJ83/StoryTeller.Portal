using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using StoryTeller.Portal.CQRS;
using StoryTeller.ResultAggregation.Commands;
using StoryTeller.ResultAggregation.Settings;

namespace StoryTeller.ResultAggregation.CommandHandlers
{
    public class AddRunForApplicationViaSql : SqlCommand, ICommandHandler<Commands.AddRunForApplication, int>
    {
        public AddRunForApplicationViaSql(ISqlSettings sqlSettings) : base(sqlSettings)
        {
        }

        public async Task ExecuteAsync(AddRunForApplication cmd, CancellationToken cancellationToken)
        {
            cmd.Key = await ExecuteScalar<int>(@"insert into Run ([ApplicationId], [Name],[RunDateTime]) values (@ApplicationId, @RunName, @RunDate)
                               select @@identity", new {cmd.ApplicationId, cmd.RunName, cmd.RunDate}, cancellationToken);
        }
    }
}
