using DotNetify;
using MediatR;
using StoryTeller.Portal.CQRS;
using StoryTeller.Portal.Models.Views;
using StoryTeller.Portal.Queries;
using StoryTeller.ResultAggregation.Events;
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
        private readonly IEventsHub _eventsHub;
        public List<RunStat> Stats { get; set; } = new List<RunStat>();

        public RunChart(IQueryHandler<LatestRunSumarries, List<RunSummary>> runSummariesQueryHandler, IEventsHub eventsHub)
        {
            _eventsHub = eventsHub;
            _runSummariesQueryHandler = runSummariesQueryHandler;

            if (!eventsHub.IsSubscribed<RunCompleted>(this))
                eventsHub.Subscribe<RunCompleted>(this, notification => Handle((RunCompleted)notification));

            RefreshStats();
        }

        public override void Dispose()
        {
            _eventsHub.UnSubscribe<RunCompleted>(this);

            base.Dispose();
        }

        public void Handle(RunCompleted notification)
        {
            RefreshStats();
        }

        void RefreshStats()
        {
            _runSummariesQueryHandler.FetchAsync(new LatestRunSumarries(), CancellationToken.None).ContinueWith(t =>
            {
                if (t.IsCompletedSuccessfully)
                {
                    List<int> daysInWeek = new List<int>();
                    var startingOn = DateTime.UtcNow.AddDays(-6);
                    for (int i = 0; i < 7; i++)
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
                                    Successful = day == null ? 0 : day.Count(x => x.Passed.HasValue && x.Passed.Value),
                                    Failed = day == null ? 0 : day.Count(x => x.Passed.HasValue && !x.Passed.Value)
                                };

                    Stats = stats.ToList();
                    Changed(nameof(Stats));
                    PushUpdates();
                }
            });
        }
    }

    public class RunStat
    {
        public int Day { get; set; }
        public int Successful { get; set; }
        public int Failed { get; set; }
    }
}
