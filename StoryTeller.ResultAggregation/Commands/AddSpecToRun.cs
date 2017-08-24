using System;
using System.Collections.Generic;
using System.Text;
using StoryTeller.Portal.CQRS;

namespace StoryTeller.ResultAggregation.Commands
{
    public class AddSpecToRun : ApplicationScoped, ICommand
    {
        public int RunId { get; }
        public int SpecId { get; }
        public AddSpecToRun(int applicationId, int runId, int specId) : base(applicationId)
        {
            RunId = runId;
            SpecId = specId;
        }
    }
}
