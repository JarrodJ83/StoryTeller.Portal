using System.Collections.Generic;
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
