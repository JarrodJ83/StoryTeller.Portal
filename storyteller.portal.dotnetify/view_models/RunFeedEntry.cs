using System.Threading.Tasks;
using DotNetify;
using MediatR;
using StoryTeller.ResultAggregation.Events;
using StoryTeller.ResultAggregation.Models;
using StoryTeller.Portal.CQRS;
using StoryTeller.Portal.Queries;
using StoryTeller.Portal.Models.Views;
using System.Threading;

namespace storyteller.portal.dotnetify.view_models
{
    public class RunFeedEntry : BaseVM
    {
        private readonly IQueryHandler<SummaryOfRun, RunSummary> _summaryOfRunQueryHandler;

        public RunSummary RunSummary { get; set; }

        public RunFeedEntry(IQueryHandler<SummaryOfRun, RunSummary> summaryOfRunQueryHandler)
        {
            _summaryOfRunQueryHandler = summaryOfRunQueryHandler;
        }


    }
}
