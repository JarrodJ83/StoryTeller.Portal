using System;
using System.Collections.Generic;
using System.Threading;
using StoryTeller.ResultAggregation.Models;

namespace StoryTeller.Portal.ResultsAggregator
{
    public class RunContext : IDisposable
    {
        private static AsyncLocal<RunContext> _instance;
        public static RunContext Current => _instance?.Value;
        public Run Run { get; private set; }
        /// <summary>
        /// Key is the Portal's Spec ID and value is ST Spec ID
        /// </summary>
        public List<Spec> Specs { get; private set; }

        internal RunContext(Run run = null, List<Spec> specs = null)
        {
            Run = run;
            Specs = specs;
        }

        public static RunContext Create(Run run = null, List<Spec> specs = null)
        {
            _instance = new AsyncLocal<RunContext>();
            _instance.Value = new RunContext(run, specs);
            return _instance.Value;
        }

        public void Update(Run run = null, List<Spec> specs = null)
        {
            Run = run;
            Specs = specs;
        }

        public void Dispose()
        {
            _instance = null;
        }
    }
}