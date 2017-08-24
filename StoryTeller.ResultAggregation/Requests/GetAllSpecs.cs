using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using StoryTeller.Portal.CQRS;
using StoryTeller.ResultAggregation.Models;

namespace StoryTeller.ResultAggregation.Requests
{
    public class GetAllSpecs : ApplicationScoped, IRequest<List<Spec>>
    {
        public GetAllSpecs(int applicationId) : base(applicationId)
        {
        }
    }
}
