﻿using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using StoryTeller.Portal.CQRS;
using StoryTeller.Portal.CQRS.Sql;
using StoryTeller.Portal.Models.Views;
using StoryTeller.Portal.Queries;
using StoryTeller.ResultAggregation.CommandHandlers;

namespace StoryTeller.Portal.QueryHandlers
{
    public class LatestRunSummariesViaSql : SqlHandler, IQueryHandler<Queries.LatestRunSumarries, List<RunSummary>>
    {
        public LatestRunSummariesViaSql(ISqlSettings sqlSettings) : base(sqlSettings)
        {
        }

        public async Task<List<RunSummary>> FetchAsync(LatestRunSumarries qry, CancellationToken cancellationToken)
        {
            return await QueryAsync<RunSummary>($@"
                            select r.Id
                            ,r.Name
                            ,a.Id as [AppId]
                            ,a.Name as [AppName]
                            ,r.RunDateTime
                            ,rr.HtmlResults
                            ,rr.Passed
                            ,(select count(1)
                                from RunSpec as rs
                                where rs.RunId = r.Id and
                                passed = 1) as [SuccessfulCount]
                            ,(select count(1)
                                from RunSpec as rs
                                where rs.RunId = r.Id and
                                passed = 0) as [FailureCount]
                            ,(select count(1)
                                from RunSpec as rs
                                where rs.RunId = r.Id) as [TotalCount]
                            from Run as r
                                inner join App as a on r.AppId = a.Id
                                left outer join RunResult as rr on r.Id = rr.RunId
                            where r.RunDateTime >= DATEADD(DAY, -6, GETUTCDATE())
                            order by r.RunDateTime desc
                        ", new { }, cancellationToken); 
        }
    }
}
