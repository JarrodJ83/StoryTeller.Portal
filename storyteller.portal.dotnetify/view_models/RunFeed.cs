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
    public class RunFeed : BaseVM, IAsyncNotificationHandler<RunCreated>, IAsyncNotificationHandler<RunSpecUpdated>, IAsyncNotificationHandler<RunCompleted>, IRoutable
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
            await AddOrUpdateRunSummary(notification.RunId);
        }

        public async Task Handle(RunSpecUpdated notification)
        {
            await AddOrUpdateRunSummary(notification.RunId);
        }

        public async Task Handle(RunCompleted notification)
        {
            await AddOrUpdateRunSummary(notification.RunId);
        }

        private async Task AddOrUpdateRunSummary(int runId)
        {
            RunSummary latestRunSummary = await _summaryOfRunQueryHandler.FetchAsync(new SummaryOfRun(runId), CancellationToken.None);

            RunSummary outOfDateRunSummary = Runs.SingleOrDefault(r => r.Id == latestRunSummary.Id);

            if(outOfDateRunSummary != null)
                Runs.Remove(outOfDateRunSummary);

            Runs.Add(latestRunSummary);

            Runs = Runs.OrderByDescending(r => r.RunDateTime).ToList();

            Changed(nameof(Runs));
            PushUpdates();
        }

        public RoutingState RoutingState { get; set; }
    }
}
