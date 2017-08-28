using System;
using System.Threading;
using System.Threading.Tasks;
using StoryTeller.Portal.CQRS;
using StoryTeller.Portal.CQRS.Sql;
using StoryTeller.ResultAggregation.Commands;

namespace StoryTeller.ResultAggregation.CommandHandlers
{
    public class UpdateRunViaSql : SqlHandler, ICommandHandler<Commands.UpdateRun>
    {
        public UpdateRunViaSql(ISqlSettings sqlSettings) : base(sqlSettings)
        {
        }

        public async Task ExecuteAsync(UpdateRun cmd, CancellationToken cancellationToken)
        {
            var qry = $@"update r
                         set r.{nameof(cmd.Run.Name)} = @{nameof(cmd.Run.Name)},
                             r.{nameof(cmd.Run.HtmlResults)} = @{nameof(cmd.Run.HtmlResults)},
                             r.{nameof(cmd.Run.RunDateTime)} = @{nameof(cmd.Run.RunDateTime)}                          
                         from Run as r
                         where r.Id = @{nameof(cmd.Run.Id)} and
                               r.AppId = @{nameof(cmd.AppId)}

                         select @@rowcount";

            var affectedRows = await ExecuteScalar<int>(qry, new { cmd.Run.Id, cmd.Run.Name, cmd.Run.HtmlResults, cmd.Run.RunDateTime, cmd.AppId }, cancellationToken);

            if(affectedRows == 0)
                throw new Exception("No updates");
        }
    }
}
