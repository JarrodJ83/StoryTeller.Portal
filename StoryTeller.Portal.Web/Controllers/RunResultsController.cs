using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StoryTeller.Portal.CQRS;
using StoryTeller.Portal.Models.Views;
using StoryTeller.Portal.Queries;
using Requests = StoryTeller.Portal.Requests;

namespace StoryTeller.Portal.Web.Controllers
{
    [Route("Runs")]
    public class ViewRunResultController : Controller
    {
        private readonly IQueryHandler<SummaryOfRun, RunSummary> _summaryOfRunQueryHandler;
        public IRequestHandler<Requests.RunSummaries, List<RunSummary>> _runSummaries { get; }

        public ViewRunResultController(IQueryHandler<SummaryOfRun, RunSummary> summaryOfRunQueryHandler, IRequestHandler<Requests.RunSummaries, List<RunSummary>> runSummaries)
        {
            _summaryOfRunQueryHandler = summaryOfRunQueryHandler;
            _runSummaries = runSummaries;
        }        

        [Route("{runId}/Results")]
        public async Task<IActionResult> Index(int runId)
        {
            RunSummary runSummary = await _summaryOfRunQueryHandler.FetchAsync(new SummaryOfRun(runId), CancellationToken.None);
            ViewBag.ResultsHtml = runSummary.HtmlResults;
            return View("ViewRunResults");
        }

        [Route("Summaries")]
        public async Task<IActionResult> RunSummaries(int[] appIds = null)
        {
            List<RunSummary> runSummaries = await _runSummaries.HandleAsync(new Requests.RunSummaries(), CancellationToken.None);

            return Ok(runSummaries);
        }
    }
}
