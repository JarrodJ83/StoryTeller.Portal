using Microsoft.AspNetCore.Mvc;
using StoryTeller.ResultAggregation.Models;
using System.Collections.Generic;

namespace StoryTeller.Portal.Controllers
{
    [Route("[controller]")]
    public class AppsController : Controller
    {
        [HttpGet]
        public List<App> GetAll()
        {            
            return new List<App>
            {
                new App{ Id = 1, Name = "App One" },
                new App{ Id = 2, Name = "App Two" }
            };
        }
    }
}
