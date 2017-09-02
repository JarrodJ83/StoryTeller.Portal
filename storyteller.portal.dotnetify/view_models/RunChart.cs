using DotNetify;
using StoryTeller.Portal.CQRS;
using StoryTeller.Portal.Models.Views;
using StoryTeller.Portal.Queries;
using StoryTeller.ResultAggregation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace storyteller.portal.dotnetify.view_models
{
    public class RunChart : BaseVM
    {
        private readonly IQueryHandler<LatestRunSumarries, List<RunSummary>> _runSummariesQueryHandler;
        public List<RunStat> Stats { get; set; } = new List<RunStat>();

        public RunChart(IQueryHandler<LatestRunSumarries, List<RunSummary>> runSummariesQueryHandler)
        {
            _runSummariesQueryHandler = runSummariesQueryHandler;
            _runSummariesQueryHandler.FetchAsync(new LatestRunSumarries(), CancellationToken.None).ContinueWith(t =>
            {
                if (t.IsCompletedSuccessfully)
                {
                    List<int> daysInWeek = new List<int>();
                    var startingOn = DateTime.UtcNow.AddDays(-6);
                    for(int i = 0; i < 7; i ++)
                    {
                        daysInWeek.Add(startingOn.Day);
                        startingOn = startingOn.AddDays(1);
                    }

                    var groupedRuns = from r in t.Result
                                      group r by r.RunDateTime.Day into byDay
                                      select byDay;
                    
                    var stats = from d in daysInWeek
                                join r in groupedRuns on d equals r.Key into dr
                                from day in dr.DefaultIfEmpty()
                                select new RunStat
                                {
                                    Day = d,
                                    SuccessfulCount = day == null ? 0 : day.Sum(x => x.SuccessfulCount),
                                    FailureCount = day == null ? 0 : day.Sum(s => s.FailureCount)
                                };
                    
                    Stats.AddRange(stats);
                    Changed(nameof(Stats));
                    PushUpdates();
                }
            });
        }
    }

    public class RunStat
    {
        public int Day { get; set; }
        public int SuccessfulCount { get; set; }
        public int FailureCount { get; set; }
    }
}
