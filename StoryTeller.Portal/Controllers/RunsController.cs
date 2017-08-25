using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StoryTeller.Portal;
using StoryTeller.Portal.CQRS;
using StoryTeller.ResultAggregation.Models.ClientModel;
using StoryTeller.ResultAggregation.Models;
using StoryTeller.ResultAggregation.Requests;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace StoryTeller.ResultAggregator.Controllers
{
    [Route("api/[controller]")]
    public class RunsController : Controller
    {
        private readonly IRequestHandler<AddRunRequest, Run> _addRunRequest;
        private readonly IRequestHandler<AddSpecToRunRequest, RunSpec> _addSpecToRunRequestRequestHandler;
        private readonly IRequestHandler<AddSpecBatchToRunRequest, List<RunSpec>> _addSpecBatchToRunRequestRequestHandler;
        private readonly IRequestHandler<PutRunSpecRequest> _putRunSpecRequestHandler;
        private readonly IApiContext _apiContext;

        public RunsController(IApiContext apiContext, 
            IRequestHandler<AddRunRequest, Run> addRunRequest, 
            IRequestHandler<AddSpecToRunRequest, RunSpec> addSpecToRunRequestRequestHandler, 
            IRequestHandler<AddSpecBatchToRunRequest, List<RunSpec>> addSpecBatchToRunRequestRequestHandler, IRequestHandler<PutRunSpecRequest> putRunSpecRequestHandler)
        {
            _addRunRequest = addRunRequest;
            _apiContext = apiContext;
            _addSpecToRunRequestRequestHandler = addSpecToRunRequestRequestHandler;
            _addSpecBatchToRunRequestRequestHandler = addSpecBatchToRunRequestRequestHandler;
            _putRunSpecRequestHandler = putRunSpecRequestHandler;
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
        public async Task<IActionResult> PostRunSpecs([FromRoute]int runId, [FromBody]PostRunSpecBatch postedRunSpecBatch)
        {
            List<RunSpec> runSpecs = await _addSpecBatchToRunRequestRequestHandler.HandleAsync(
                new AddSpecBatchToRunRequest(_apiContext.ApplicationId, runId, postedRunSpecBatch.SpecIds), Request.HttpContext.RequestAborted);
            return Created(string.Empty, runSpecs);
        }

        [HttpPost]
        [Route("{runId}/Specs")]
        public async Task<IActionResult> PostRunSpecs([FromRoute]int runId, [FromBody]PostRunSpec postedRunSpec)
        {
            RunSpec runSpec = await _addSpecToRunRequestRequestHandler.HandleAsync(new AddSpecToRunRequest(_apiContext.ApplicationId, runId, postedRunSpec.SpecId), Request.HttpContext.RequestAborted);
            return Created(string.Empty, runSpec);
        }

        [HttpPut]
        [Route("{runId}/Specs/{specId}")]
        public async Task<IActionResult> PutRunSpecs([FromRoute] int runId, [FromRoute] int specID, [FromBody] PutRunSpec postedRunSpec)
        {
            var runSpec = new RunSpec
            {
                RunId = runId,
                SpecId = specID,
                Success = postedRunSpec.Passed
            };
            await _putRunSpecRequestHandler.HandleAsync(new PutRunSpecRequest(_apiContext.ApplicationId, runSpec), Request.HttpContext.RequestAborted);
            return NoContent();
        }
    }
}
