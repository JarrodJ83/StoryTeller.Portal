using MediatR;
using Microsoft.AspNetCore.SignalR;
using StoryTeller.Portal.CQRS;
using StoryTeller.Portal.Models.Views;
using StoryTeller.Portal.Web.Hubs;
using StoryTeller.ResultAggregation.Events;
using System.Threading;
using System.Threading.Tasks;

namespace StoryTeller.Portal.Web
{
    public class DashbaordEventsBroadcaster : INotificationHandler<RunCreated>, INotificationHandler<RunCompleted>, INotificationHandler<RunSpecUpdated>
    {
        public IHubContext<DashboardHub> DashboardHubContext { get; }
        public IQueryHandler<Queries.SummaryOfRun, RunSummary> RunSummary { get; }

        public DashbaordEventsBroadcaster(IHubContext<DashboardHub> dashboardHub, IQueryHandler<Queries.SummaryOfRun, RunSummary> runSummary)
        {
            DashboardHubContext = dashboardHub;
            RunSummary = runSummary;
        }        

        public async Task Handle(RunCreated runCreated, CancellationToken cancellationToken)
        {
            // TODO: IF we don't need the app name for display we could really do away with this query because we know it just started
            RunSummary runSummary = await RunSummary.FetchAsync(new Queries.SummaryOfRun(runCreated.Run.Id), cancellationToken);
            await DashboardHubContext.Clients.All.SendAsync(nameof(RunCreated), runSummary);
        }

        public async Task Handle(RunSpecUpdated runSpecUpdated, CancellationToken cancellationToken)
        {
            await DashboardHubContext.Clients.All.SendAsync(nameof(RunSpecUpdated), runSpecUpdated);
        }

        public async Task Handle(RunCompleted notification, CancellationToken cancellationToken)
        {
            await DashboardHubContext.Clients.All.SendAsync(nameof(RunCompleted), notification);
        }
    }
}
