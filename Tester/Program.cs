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
using System.Threading;
using System.Threading.Tasks;
using StoryTeller.Portal.Db;
using Entities = Storyteller.Portal.Db.Model;

namespace Tester
{
    class Program
    {
        static void Main(string[] args)
        {
            var client = new PortalResultsAggregatorClient("http://localhost:51879/",
                "ee5b80aa-c4e5-413d-8c60-5eb0acabca52");
            try
            {
                SetupApp();
                TestClient(client);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            Console.ReadLine();
        }

        static void SetupApp()
        {
            using (var db = new ResultsDbContext(new Microsoft.EntityFrameworkCore.DbContextOptions<ResultsDbContext>()))
            {
                db.Database.EnsureDeleted();
                db.Database.EnsureCreated();

                var testApp = new Entities.App
                {
                    ApiKey = "test",
                    Name = "test"
                };
                
                var spec = new Entities.Spec()
                {
                    App = testApp,
                    Name = "Calculate something",
                    StorytellerId = Guid.NewGuid()
                };

                db.Specs.Add(spec);

                var run = new Entities.Run()
                {
                    App = testApp,
                    Name = $"Run @ {DateTime.Now.ToString("yyyy-mm-dd hh:MM:ss")}"
                };
                db.Runs.Add(run);

                var runSpec = new Entities.RunSpec()
                {
                    Run = run,
                    Spec = spec,
                    Passed = true
                };

                var runResults = new Entities.RunResult()
                { 
                    HtmlResults = "<html><div>resuls here</div></html>",
                    Run = run
                };

                db.Apps.Add(testApp);

                db.SaveChanges();
            }
        }

        static void TestRunLogger(IPortalResultsAggregatorClient client)
        {
            var runLogger = new RunLogger(client, new RunLoggerSettings("D:\\github\\StoryTeller.Portal\\FakeSystemAndSpecs\\FakeSystemAndSpecs\\stresults.html"));
            
            List<Spec> specs = client.GetAllSpecsAsync().Result;

            var stSpecs = specs.Select(s => new Specification
                {
                    id = s.StoryTellerId.ToString(),
                    name = s.Name
                })
                .ToList();

            runLogger.Receive(new BatchRunRequest(stSpecs));

            var specRunner = new Task(() =>
            {
                var extension = new SpecResultLoggingExtension(client);

                foreach (var s in stSpecs)
                {
                    Thread.Sleep(TimeSpan.FromSeconds(2));
                    extension.AfterEach(new SpecContext(s, null, null, null, null));
                }
            });

            specRunner.Start();
            specRunner.Wait();

            client.CompleteRunAsync(new RunResult
            {
                HtmlResults = "testing",
                Passed = true,
                RunId = RunContext.Current.Run.Id
            });
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
            
            client.CompleteRunAsync(new RunResult
            {
                RunId = run.Id,
                Passed = true,
                HtmlResults = "test"
            }).Wait();
        }
    }
}