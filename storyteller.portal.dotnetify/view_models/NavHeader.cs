using DotNetify;
using StoryTeller.Portal.CQRS;
using StoryTeller.Portal.Models;
using StoryTeller.Portal.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace storyteller.portal.dotnetify.view_models
{
    public class NavHeader : BaseVM
    {
        public List<App> Apps = new List<App>();
        private readonly IQueryHandler<AllApps, List<App>> _allAppsQueryHandler;
        public NavHeader(IQueryHandler<AllApps, List<App>> allAppsQueryHandler)
        {
            _allAppsQueryHandler = allAppsQueryHandler;

            allAppsQueryHandler.FetchAsync(new AllApps(), CancellationToken.None)
                .ContinueWith(t =>
                {
                    if (t.IsCompletedSuccessfully)
                    {
                        Apps = t.Result;
                    }
                });
        }        
    }
}
