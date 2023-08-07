using SubjectInsights.Logger;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace SubjectInsights.Common.Extensions
{
    public static class ServiceExtensions
    {
        public static async Task<O> SendSimpleAsync<O>(this HttpClient client, string route, Dictionary<string, string> customHeaders = null)
        {
            using HttpRequestMessage request = new(HttpMethod.Get, route);
            if (customHeaders != null)
            {
                foreach (var h in customHeaders)
                {
                    request.Headers.Add(h.Key, h.Value);
                }
            }

            using var response = await client.SendAsync(request);
            if (!response.IsSuccessStatusCode) return default;

            return await response.Content.ReadAsAsync<O>();
        }
        public static async Task<HttpResponseMessage> GetAuthorizedSimpleAsync(this HttpClient client, string route, Dictionary<string, string> customHeaders = null, AuthenticationHeaderValue authorization = null)
        {
            using HttpRequestMessage request = new(HttpMethod.Get, route);
            if (authorization != null) request.Headers.Authorization = authorization;
            if (customHeaders != null)
            {
                foreach (var h in customHeaders)
                {
                    request.Headers.Add(h.Key, h.Value);
                }
            }

            return await client.SendAsync(request);
        }

        public static async Task<O> PostSimpleWithHeadersAsync<T, O>(this HttpClient client, T input, string route, List<KeyValuePair<string, string>> customHeaders = null)
        {
            var request = new HttpRequestMessage()
            {
                RequestUri = new Uri(client.BaseAddress.AbsoluteUri + route),
                Method = HttpMethod.Post,
                Content = new StringContent(JsonConvert.SerializeObject(input), Encoding.UTF8, "application/json")
            };
            if (customHeaders != null && customHeaders.Any())
            {
                foreach (var h in customHeaders)
                {
                    request.Headers.Add(h.Key, h.Value);
                }
            }

            using var res = await client.SendAsync(request);
            return await res.Content.ReadAsAsync<O>();
        }

        public static async Task<O> PostSimpleAsync<T, O>(this HttpClient client, T input, string route)
        {
            string inputString = JsonConvert.SerializeObject(input);
            try
            {
                var Content = new StringContent(inputString, Encoding.UTF8, "application/json");
                using var response = await client.PostAsync($"{client.BaseAddress}{route}", Content);
                if (!response.IsSuccessStatusCode)
                {
                    var httpStatusCode = response.StatusCode;
                    var msg = $"call {route} fail. http status code = {httpStatusCode}";
                    CommonLogger.LogError(msg);
                    return default;
                }

                return await response.Content.ReadAsAsync<O>();
            }
            catch (Exception ex)
            {
                CommonLogger.LogError($"call {route} exception. | inputString={inputString} | {ex.Message} | {ex.StackTrace}");
            }

            return default;
        }

        public static async Task<O> PostSimpleAsyncD<T, O>(this HttpClient client, T input, string route, List<KeyValuePair<string, string>> customHeaders = null)
        {
            var request = new HttpRequestMessage()
            {
                RequestUri = new Uri(client.BaseAddress.AbsoluteUri + route),
                Method = HttpMethod.Post,
                Content = new StringContent(JsonConvert.SerializeObject(input), Encoding.UTF8, "application/json")
            };
            if (customHeaders != null && customHeaders.Any())
            {
                foreach (var h in customHeaders)
                {
                    request.Headers.Add(h.Key, h.Value);
                }
            }

            using (var res = await client.SendAsync(request))
            {
                return await res.Content.ReadAsAsync<O>();
            }
        }
    }
}
