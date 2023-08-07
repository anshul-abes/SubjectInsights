using System;
using System.Collections.Concurrent;
using System.Net.Http;

namespace SubjectInsights.Common
{
    public interface IServiceApiClient
    {
        HttpClient GetHttpClient(string apiName);

        HttpClient GetHttpClient(string apiName, int maxConnections);

        HttpClient GetHttpClientForTest(string baseAddress);

        HttpClient GetHttpClientBaseAddress(string baseAddress);
    }

    public class ServiceApiClient : IServiceApiClient
    {
        private const int HttpClientMaxConnections = 100;
        private readonly ConcurrentDictionary<string, HttpClient> _httpClients = new ConcurrentDictionary<string, HttpClient>();

        public ServiceApiClient()
        {
        }

        public HttpClient GetHttpClient(string apiName)
        {
            return GetHttpClient(apiName, HttpClientMaxConnections);
        }


        public HttpClient GetHttpClient(string apiName, int maxConnections)
        {
            var client = _httpClients.GetOrAdd(apiName, delegate
            {
                if (_httpClients.ContainsKey(apiName))
                    return _httpClients[apiName];

                var serviceEndpoint = ServiceHelper.GetServiceAddress(apiName);
                var httpClient = new HttpClient(new HttpClientHandler
                {
                    MaxConnectionsPerServer = maxConnections
                })
                {
                    BaseAddress = serviceEndpoint
                };
                var sp = System.Net.ServicePointManager.FindServicePoint(httpClient.BaseAddress);
                sp.ConnectionLeaseTimeout = 5 * 60 * 1000;
                httpClient.DefaultRequestHeaders.Add("X-REQUEST-ID", Guid.NewGuid().ToString());
                return httpClient;
            });

            return client;
        }


        public HttpClient GetHttpClientForTest(string baseAddress)
        {
            if (string.IsNullOrEmpty(baseAddress))
            {
                baseAddress = "http://localhost:51170";
            }

            var client = _httpClients.GetOrAdd(baseAddress, delegate
            {
                if (_httpClients.ContainsKey(baseAddress))
                    return _httpClients[baseAddress];

                var httpClient = new HttpClient { BaseAddress = new Uri(baseAddress) };
                httpClient.DefaultRequestHeaders.Add("X-REQUEST-ID", Guid.NewGuid().ToString());
                return httpClient;
            });

            return client;
        }

        public HttpClient GetHttpClientBaseAddress(string baseAddress)
        {
            var address = string.IsNullOrEmpty(baseAddress) ? "http://localhost:51170/" : baseAddress;

            var client = _httpClients.GetOrAdd(address, delegate
            {
                if (_httpClients.ContainsKey(address))
                    return _httpClients[address];

                var httpClient = new HttpClient(new HttpClientHandler()) { BaseAddress = new Uri(address) };
                var sp = System.Net.ServicePointManager.FindServicePoint(httpClient.BaseAddress);
                sp.ConnectionLeaseTimeout = 5 * 60 * 1000;
                return httpClient;
            });
            return client;
        }
    }
}
