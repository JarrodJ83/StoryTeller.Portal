using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DotNetify;
using DotNetify.Routing;
using MediatR;
using StoryTeller.Portal.CQRS;
using StoryTeller.Portal.Models.Views;
using StoryTeller.Portal.Queries;
using StoryTeller.ResultAggregation.Events;

namespace storyteller.portal.dotnetify.view_models
{
    public class RunFeed : BaseVM, IRoutable//, IAsyncNotificationHandler<RunCompleted>, IAsyncNotificationHandler<RunSpecUpdated>, IAsyncNotificationHandler<RunCreated>
    {
        private readonly IQueryHandler<LatestRunSumarries, List<RunSummary>> _runSummariesQueryHandler;
        private readonly IQueryHandler<SummaryOfRun, RunSummary> _summaryOfRunQueryHandler;
        private readonly IEventsHub _eventsHub;

        private const int MAX_RECORDS = 20;
        public List<RunSummary> Runs { get; set; } = new List<RunSummary>();

        public RunFeed(IQueryHandler<LatestRunSumarries, List<RunSummary>> runSummariesQueryHandler, 
            IQueryHandler<SummaryOfRun, RunSummary> summaryOfRunQueryHandler, IEventsHub eventsHub)
        {
            this.RegisterRoutes("index", new List<RouteTemplate>
            {
                new RouteTemplate("RunResults") {UrlPattern = "RunResults(/id)"},
            });

            _eventsHub = eventsHub;

            if (!_eventsHub.IsSubscribed<RunCreated>(this))
                _eventsHub.Subscribe<RunCreated>(this, notification => Handle((RunCreated)notification));

            if (!_eventsHub.IsSubscribed<RunCompleted>(this))
                _eventsHub.Subscribe<RunCompleted>(this, notification => Handle((RunCompleted)notification));

            if (!_eventsHub.IsSubscribed<RunSpecUpdated>(this))
                _eventsHub.Subscribe<RunSpecUpdated>(this, notification => Handle((RunSpecUpdated)notification));

            _runSummariesQueryHandler = runSummariesQueryHandler;
            _summaryOfRunQueryHandler = summaryOfRunQueryHandler;
            
            _runSummariesQueryHandler.FetchAsync(new LatestRunSumarries(), CancellationToken.None).ContinueWith(t =>
            {
                if (t.IsCompletedSuccessfully)
                {
                    Runs.AddRange(t.Result.Take(MAX_RECORDS));
                    Changed(nameof(Runs));
                    PushUpdates();
                }
            });
        }

        public override void Dispose()
        {
            _eventsHub.UnSubscribe<RunCreated>(this);
            _eventsHub.UnSubscribe<RunCompleted>(this);
            _eventsHub.UnSubscribe<RunSpecUpdated>(this);

            base.Dispose();
        }

        private void AddOrUpdateRunSummary(int runId)
        {
            _summaryOfRunQueryHandler.FetchAsync(new SummaryOfRun(runId), CancellationToken.None)
                .ContinueWith(t =>
                {
                    if (t.IsCompletedSuccessfully)
                    {
                        RunSummary latestRunSummary = t.Result;
                        RunSummary outOfDateRunSummary = Runs.SingleOrDefault(r => r.Id == latestRunSummary.Id);

                        if (outOfDateRunSummary != null)
                        {
                            Runs[Runs.IndexOf(outOfDateRunSummary)] = latestRunSummary;
                        }
                        else
                        {
                            Runs.Insert(0, latestRunSummary);
                        }

                        Runs = Runs.OrderByDescending(r => r.RunDateTime).Take(MAX_RECORDS).ToList();

                        Changed(nameof(Runs));
                        PushUpdates();
                    }
                });
        }

        public void Handle(RunCompleted notification)
        {
            AddOrUpdateRunSummary(notification.RunId);
        }

        public RoutingState RoutingState { get; set; }

        public void Handle(RunSpecUpdated notification)
        {
            AddOrUpdateRunSummary(notification.RunId);
        }

        public void Handle(RunCreated notification)
        {
            AddOrUpdateRunSummary(notification.RunId);
        }
    }
}
