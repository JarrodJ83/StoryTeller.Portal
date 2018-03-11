using MediatR;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using StoryTeller.Portal.Web.Hubs;
using StoryTeller.ResultAggregation.Events;
using System.Threading;
using System.Threading.Tasks;

namespace StoryTeller.Portal.Web
{
    public class EventsBroadcaster : INotificationHandler<RunCreated>, INotificationHandler<RunCompleted>, INotificationHandler<RunSpecUpdated>
    {
        public IHubContext<DashboardHub> DashboardHubContext { get; }

        public EventsBroadcaster(IHubContext<DashboardHub> dashboardHub)
        {
            DashboardHubContext = dashboardHub;
        }        

        public async Task Handle(RunCreated notification, CancellationToken cancellationToken)
        {
            await DashboardHubContext.Clients.All.SendAsync(nameof(RunCreated), notification);
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
