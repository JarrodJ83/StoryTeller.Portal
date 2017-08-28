using System.Collections.Generic;
using StoryTeller.Portal.AppManagment.Models;
using StoryTeller.Portal.CQRS;

namespace StoryTeller.Portal.AppManagment.Requests
{
    public class AllAppsRequest : IRequest<List<App>>
    {
    }
}
