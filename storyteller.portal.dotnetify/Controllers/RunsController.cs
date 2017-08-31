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
        private readonly IRequestHandler<PutRunRequest> _putRunRequestHandler;
        private readonly IRequestHandler<GetLatestRunRequest, Run> _getLatestRunRequestHandler;
        private readonly IApiContext _apiContext;

        public RunsController(IApiContext apiContext, 
            IRequestHandler<AddRunRequest, Run> addRunRequest, 
            IRequestHandler<AddSpecToRunRequest, RunSpec> addSpecToRunRequestRequestHandler, 
            IRequestHandler<AddSpecBatchToRunRequest, List<RunSpec>> addSpecBatchToRunRequestRequestHandler, IRequestHandler<PutRunSpecRequest> putRunSpecRequestHandler, IRequestHandler<PutRunRequest> putRunRequestHandler, IRequestHandler<GetLatestRunRequest, Run> getLatestRunRequestHandler)
        {
            _addRunRequest = addRunRequest;
            _apiContext = apiContext;
            _addSpecToRunRequestRequestHandler = addSpecToRunRequestRequestHandler;
            _addSpecBatchToRunRequestRequestHandler = addSpecBatchToRunRequestRequestHandler;
            _putRunSpecRequestHandler = putRunSpecRequestHandler;
            _putRunRequestHandler = putRunRequestHandler;
            _getLatestRunRequestHandler = getLatestRunRequestHandler;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]StartNewRun postRun)
        {
            var addRunRequest = new AddRunRequest(_apiContext.AppId, postRun);

            Run run = await _addRunRequest.HandleAsync(addRunRequest, Request.HttpContext.RequestAborted);

            return Created(String.Empty, run);
        }

        [HttpPost]
        [Route("{runId}/SpecBatches")]
        public async Task<IActionResult> PostRunSpecs([FromRoute]int runId, [FromBody]PostRunSpecBatch postedRunSpecBatch)
        {
            List<RunSpec> runSpecs = await _addSpecBatchToRunRequestRequestHandler.HandleAsync(
                new AddSpecBatchToRunRequest(_apiContext.AppId, runId, postedRunSpecBatch.SpecIds), Request.HttpContext.RequestAborted);
            return Created(string.Empty, runSpecs);
        }

        [HttpPost]
        [Route("{runId}/Specs")]
        public async Task<IActionResult> PostRunSpecs([FromRoute]int runId, [FromBody]PostRunSpec postedRunSpec)
        {
            RunSpec runSpec = await _addSpecToRunRequestRequestHandler.HandleAsync(new AddSpecToRunRequest(_apiContext.AppId, runId, postedRunSpec.SpecId), Request.HttpContext.RequestAborted);
            return Created(string.Empty, runSpec);
        }

        [HttpPut]
        [Route("{runId}/Specs/{specId}")]
        public async Task<IActionResult> PutRunSpecs([FromRoute] int runId, [FromRoute] int specId, [FromBody] PassFailRunSpec postedRunSpec)
        {
            var runSpec = new RunSpec
            {
                RunId = runId,
                SpecId = specId,
                Success = postedRunSpec.Passed
            };
            await _putRunSpecRequestHandler.HandleAsync(new PutRunSpecRequest(_apiContext.AppId, runSpec), Request.HttpContext.RequestAborted);
            return NoContent();
        }

        [HttpPut]
        [Route("{runId}")]
        public async Task<IActionResult> PutRun([FromRoute] int runId, [FromBody] Run run)
        {
            await _putRunRequestHandler.HandleAsync(new PutRunRequest(_apiContext.AppId, run), Request.HttpContext.RequestAborted);

            return NoContent();
        }

        [HttpGet]
        [Route("latest")]
        public async Task<IActionResult> GetRuns()
        {
            Run run = await _getLatestRunRequestHandler.HandleAsync(new GetLatestRunRequest(_apiContext.AppId), Request.HttpContext.RequestAborted);
            return Ok(run);
        }
    }
}
