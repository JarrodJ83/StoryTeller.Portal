namespace StoryTeller.Portal
{
    public class ApiContext : IApiContext
    {
        public int ApplicationId { get; }

        private ApiContext(int applicationId)
        {
            ApplicationId = applicationId;
        }
    }
}
