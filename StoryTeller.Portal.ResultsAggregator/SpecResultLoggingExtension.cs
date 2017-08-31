﻿using System;
using System.Linq;
using System.Threading.Tasks;
using StoryTeller.Portal.ResultsAggregator.Client;
using StoryTeller.ResultAggregation.Models;
using StoryTeller.ResultAggregation.Models.ClientModel;

namespace StoryTeller.Portal.ResultsAggregator
{
    public class SpecResultLoggingExtension : IExtension
    {
        private IPortalResultsAggregatorClient _client;

        public void Dispose()
        {
            _client = null;
        }

        public SpecResultLoggingExtension(IPortalResultsAggregatorClient Client)
        {
            _client = Client;
        }

        #region IExtension

        public Task Start()
        {
            return Task.CompletedTask;
        }

        public void BeforeEach(ISpecContext context)
        {
        }

        public void AfterEach(ISpecContext context)
        {
            SpecContext ctx = (SpecContext) context;

            if (RunContext.Current == null)
            {
                Console.WriteLine("No RunContext present. Will not attempt to send spec results to StoryTeller Portal");
                return;
            }

            var spec = new Spec();
            try
            {
                spec = RunContext.Current.Specs.Single(s => s.StoryTellerId.Equals(Guid.Parse(ctx.Specification.id)));

                bool passed = ctx.Counts.Exceptions == 0 && ctx.Counts.Wrongs == 0;
                _client.PassFailRunSpecAsync(new PassFailRunSpec(RunContext.Current.Run.Id, spec.Id, passed));

                Console.WriteLine($"Spec {spec.Id} [{spec.StoryTellerId}] updated in StoryTeller Portal");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating {spec.Id} [{spec.StoryTellerId}] in StoryTeller Portal", ex);
            }
        }

        #endregion
    }
}
