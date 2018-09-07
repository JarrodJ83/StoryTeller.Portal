using Microsoft.AspNetCore.SignalR;
using StoryTeller.ResultAggregation.Events;
using System.Threading.Tasks;

namespace StoryTeller.Portal.Web.Hubs
{
    public class DashboardHub : Hub
    {
        public Task RunCreated(RunCreated runCreated)
        {
            return Clients.All.SendAsync(nameof(RunCreated), runCreated);
        }

        public Task RunCompleted(RunCompleted runCreated)
        {
            return Clients.All.SendAsync(nameof(RunCompleted), runCreated);
        }

        public Task RunSpecUpdated(RunSpecUpdated runCreated)
        {
            return Clients.All.SendAsync(nameof(RunSpecUpdated), runCreated);
        }
    }
}
