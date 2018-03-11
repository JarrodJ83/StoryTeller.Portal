using MediatR;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
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
            RunSummary runSummary = await RunSummary.FetchAsync(new Queries.SummaryOfRun(runCreated.RunId), cancellationToken);
            await DashboardHubContext.Clients.All.SendAsync(nameof(RunCreated), runSummary);
        }

        public async Task Handle(RunSpecUpdated notification, CancellationToken cancellationToken)
        {
            await DashboardHubContext.Clients.All.SendAsync(nameof(RunSpecUpdated), notification);
        }

        public async Task Handle(RunCompleted notification, CancellationToken cancellationToken)
        {
            await DashboardHubContext.Clients.All.SendAsync(nameof(RunCompleted), notification);
        }
    }
}
