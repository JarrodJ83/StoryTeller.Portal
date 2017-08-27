namespace StoryTeller.Portal.ResultsAggregator
{
    public class RunLoggerSettings : IRunLoggerSettings
    {
        private IRunNameGenerator _runNameGenerator = new DateTimeRunNameGenerator();
        public IRunNameGenerator RunNameGenerator
        {
            get => _runNameGenerator;
            set => _runNameGenerator = value ?? new DateTimeRunNameGenerator();
        }

        public string HtmlResultsFileName { get; }

        #region IRunLoggerSettings

        public RunLoggerSettings(IRunNameGenerator runNameGenerator = null)
        {
            RunNameGenerator = runNameGenerator;
            HtmlResultsFileName = @"C:\temp\stresults.html";
        }

        #endregion
    }
}
