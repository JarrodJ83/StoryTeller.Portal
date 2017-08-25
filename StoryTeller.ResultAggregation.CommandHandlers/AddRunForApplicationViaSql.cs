using System.Threading;
using System.Threading.Tasks;
using StoryTeller.Portal.CQRS;
using StoryTeller.ResultAggregation.Commands;
using StoryTeller.ResultAggregation.Settings;

namespace StoryTeller.ResultAggregation.CommandHandlers
{
    public class AddRunForApplicationViaSql : SqlCommandHandler, ICommandHandler<Commands.AddRunForApplication>
    {
        public AddRunForApplicationViaSql(ISqlSettings sqlSettings) : base(sqlSettings)
        {
        }

        public async Task ExecuteAsync(AddRunForApplication cmd, CancellationToken cancellationToken)
        {
            var runid = await ExecuteScalar<int>($@"insert into Run ([AppId], [Name],[RunDateTime]) 
                                                 values (@{nameof(cmd.AppId)}, @{nameof(cmd.Run.Name)}, @{nameof(cmd.Run.RunDateTime)})
                                                 select @@identity", new {cmd.AppId, cmd.Run.Name, cmd.Run.RunDateTime}, cancellationToken);

            cmd.Run.Id = runid;
        }
    }
}
