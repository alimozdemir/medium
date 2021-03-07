using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using StreamSerializeAPI.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace StreamSerializeAPI.Controllers
{
    public class ConcatenateBaseController : ControllerBase
    {
        private readonly HttpClient httpClient;
        public ConcatenateBaseController(IHttpClientFactory httpClientFactory)
        {
            httpClient = httpClientFactory.CreateClient("placeholder");
        }

        public async Task<JRaw> GetAsRaw(string route)
        {
            var response = await httpClient.GetAsync(route);
            var stringData = await response.Content.ReadAsStringAsync();

            return new JRaw(stringData);
        }

        public async Task<JRaw> GetAsStream(string route)
        {
            var hrm = new HttpRequestMessage(HttpMethod.Get, route);
            var response = await httpClient.SendAsync(hrm, HttpCompletionOption.ResponseHeadersRead);

            var stream = await response.Content.ReadAsStreamAsync();

            return new JRaw(stream);
        }

        public Dictionary<int, string> Sources { get; set; } = new Dictionary<int, string>()
        {
            { 0, "albums" },
            { 1, "todos" },
            { 2, "comments" },
            { 3, "photos" },
        };

        public async Task<ConcatenateModel<JRaw>> FetchData(Func<string, Task<JRaw>> funcPointer)
        {
            List<Task> tasks = new List<Task>();

            ConcatenateModel<JRaw> result = new ConcatenateModel<JRaw>() { Data = new ConcurrentDictionary<int, JRaw>() };

            async Task Fetch(KeyValuePair<int, string> source)
            {
                var fetchResult = await funcPointer(source.Value);
                result.Data.AddOrUpdate(source.Key, fetchResult, (key, oldValue) => fetchResult);
            }

            foreach (var item in Sources)
            {
                tasks.Add(Fetch(item));
            }

            await Task.WhenAll(tasks);

            return result;
        }
    }
}
