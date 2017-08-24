using System;
using System.Collections.Generic;
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
using StoryTeller.ResultAggregation.Models;
using StoryTeller.ResultAggregation.Requests;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace StoryTeller.ResultAggregator.Controllers
{
    [Route("api/[controller]")]
    public class RunsController : Controller
    {
        private readonly IRequestHandler<AddRunRequest, Run> _addRunRequest;
        private readonly IRequestHandler<AddSpecToRunRequest> _addSpecToRunRequestRequestHandler;
        private readonly IApiContext _apiContext;

        public RunsController(IApiContext apiContext, IRequestHandler<AddRunRequest, Run> addRunRequest, IRequestHandler<AddSpecToRunRequest> addSpecToRunRequestRequestHandler)
        {
            _addRunRequest = addRunRequest;
            _apiContext = apiContext;
            _addSpecToRunRequestRequestHandler = addSpecToRunRequestRequestHandler;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]PostRun postRun)
        {
            var addRunRequest = new AddRunRequest(_apiContext.ApplicationId, postRun);

            Run run = await _addRunRequest.HandleAsync(addRunRequest, Request.HttpContext.RequestAborted);

            return Created(String.Empty, run);
        }

        [HttpPost]
        [Route("{runId}/SpecBatches")]
        public async Task<IActionResult> PostRunSpecs([FromRoute]int runId, List<int> specIds)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        [Route("{runId}/Specs")]
        public async Task<IActionResult> PostRunSpecs([FromRoute]int runId, [FromBody]PostRunSpec postedRunSpec)
        {
            await _addSpecToRunRequestRequestHandler.HandleAsync(new AddSpecToRunRequest(_apiContext.ApplicationId, runId, postedRunSpec.SpecId), Request.HttpContext.RequestAborted);
            return Created(string.Empty, null);
        }
    }
}
