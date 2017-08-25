using System;
using System.Threading;
using System.Threading.Tasks;
using StoryTeller.Portal.CQRS;
using StoryTeller.ResultAggregation.Settings;

namespace StoryTeller.ResultAggregation.CommandHandlers
{
    public class UpdateRunSpec : SqlCommandHandler, ICommandHandler<Commands.UpdateRunSpec>
    {
        public UpdateRunSpec(ISqlSettings sqlSettings) : base(sqlSettings)
        {
        }

        public async Task ExecuteAsync(Commands.UpdateRunSpec cmd, CancellationToken cancellationToken)
        {
            var affectedRows = await ExecuteScalar<int>($@"update rs                                                        
                                                            set Passed = @{nameof(cmd.Passed)}
                                                           from RunSpec as rs
                                                            inner join Run as r on rs.RunId = r.Id
                                                           where rs.RunId = @{nameof(cmd.RunId)}
                                                            and rs.SpecId = @{nameof(cmd.SpecId)}
                                                            and r.AppId = @{nameof(cmd.AppId)};

                                                            select @@ROWCOUNT", cmd, cancellationToken);

            if(affectedRows == 0)
                throw new Exception("No records updated");
        }
    }
}
