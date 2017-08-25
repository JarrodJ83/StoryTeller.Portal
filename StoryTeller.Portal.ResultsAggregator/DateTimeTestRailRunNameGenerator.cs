using System;

namespace StoryTeller.Portal.ResultsAggregator
{
    public class DateTimeRunNameGenerator : IRunNameGenerator
    {
        public string Generate(params object[] parameters)
        {
            return DateTime.UtcNow.ToString("G");
        }
    }
}
