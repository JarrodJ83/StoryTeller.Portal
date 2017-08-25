namespace StoryTeller.Portal.ResultsAggregator
{
    public interface IRunNameGenerator
    {
        string Generate(params object[] parameters);
    }
}
