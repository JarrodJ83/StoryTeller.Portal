using System;
using System.Threading;
using System.Threading.Tasks;
using StoryTeller.Portal.CQRS;
using StoryTeller.Portal.CQRS.Sql;
using StoryTeller.ResultAggregation.Commands;
using StoryTeller.ResultAggregation.Models;

namespace StoryTeller.ResultAggregation.CommandHandlers
{
    public class AddRunResultViaSql : SqlHandler, ICommandHandler<AddRunResult>
    {
        public AddRunResultViaSql(ISqlSettings sqlSettings) : base(sqlSettings)
        {
        }

        public async Task ExecuteAsync(AddRunResult cmd, CancellationToken cancellationToken)
        {
            var rowsAffected = await base.ExecuteScalarAsync<int>($@"
                    insert into {nameof(RunResult)} ({nameof(RunResult.RunId)}, {nameof(RunResult.HtmlResults)}, {nameof(RunResult.Passed)})
                    select @{nameof(RunResult.RunId)}, @{nameof(RunResult.HtmlResults)}, @{nameof(RunResult.Passed)}
                    from {nameof(Run)} as r
                        inner join App as a on r.AppId = a.Id
                    where r.{nameof(Run.Id)} = @{nameof(cmd.RunResult.RunId)}

                    select @@Rowcount
            ", new
            {
                cmd.AppId,
                cmd.RunResult.RunId,
                cmd.RunResult.HtmlResults,
                cmd.RunResult.Passed
            }, cancellationToken);

            if(rowsAffected == 0)
                throw new Exception($"Run {cmd.RunResult.RunId} does not exist or could not be updated");
        }
    }
}
