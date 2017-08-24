using System;
using System.Collections.Generic;
using System.Text;
using StoryTeller.Portal.CQRS;
using StoryTeller.ResultAggregation.Models;
using StoryTeller.ResultAggregation.Queries;

namespace StoryTeller.ResultAggregation.Commands
{
    public class AddSpec : ICommand<int>
    {
        public Spec Spec { get; }

        public AddSpec(Spec spec)
        {
            Spec = spec;
        }

        public int Key => Spec.Id;
    }
}
