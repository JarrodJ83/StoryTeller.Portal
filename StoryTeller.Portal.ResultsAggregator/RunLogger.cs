using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using StoryTeller.Engine;
using StoryTeller.Model;
using StoryTeller.Portal.ResultsAggregator.Client;
using StoryTeller.Remotes.Messaging;
using StoryTeller.ResultAggregation.Models;
using StoryTeller.ResultAggregation.Models.ClientModel;
using StoryTeller.Results;

namespace StoryTeller.Portal.ResultsAggregator
{
    public class RunLogger : 
        IListener<BatchRunRequest> 
        //IListener<BatchRunResponse>
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
            try
            {
                List<Spec> allSpecs = _client.GetAllSpecsAsync().Result;
                Guid[] storyTellerIds = allSpecs.Select(remoteSpec => remoteSpec.StoryTellerId).ToArray();
                List<Specification> newSpecs = message.Specifications
                    .Where(s => !storyTellerIds.Contains(Guid.Parse(s.id)))
                    .ToList();

                newSpecs.ForEach(ns =>
                {
                    Spec newSpec = _client.AddSpecAsync(new PostSpec
                        {
                            Name = ns.name,
                            StoryTellerId = Guid.Parse(ns.id)
                        }).Result;

                    Console.WriteLine($"Spec {newSpec.Name} added to StoryTeller Portal");

                    allSpecs.Add(newSpec);
                });

                List<Guid> stSpecIds = message.Specifications.Select(s => Guid.Parse(s.id)).ToList();
                List<Spec> runSpecs = allSpecs.Where(s => stSpecIds.Contains(s.StoryTellerId))
                                                .ToList();

                Run run = _client.StartNewRunAsync(new StartNewRun
                    {
                        RunDateTime = DateTime.Now,
                        RunName = _RunLoggerSettings.RunNameGenerator.Generate(),
                        SpecIds = runSpecs.Select(s => s.Id).ToList()
            }).Result;

                Console.WriteLine($"Run {run.Name} added to StoryTeller Portal");

                if (RunContext.Current == null)
                {
                    Console.WriteLine($"Creating Run {run.Name} context");
                    RunContext.Create(run, runSpecs); 
                }
                else
                {
                    Console.WriteLine($"Updating Run {run.Name} context");
                    RunContext.Current.Update(run, runSpecs);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error setting up Run in StoryTeller Portal\n\n{ex}");
            }
        }

        #endregion

        //#region IListener<BatchRunResponse>

        //public void Receive(BatchRunResponse results)
        //{
        //    var run = RunContext.Current.Run;

        //    var htmlResults = File.ReadAllText(_RunLoggerSettings.HtmlResultsFileName);

        //    run.HtmlResults = htmlResults;

        //    _client.UpdateRunAsync(run).Wait();

        //    Console.WriteLine($"Run {run.Id} updated in StoryTeller Portal with final results");
        //}

        //#endregion
    }
}
