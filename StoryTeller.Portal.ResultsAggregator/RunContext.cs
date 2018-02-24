using System.Collections.Generic;
using StoryTeller.ResultAggregation.Models;

namespace StoryTeller.Portal.ResultsAggregator
{
    public class RunContext
    {
        private static RunContext _instance;
        public static RunContext Current => _instance;
        public Run Run { get; }
        /// <summary>
        /// Key is the Portal's Spec ID and value is ST Spec ID
        /// </summary>
        public List<Spec> Specs { get; }

        internal RunContext(Run run, List<Spec> specs)
        {
            Run = run;
            Specs = specs;
        }

        public static void Create(Run run, List<Spec> specs)
        {
            _instance = new RunContext(run, specs);
        }

        public static void Destroy()
        {
            _instance = null;
        }
    }
}