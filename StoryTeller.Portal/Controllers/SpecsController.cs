using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StoryTeller.Portal.CQRS;
using StoryTeller.ResultAggregation.Models;
using StoryTeller.ResultAggregation.Requests;

namespace StoryTeller.Portal.Controllers
{
    public class SpecsController : Controller
    {
        private readonly IRequestHandler<GetAllSpecs, List<Spec>> _getAllSpecsRequestHandler;
        private readonly IApiContext _apiContext;

        public SpecsController(IRequestHandler<ResultAggregation.Requests.GetAllSpecs, List<Spec>> getAllSpecsRequestHandler, IApiContext apiContext)
        {
            _getAllSpecsRequestHandler = getAllSpecsRequestHandler;
            _apiContext = apiContext;
        }

        public async Task<List<Spec>> Get()
        {
            return await _getAllSpecsRequestHandler.HandleAsync(new GetAllSpecs(_apiContext.ApplicationId), Request.HttpContext.RequestAborted);
        }
    }
}
