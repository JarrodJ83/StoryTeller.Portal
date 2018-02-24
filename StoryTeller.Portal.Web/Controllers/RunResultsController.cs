using System.Threading;
using Microsoft.AspNetCore.Mvc;
using StoryTeller.Portal.CQRS;
using StoryTeller.Portal.Models.Views;
using StoryTeller.Portal.Queries;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace StoryTeller.Portal.Web.Controllers
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
