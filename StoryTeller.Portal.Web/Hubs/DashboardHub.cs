using MediatR;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using StoryTeller.ResultAggregation.Events;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace StoryTeller.Portal.Web.Hubs
{
    public class DashboardHub : Hub
    {
        public DashboardHub()
        {
            Debug.WriteLine("hub ctor");

        }
        public Task Send(string message)
        {
            return Clients.All.SendAsync("Send", message);
        }
    }
}
