using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StoryTeller.Portal.CQRS;
using StoryTeller.ResultAggregation.ClientModel;
using StoryTeller.ResultAggregation.Models;
using StoryTeller.ResultAggregation.Requests;

namespace StoryTeller.Portal.Controllers
{
    [Route("api/[controller]")]
    public class SpecsController : Controller
    {
        private readonly IRequestHandler<GetAllSpecs, List<Spec>> _getAllSpecsRequestHandler;
        private readonly IRequestHandler<AddSpecRequest, Spec> _addSpecRequestHandler;
        private readonly IApiContext _apiContext;

        public SpecsController(IRequestHandler<ResultAggregation.Requests.GetAllSpecs, List<Spec>> getAllSpecsRequestHandler, IApiContext apiContext, IRequestHandler<AddSpecRequest, Spec> addSpecRequestHandler)
        {
            _getAllSpecsRequestHandler = getAllSpecsRequestHandler;
            _apiContext = apiContext;
            _addSpecRequestHandler = addSpecRequestHandler;
        }

        [HttpGet]
        public async Task<List<Spec>> Get()
        {
            return await _getAllSpecsRequestHandler.HandleAsync(new GetAllSpecs(_apiContext.ApplicationId), Request.HttpContext.RequestAborted);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]PostSpec postSpec)
        {
            var addSpecRequest = new AddSpecRequest(_apiContext.ApplicationId, postSpec);
            Spec spec = await _addSpecRequestHandler.HandleAsync(addSpecRequest, Request.HttpContext.RequestAborted);
            return Created(string.Empty, spec);
        }
    }
}
