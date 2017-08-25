namespace StoryTeller.ResultAggregation
{
    public abstract class ApplicationScoped
    {
        public int AppId { get; }

        public ApplicationScoped(int appId)
        {
            AppId = appId;
        }
    }
}
