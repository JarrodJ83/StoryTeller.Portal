using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using StoryTeller.Portal.CQRS;
using StoryTeller.Portal.CQRS.Sql;
using StoryTeller.Portal.Models.Views;
using StoryTeller.Portal.Queries;
using StoryTeller.ResultAggregation.CommandHandlers;

namespace StoryTeller.Portal.QueryHandlers
{
    public class SummaryForRunViaSql : SqlHandler, IQueryHandler<Queries.SummaryOfRun, RunSummary>
    {
        public SummaryForRunViaSql(ISqlSettings sqlSettings) : base(sqlSettings)
        {
        }

        public async Task<RunSummary> FetchAsync(SummaryOfRun qry, CancellationToken cancellationToken)
        {
            return await QuerySingleOrDefaultAsync<RunSummary>($@"
                            declare @successCount int, @failureCount int, @totalCount int

                            select @successCount = count(1)
                            from RunSpec
                            where runId = @runId and
                            passed = 1

                            select @failureCount = count(1)
                            from RunSpec
                            where runId = @runId and
                            passed = 0

                            select r.Id
                            ,r.Name
                            ,a.Id as [AppId]
                            ,a.Name as [AppName]
                            ,r.RunDateTime
                            ,r.HtmlResults
                            ,@successCount as [SuccessCount]
                            ,@failureCount as [FailureCount]
                            ,@totalCount as [TotalCount]
                            from Run as r
                                inner join App as a on r.AppId = a.Id
                            where r.Id = @runId
                        ", new {qry.RunId}, cancellationToken);
        }
    }
}
