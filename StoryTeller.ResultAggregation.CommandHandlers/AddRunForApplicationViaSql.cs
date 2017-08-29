using System.Threading;
using System.Threading.Tasks;
using StoryTeller.Portal.CQRS;
using StoryTeller.Portal.CQRS.Sql;
using StoryTeller.ResultAggregation.Commands;

namespace StoryTeller.ResultAggregation.CommandHandlers
{
    public class AddRunForApplicationViaSql : SqlHandler, ICommandHandler<Commands.AddRunForApplication>
    {
        public AddRunForApplicationViaSql(ISqlSettings sqlSettings) : base(sqlSettings)
        {
        }

        public async Task ExecuteAsync(AddRunForApplication cmd, CancellationToken cancellationToken)
        {
            var runid = await ExecuteScalarAsync<int>($@"insert into Run ([AppId], [Name],[RunDateTime]) 
                                                 values (@{nameof(cmd.AppId)}, @{nameof(cmd.Run.Name)}, @{nameof(cmd.Run.RunDateTime)})
                                                 select @@identity", new {cmd.AppId, cmd.Run.Name, cmd.Run.RunDateTime}, cancellationToken);

            cmd.Run.Id = runid;
        }
    }
}
