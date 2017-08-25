using System;
using System.Collections.Generic;
using System.Text;
using StoryTeller.Portal.CQRS;
using StoryTeller.ResultAggregation.Models;

namespace StoryTeller.ResultAggregation.Commands
{
    public class AddSpecToRun : ApplicationScoped, ICommand
    {
        public RunSpec RunSpec { get; }
        public AddSpecToRun(int applicationId, RunSpec runSpec) : base(applicationId)
        {
            RunSpec = runSpec;
        }
    }
}
