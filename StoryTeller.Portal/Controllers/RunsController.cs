using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StoryTeller.Portal.CQRS;
using StoryTeller.ResultAggregation.Requests;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace StoryTeller.ResultAggregator.Controllers
{
    [Route("api/[controller]")]
    public class RunsController : Controller
    {
        private readonly IRequestHandler<AddRunRequest, int> _addRunRequest;

        public RunsController(IRequestHandler<AddRunRequest, int> addRunRequest)
        {
            _addRunRequest = addRunRequest;
        }

        [HttpPost]
        public async Task Post([FromBody]AddRunRequest addRunRequest)
        {
            await _addRunRequest.HandleAsync(addRunRequest, Request.HttpContext.RequestAborted);
        }
    }
}
