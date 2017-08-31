using System;
using System.Collections.Generic;
using System.Linq;
using StoryTeller;
using StoryTeller.Engine;
using StoryTeller.Model;
using StoryTeller.Portal.ResultsAggregator;
using StoryTeller.Portal.ResultsAggregator.Client;
using StoryTeller.ResultAggregation.Models;
using StoryTeller.ResultAggregation.Models.ClientModel;
using Fixture = Ploeh.AutoFixture.Fixture;
using System.Threading;

namespace Tester
{
    class Program
    {
        static void Main(string[] args)
        {
            var client = new PortalResultsAggregatorClient("http://localhost:1881/",
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
            var runLogger = new RunLogger(client, new RunLoggerSettings("c:\\temp\\stresults.html"));

            var fixture = new Fixture();

            List<Spec> specs = client.GetAllSpecsAsync().Result;

            var stSpecs = specs.Select(s => new Specification
                {
                    id = s.StoryTellerId.ToString(),
                    name = s.Name
                })
                .ToList();

            runLogger.Receive(new BatchRunRequest(stSpecs));

            var extension = new SpecResultLoggingExtension(client);

            foreach (var s in stSpecs) {
                Thread.Sleep(TimeSpan.FromSeconds(2));
                extension.AfterEach(new SpecContext(s, null, null, null, null));
            }

            //runLogger.Receive(new BatchRunResponse
            //{
                
            //});
        }
        
        static void TestClient(IPortalResultsAggregatorClient client)
        {
            List<Spec> allSpecs = client.GetAllSpecsAsync().Result;

            var newSpec = client.AddSpecAsync(new PostSpec
                {
                    Name = "new spec",
                    StoryTellerId = Guid.NewGuid()
                })
                .Result;

            allSpecs.Add(newSpec);

            var run = client.StartNewRunAsync(new StartNewRun
                {
                    RunDateTime = DateTime.Now,
                    RunName = "My Run",
                    SpecIds = allSpecs.Select(s => s.Id).ToList()
                }).Result;

            var rand = new Random();
            foreach (Spec spec in allSpecs)
            {
                Thread.Sleep(TimeSpan.FromSeconds(2));
                client.PassFailRunSpecAsync(new PassFailRunSpec(run.Id, spec.Id, rand.Next(1000) % 2 == 0)).Wait();
            }

            run.HtmlResults = "FINISHED!";

            client.UpdateRunAsync(run).Wait();
        }
    }
}