using System;
using System.Collections.Generic;
using StoryTeller.Portal.CQRS;

namespace StoryTeller.ResultAggregation.Commands
{
    public class AddOrUpdateSpecs : ICommand
    {
        public Dictionary<Guid, string> Specifications { get; }

        public AddOrUpdateSpecs(Dictionary<Guid, string> specifications)
        {
            Specifications = specifications;
        }
    }
}
