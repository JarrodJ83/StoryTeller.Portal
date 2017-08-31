using System.Collections.Generic;
using StoryTeller.Portal.Models;
using StoryTeller.Portal.CQRS;

namespace StoryTeller.Portal.Queries
{
    public class AllApps : IQuery<List<App>>
    {
    }
}
