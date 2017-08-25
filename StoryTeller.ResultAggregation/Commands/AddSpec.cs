using StoryTeller.Portal.CQRS;
using StoryTeller.ResultAggregation.Models;

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
