using System;
using System.Collections.Generic;
using System.Linq;
using Ploeh.AutoFixture;
using StoryTeller;
using StoryTeller.Engine;
using StoryTeller.Model;
using StoryTeller.Portal.ResultsAggregator;
using StoryTeller.Portal.ResultsAggregator.Client;
using StoryTeller.ResultAggregation.Models;
using StoryTeller.ResultAggregation.Models.ClientModel;
using Fixture = Ploeh.AutoFixture.Fixture;

namespace Tester
{
    class Program
    {
        static void Main(string[] args)
        {
            var client = new PortalResultsAggregatorClient("http://localhost:17286/",
                "ee5b80aa-c4e5-413d-8c60-5eb0acabca52");
            try
            {
                TestRunLogger(client);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            Console.ReadLine();
        }

        static void TestRunLogger(IPortalResultsAggregatorClient client)
        {
            var runLogger = new RunLogger(client, new RunLoggerSettings());

            var fixture = new Fixture();

            List<Spec> specs = client.GetSpecsAsync().Result;

            var stSpecs = specs.Select(s => new Specification
                {
                    id = s.StoryTellerId.ToString(),
                    name = s.Name
                })
                .ToList();

            runLogger.Receive(new BatchRunRequest(stSpecs));

            var extension = new SpecResultLoggingExtension(client);

            stSpecs.ForEach(s => extension.AfterEach(new SpecContext(s, null, null, null, null)));

            runLogger.Receive(new BatchRunResponse
            {
                
            });
        }
        
        static void TestClient(IPortalResultsAggregatorClient client)
        {
            List<Spec> allSpecs = client.GetSpecsAsync().Result;

            var newSpec = client.AddSpecAsync(new PostSpec
                {
                    Name = "new spec",
                    StoryTellerId = Guid.NewGuid()
                })
                .Result;

            allSpecs.Add(newSpec);

            var run = client.AddRunAsync(new PostRun
                {
                    RunDateTime = DateTime.Now,
                    RunName = "My Run"
                })
                .Result;

            client.AddSpecsToRunAsync(run.Id, new PostRunSpecBatch
                {
                    SpecIds = allSpecs.Select(s => s.Id).ToList()
                })
                .Wait();

            foreach (Spec spec in allSpecs)
            {
                client.UpdateRunSpecAsync(run.Id, spec.Id, new PutRunSpec
                    {
                        Passed = new Random().Next(1000) % 2 == 0
                    })
                    .Wait();
            }

            run.HtmlResults = "FINISHED!";

            client.UpdateRunAsync(run).Wait();
        }
    }
}