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
    public class RunFeed : BaseVM
    {
        private readonly RunFeedDataSource _ds;

        public List<string> Runs => _ds.Runs;

        public RunFeed(RunFeedDataSource ds)
        {
            _ds = ds;
            _ds.PropertyChanged += (sender, args) =>
            {
                this.Changed(nameof(Runs));
                PushUpdates();
            };
        }
    }
}
