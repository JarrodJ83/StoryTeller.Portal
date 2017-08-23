using Microsoft.AspNetCore.Mvc;
using StoryTeller.Portal.CQRS;
using StoryTeller.ResultAggregation.Requests;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace StoryTeller.ResultAggregator.Controllers
{
    [Route("api/[controller]")]
    public class RunsController : Controller
    {
        private readonly IRequestHandler<AddRunRequest> _addRunRequest;

        public RunsController(IRequestHandler<AddRunRequest> addRunRequest)
        {
            _addRunRequest = addRunRequest;
        }

        [HttpPost]
        public void Post([FromBody]AddRunRequest addRunRequest)
        {
            _addRunRequest.Handle(addRunRequest);
        }
    }
}
