using System;
using System.Collections.Generic;
using System.Text;
using StoryTeller.Portal.CQRS;
using StoryTeller.ResultAggregation.Models;

namespace StoryTeller.ResultAggregation.Queries
{
    public class SpecsByApplication : ApplicationScoped, IQuery<List<Spec>>
    {
        public SpecsByApplication(int applicationId) : base(applicationId)
        {
        }
    }
}
