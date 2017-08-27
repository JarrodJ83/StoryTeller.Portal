namespace StoryTeller.Portal.ResultsAggregator
{
    public interface IRunLoggerSettings
    {
        string HtmlResultsFileName { get;  }
        IRunNameGenerator RunNameGenerator { get; }
    }
}
