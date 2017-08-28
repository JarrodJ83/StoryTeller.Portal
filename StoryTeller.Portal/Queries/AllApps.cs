using System;
using System.Collections.Generic;
using System.Text;
using StoryTeller.Portal.Models;
using StoryTeller.Portal.CQRS;

namespace StoryTeller.Portal.Queries
{
    public class AllApps : IQuery<List<App>>
    {
    }
}
