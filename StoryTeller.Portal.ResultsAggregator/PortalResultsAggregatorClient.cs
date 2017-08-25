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

namespace StoryTeller.Portal.ResultsAggregator
{
    public class PortalResultsAggregatorClient : IPortalResultsAggregatorClient
    {
        public string ApiKey { get; }
        public string Url { get; }

        public PortalResultsAggregatorClient(string url, string apiKey)
        {
            ApiKey = apiKey;
            Url = url;
        }

        #region IPortalResultsAggregatorClient
        public async Task<List<Spec>> GetSpecsAsync() => await GetAsync<List<Spec>>("Specs");

        public async Task<Spec> AddSpecAsync(PostSpec spec) => await PostAsync<Spec>("Specs", spec);

        public async Task<Run> AddRunAsync(PostRun run) => await PostAsync<Run>("Runs", run);

        public async Task AddSpecsToRunAsync(int runId, PostRunSpecBatch runSpecBatch) => await PostAsync($"Runs/{runId}/SpecBatches", runSpecBatch);

        public async Task UpdateRunSpecAsync(int runId, int specId, PutRunSpec runSpec) => await PutAsync($"Runs/{runId}/Specs/{specId}", runSpec);

        public async Task UpdateRunAsync(Run run) => await PutAsync($"Runs/{run.Id}", run);
        
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

                Task<HttpResponseMessage> invokeTask = action.Invoke(client);
                invokeTask.Wait();
                return invokeTask.Result;
            }
        }

        async Task<TResult> GetAsync<TResult>(string path) where TResult : class
        {
            HttpResponseMessage response = await SendAsync(client => client.GetAsync(path));
            return await ResultOrThrowAsync<TResult>(HttpStatusCode.OK, response);
        }

        async Task<TResult> PostAsync<TResult>(string path, object payload) where TResult : class
        {
            HttpResponseMessage response = await SendAsync(client => client.PostAsync(path, CreatePostContent(payload)));
            return await ResultOrThrowAsync<TResult>(HttpStatusCode.Created, response);
        }

        async Task PostAsync(string path, object payload)
        {
            HttpResponseMessage response = await SendAsync(client => client.PostAsync(path, CreatePostContent(payload)));
            if (response.StatusCode != HttpStatusCode.Created)
            {
                var content = await response.Content?.ReadAsStringAsync();
                throw new Exception(content);
            }
        }

        async Task PutAsync<TObject>(string path, TObject payload)
        {
            HttpResponseMessage response = await SendAsync(client => client.PostAsync(path, CreatePostContent(payload)));
            if (response.StatusCode != HttpStatusCode.NoContent)
            {
                var content = await response.Content?.ReadAsStringAsync();
                throw new Exception(content);
            }
        }

        #endregion
    }
}
