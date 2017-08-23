using StoryTeller.Portal.CQRS;

namespace StoryTeller.ResultAggregation.Commands
{
    public class AddApplicationIfMissing : ICommand
    {
        public int ApplicationId { get; protected set; }
        public string ApplicationName { get; }

        public AddApplicationIfMissing(string applicationName)
        {
            ApplicationName = applicationName;
        }
    }
}
