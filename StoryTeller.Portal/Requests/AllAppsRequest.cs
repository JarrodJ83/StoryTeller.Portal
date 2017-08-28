using System.Collections.Generic;
using StoryTeller.Portal.Models;
using StoryTeller.Portal.CQRS;

namespace StoryTeller.Portal.Requests
{
    public class AllAppsRequest : IRequest<List<App>>
    {
    }
}
