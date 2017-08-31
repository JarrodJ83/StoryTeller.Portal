using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using StoryTeller.ResultAggregation.Models;
using StoryTeller.ResultAggregation.Models.ClientModel;

namespace StoryTeller.Portal.ResultsAggregator.Client
{
    public class PortalResultsAggregatorClient : IPortalResultsAggregatorClient
    {
        public string ApiKey { get; }
        public string Url { get; }

        public PortalResultsAggregatorClient(string url, string apiKey)
        {
            ApiKey = apiKey;
            Url = url;

            if (Url.EndsWith("/"))
                Url = Url.Substring(0, Url.Length - 1);
        }

        #region IPortalResultsAggregatorClient
        public async Task<List<Spec>> GetAllSpecsAsync() => await GetAsync<List<Spec>>("Specs");

        public async Task<Spec> AddSpecAsync(PostSpec spec) => await PostAsync<Spec>("Specs", spec);

        public async Task<Run> StartNewRunAsync(StartNewRun run) => await PostAsync<Run>("Runs", run);
        
        public async Task PassFailRunSpecAsync(PassFailRunSpec runSpec) => await PutAsync($"Runs/{runSpec.RunId}/Specs/{runSpec.SpecId}", runSpec);

        public async Task CompleteRunAsync(RunResult runResult) => await PostAsync($"Runs/{runResult.RunId}/Results", runResult);

        public async Task<Run> GetLatestRun() => await GetAsync<Run>("Runs/Latest");

        #endregion

        #region Private

        async Task<TResult> ResultOrThrowAsync<TResult>(HttpStatusCode expectedStatusCode, HttpResponseMessage response) where TResult : class
        {
            string content = string.Empty;

            if (response.StatusCode != expectedStatusCode)
            {
                content = await response.Content?.ReadAsStringAsync();
                throw new Exception(content);
            }

            if (response.Content == null)
                return null;

            content = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<TResult>(content);
        }

        StringContent CreatePostContent(object data)
        {
            string postData = JsonConvert.SerializeObject(data);
            return new StringContent(postData, Encoding.UTF8, "application/json");
        }

        async Task<HttpResponseMessage> SendAsync(Func<HttpClient, Task<HttpResponseMessage>> action)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Url);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                client.DefaultRequestHeaders.Add("x-api-key", ApiKey);

                return await action.Invoke(client);
            }
        }

        async Task<TResult> GetAsync<TResult>(string resource) where TResult : class
        {
            HttpResponseMessage response = await SendAsync(client => client.GetAsync($"api/{resource}"));
            return await ResultOrThrowAsync<TResult>(HttpStatusCode.OK, response);
        }

        async Task<TResult> PostAsync<TResult>(string resource, object payload) where TResult : class
        {
            HttpResponseMessage response = await SendAsync(client => client.PostAsync($"api/{resource}", CreatePostContent(payload)));
            return await ResultOrThrowAsync<TResult>(HttpStatusCode.Created, response);
        }

        async Task PostAsync(string resource, object payload)
        {
            HttpResponseMessage response = await SendAsync(client => client.PostAsync($"api/{resource}", CreatePostContent(payload)));
            if (response.StatusCode != HttpStatusCode.Created)
            {
                var content = await response.Content?.ReadAsStringAsync();
                throw new Exception(content);
            }
        }

        async Task PutAsync(string resource, object payload)
        {
            HttpResponseMessage response = await SendAsync(client => client.PutAsync($"api/{resource}", CreatePostContent(payload)));
            if (response.StatusCode != HttpStatusCode.NoContent)
            {
                var content = await response.Content?.ReadAsStringAsync();
                throw new Exception(content);
            }
        }

        #endregion
    }
}
