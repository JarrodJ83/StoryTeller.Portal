using System.Threading;
using System.Threading.Tasks;
using StoryTeller.Portal.CQRS;
using StoryTeller.Portal.CQRS.Sql;
using StoryTeller.ResultAggregation.CommandHandlers;
using StoryTeller.ResultAggregation.Models;
using StoryTeller.ResultAggregation.Queries;

namespace StoryTeller.ResultAggregation.QueryHandlers
{
    public class LatestRunByApplicationViaSql : SqlHandler, IQueryHandler<LatestRunByApplication, Run>
    {
        public LatestRunByApplicationViaSql(ISqlSettings sqlSettings) : base(sqlSettings)
        {
        }

        public async Task<Run> FetchAsync(LatestRunByApplication qry, CancellationToken cancellationToken)
        {
            return await QuerySingleOrDefaultAsync<Run>($@"
                            select top 1
                                {nameof(Run.Id)}, {nameof(Run.Name)}, {nameof(Run.RunDateTime)}, {nameof(Run.HtmlResults)}
                            from {nameof(Run)}
                            order by {nameof(Run.RunDateTime)} desc", cancellationToken);
        }

    }
}
