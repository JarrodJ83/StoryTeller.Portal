using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using StoryTeller.Portal.Models;
using StoryTeller.Portal.Requests;
using StoryTeller.Portal.CQRS;

namespace StoryTeller.Portal.Web.Controllers
{
    [Route("[controller]")]
    public class AppsController : Controller
    {
        private readonly IRequestHandler<AllAppsRequest, List<App>> _allAppsRequestHandler;

        public AppsController(IRequestHandler<AllAppsRequest, List<App>> allAppsRequestHandler)
        {
            _allAppsRequestHandler = allAppsRequestHandler;
        }

        [HttpGet]
        public async Task<List<App>> GetAll()
        {
            return await _allAppsRequestHandler.HandleAsync(new AllAppsRequest(), Request.HttpContext.RequestAborted);
        }
    }
}
