using System;
using System.Collections.Generic;
using System.Text;
using StoryTeller.Portal.AppManagment.Models;
using StoryTeller.Portal.CQRS;

namespace StoryTeller.Portal.AppManagment.Queries
{
    public class AllApps : IQuery<List<App>>
    {
    }
}
