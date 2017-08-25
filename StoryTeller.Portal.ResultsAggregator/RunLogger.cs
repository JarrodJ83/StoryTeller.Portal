﻿using System;
using System.Collections.Generic;
using System.Linq;
using StoryTeller.Engine;
using StoryTeller.Model;
using StoryTeller.Portal.ResultsAggregator.Client;
using StoryTeller.Remotes.Messaging;
using StoryTeller.ResultAggregation.Models;
using StoryTeller.ResultAggregation.Models.ClientModel;

namespace StoryTeller.Portal.ResultsAggregator
{
    public class RunLogger : IListener<BatchRunRequest>
    {
        private readonly IPortalResultsAggregatorClient _client;
        private readonly IRunLoggerSettings _RunLoggerSettings;

        public RunLogger(IPortalResultsAggregatorClient client, IRunLoggerSettings RunLoggerSettings)
        {
            _client = client;
            _RunLoggerSettings = RunLoggerSettings;
        }

        #region IListener<BatchRunRequest>

        public void Receive(BatchRunRequest message)
        {
            List<Spec> allSpecs = _client.GetSpecsAsync().Result;
            Guid[] storyTellerIds = allSpecs.Select(remoteSpec => remoteSpec.StoryTellerId).ToArray();
            List<Specification> newSpecs = message.Specifications.Where(s => !storyTellerIds.Contains(Guid.Parse(s.id))).ToList();
            
            newSpecs.ForEach(ns =>
            {
                Spec newSpec = _client.AddSpecAsync(new PostSpec
                    {
                        Name = "new spec",
                        StoryTellerId = Guid.NewGuid()
                    }).Result;

                Console.WriteLine($"Spec {newSpec.Name} added to StoryTeller Portal");

                allSpecs.Add(newSpec);
            });

            Run run = _client.AddRunAsync(new PostRun
                {
                    RunDateTime = DateTime.Now,
                    RunName = _RunLoggerSettings.RunNameGenerator.Generate()
                }).Result;

            Console.WriteLine($"Run {run.Name} added to StoryTeller Portal");

            _client.AddSpecsToRunAsync(run.Id, new PostRunSpecBatch
                {
                    SpecIds = allSpecs.Select(s => s.Id).ToList()
                }).Wait();

            Console.WriteLine($"Specs associated to {run.Name} in StoryTeller Portal");
        }

        #endregion
    }
}
