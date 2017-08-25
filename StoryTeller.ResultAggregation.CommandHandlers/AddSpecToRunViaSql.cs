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
    public class AddSpecToRunViaSql : SqlCommandHandler, ICommandHandler<AddSpecToRun>
    {
        public AddSpecToRunViaSql(ISqlSettings sqlSettings) : base(sqlSettings)
        {
        }

        public async Task ExecuteAsync(AddSpecToRun cmd, CancellationToken cancellationToken)
        {
            var result = await ExecuteScalar<int?>($@"
                                    insert into RunSpec (runId, specId)
                                    select r.id, @{nameof(cmd.RunSpec.SpecId)}
                                    from Run as r
                                    inner join App as a on r.AppId = a.Id                                    
                                    where a.Id = @{nameof(cmd.AppId)} and r.id = @{nameof(cmd.RunSpec.RunId)}
                                    select @@identity", new {cmd.RunSpec.RunId, cmd.RunSpec.SpecId, cmd.AppId}, cancellationToken);

            if(result == null)
                throw new Exception($"Run {cmd.RunSpec.RunId} either does not exist or is not associated with App {cmd.AppId}");

            cmd.RunSpec.Id = result.Value;
        }
    }
}
