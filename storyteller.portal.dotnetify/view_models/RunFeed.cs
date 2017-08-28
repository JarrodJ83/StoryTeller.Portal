using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DotNetify;
using MediatR;
using StoryTeller.ResultAggregation.Events;

namespace storyteller.portal.dotnetify.view_models
{
    public class RunFeed : BaseVM, IAsyncNotificationHandler<RunCreated>
    {
        public List<string> Runs => new List<string>
        {
            "Jarrod"
        };
        
        public RunFeed()
        {
        }

        public async Task Handle(RunCreated notification)
        {
            Runs.Add(notification.RunId.ToString());
        }
    }
}
