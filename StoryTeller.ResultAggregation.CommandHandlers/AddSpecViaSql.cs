using System.Threading;
using System.Threading.Tasks;
using StoryTeller.Portal.CQRS;
using StoryTeller.Portal.CQRS.Sql;
using StoryTeller.ResultAggregation.Commands;

namespace StoryTeller.ResultAggregation.CommandHandlers
{
    public class AddSpecViaSql : SqlHandler, ICommandHandler<Commands.AddSpec, int>
    {
        public AddSpecViaSql(ISqlSettings sqlSettings) : base(sqlSettings)
        {
        }

        public async Task ExecuteAsync(AddSpec cmd, CancellationToken cancellationToken)
        {
            var qry = $@"insert into Spec ([AppId], [Name], [StoryTellerId]) 
                         values (@{nameof(cmd.Spec.AppId)}, @{nameof(cmd.Spec.Name)}, @{nameof(cmd.Spec.StoryTellerId)}) 
                         select @@identity";

            var appId = await ExecuteScalar<int>(qry, new { cmd.Spec.AppId, cmd.Spec.Name, cmd.Spec.StoryTellerId }, cancellationToken);

            cmd.Spec.Id = appId;
        }
    }
}
