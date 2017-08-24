using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using StoryTeller.Portal.CQRS;
using StoryTeller.ResultAggregation.Commands;
using StoryTeller.ResultAggregation.Settings;

namespace StoryTeller.ResultAggregation.CommandHandlers
{
    public class AddRunForApplicationViaSql : SqlCommand, ICommandHandler<Commands.AddRunForApplication>
    {
        public AddRunForApplicationViaSql(ISqlSettings sqlSettings) : base(sqlSettings)
        {
        }

        public async Task ExecuteAsync(AddRunForApplication cmd, CancellationToken cancellationToken)
        {
            var runid = await ExecuteScalar<int>($@"insert into Run ([ApplicationId], [Name],[RunDateTime]) 
                                                 values (@{nameof(cmd.ApplicationId)}, @{nameof(cmd.Run.Name)}, @{nameof(cmd.Run.RunDateTime)})
                                                 select @@identity", new {cmd.ApplicationId, cmd.Run.Name, cmd.Run.RunDateTime}, cancellationToken);

            cmd.Run.Id = runid;
        }
    }
}
