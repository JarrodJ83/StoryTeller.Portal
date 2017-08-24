using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using StoryTeller.Portal.CQRS;
using StoryTeller.ResultAggregation.Commands;
using StoryTeller.ResultAggregation.Settings;

namespace StoryTeller.ResultAggregation.CommandHandlers
{
    public class AddSpecViaSql : SqlCommandHandler, ICommandHandler<Commands.AddSpec, int>
    {
        public AddSpecViaSql(ISqlSettings sqlSettings) : base(sqlSettings)
        {
        }

        public async Task ExecuteAsync(AddSpec cmd, CancellationToken cancellationToken)
        {
            var qry = $@"insert into Spec ([AppId], [Name], [StoryTellerId]) 
                         values (@{nameof(cmd.Spec.AppId)}, @{nameof(cmd.Spec.Name)}, @{nameof(cmd.Spec.StoryTellerId)}) 
                         select @@identity";

            var appId = await ExecuteScalar<int>(qry, new { ApplicationId = cmd.Spec.AppId, cmd.Spec.Name, cmd.Spec.StoryTellerId }, cancellationToken);

            cmd.Spec.Id = appId;
        }
    }
}
