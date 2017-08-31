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
    public class RunFeed : BaseVM, IAsyncNotificationHandler<RunCreated>, IAsyncNotificationHandler<RunSpecUpdated>, IRoutable
    {
        private readonly IQueryHandler<LatestRunSumarries, List<RunSummary>> _runSummariesQueryHandler;
        private readonly IQueryHandler<SummaryOfRun, RunSummary> _summaryOfRunQueryHandler;
        public List<RunSummary> Runs { get; set; } = new List<RunSummary>();

        public RunFeed(IQueryHandler<LatestRunSumarries, List<RunSummary>> runSummariesQueryHandler, IQueryHandler<SummaryOfRun, RunSummary> summaryOfRunQueryHandler)
        {
            this.RegisterRoutes("index", new List<RouteTemplate>
            {
                new RouteTemplate("RunResults") {UrlPattern = "RunResults(/id)"},
            });

            _runSummariesQueryHandler = runSummariesQueryHandler;
            _summaryOfRunQueryHandler = summaryOfRunQueryHandler;
            _runSummariesQueryHandler.FetchAsync(new LatestRunSumarries(), CancellationToken.None).ContinueWith(t =>
            {
                if (t.IsCompletedSuccessfully)
                {
                    Runs.AddRange(t.Result);
                    Changed(nameof(Runs));
                    PushUpdates();
                }
            });
        }

        public async Task Handle(RunCreated notification)
        {
            RunSummary runSummary = await _summaryOfRunQueryHandler.FetchAsync(new SummaryOfRun(notification.RunId), CancellationToken.None);
           
            AddAndReorder(runSummary);
        }

        public async Task Handle(RunSpecUpdated notification)
        {
            var latestRunSummary = await _summaryOfRunQueryHandler.FetchAsync(new SummaryOfRun(notification.RunId), CancellationToken.None);

            var outOfDateRunSummary = Runs.Single(r => r.Id == latestRunSummary.Id);

            Runs.Remove(outOfDateRunSummary);
            AddAndReorder(latestRunSummary);
        }

        private void AddAndReorder(RunSummary runSummary)
        {
            Runs.Add(runSummary);

            Runs = Runs.OrderByDescending(r => r.RunDateTime).ToList();

            Changed(nameof(Runs));
            PushUpdates();
        }

        public RoutingState RoutingState { get; set; }
    }
}
