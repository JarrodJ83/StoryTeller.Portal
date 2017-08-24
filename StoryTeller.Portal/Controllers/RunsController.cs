using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using StoryTeller.Portal;
using StoryTeller.Portal.CQRS;
using StoryTeller.ResultAggregation.ClientModel;
using StoryTeller.ResultAggregation.Requests;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace StoryTeller.ResultAggregator.Controllers
{
    [Route("api/[controller]")]
    public class RunsController : Controller
    {
        private readonly IRequestHandler<AddRunRequest, int> _addRunRequest;
        private readonly IApiContext _apiContext;

        public RunsController(IRequestHandler<AddRunRequest, int> addRunRequest, IApiContext apiContext)
        {
            _addRunRequest = addRunRequest;
            _apiContext = apiContext;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]PostRun postRun)
        {
            var addRunRequest = new AddRunRequest(_apiContext.ApplicationId)
            {
                RunDateTime = postRun.RunDateTime,
                RunName = postRun.RunName
            };

            int key = await _addRunRequest.HandleAsync(addRunRequest, Request.HttpContext.RequestAborted);
            return Created(String.Empty, key);
        }
    }
}
