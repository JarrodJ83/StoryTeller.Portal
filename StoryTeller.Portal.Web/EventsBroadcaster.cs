using MediatR;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using StoryTeller.Portal.Web.Hubs;
using StoryTeller.ResultAggregation.Events;
using System.Threading;
using System.Threading.Tasks;

namespace StoryTeller.Portal.Web
{
    public class EventsBroadcaster : INotificationHandler<RunCreated>
    {
        public EventsBroadcaster(IHubContext<DashboardHub> dashboardHub)
        {
            DashboardHubContext = dashboardHub;
        }

        public IHubContext<DashboardHub>DashboardHubContext { get; }

        public async Task Handle(RunCreated notification, CancellationToken cancellationToken)
        {
            await DashboardHubContext.Clients.All.SendAsync("Send", JsonConvert.SerializeObject(notification));
        }
    }
}
