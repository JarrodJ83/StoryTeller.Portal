using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace StoryTeller.Portal.Web.Hubs
{
    public class DashboardHub : Hub
    {
        public Task Send(string message)
        {
            return Clients.All.SendAsync("Send", message);
        }
    }
}
