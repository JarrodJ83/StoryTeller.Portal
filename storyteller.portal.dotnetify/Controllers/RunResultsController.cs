using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StoryTeller.Portal.CQRS;
using StoryTeller.Portal.Models.Views;
using StoryTeller.Portal.Queries;
using StoryTeller.Portal.QueryHandlers;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace storyteller.portal.dotnetify.Controllers
{
    [Route("Runs")]
    public class ViewRunResultController : Controller
    {
        private readonly IQueryHandler<SummaryOfRun, RunSummary> _summaryOfRunQueryHandler;

        public ViewRunResultController(IQueryHandler<SummaryOfRun, RunSummary> summaryOfRunQueryHandler)
        {
            _summaryOfRunQueryHandler = summaryOfRunQueryHandler;
        }
        [Route("{runId}/Results")]
        public IActionResult Index(int runId)
        {
            var runSummar = _summaryOfRunQueryHandler.FetchAsync(new SummaryOfRun(runId), CancellationToken.None)
                .Result;
            ViewBag.ResultsHtml = runSummar.HtmlResults;
            return View("ViewRunResults");
        }
    }
}
