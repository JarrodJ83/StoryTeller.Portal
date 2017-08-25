using System;
using System.Collections.Generic;
using System.Linq;
using StoryTeller.Portal.ResultsAggregator.Client;
using StoryTeller.ResultAggregation.Models;
using StoryTeller.ResultAggregation.Models.ClientModel;

namespace Tester
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var client = new PortalResultsAggregatorClient("http://localhost:17285/",
                    "ee5b80aa-c4e5-413d-8c60-5eb0acabca52");

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
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            Console.ReadLine();
        }
    }
}