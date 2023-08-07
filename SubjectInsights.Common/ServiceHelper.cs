using System;

namespace SubjectInsights.Common
{
    public class B2BConstants
    {
        public const string XToken = "x-b2b-token";
        public const string MarketID = "x-market-id";
        public const string RequestID = "x-request-id";
        public const string Username = "x-username";
    }

    public static class ServiceHelper
    {
        ///// <summary>
        ///// Method to post request to Api
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="httpClient"></param>
        ///// <param name="dataContent"></param>
        ///// <param name="url"></param>
        ///// <param name="authToken"></param>
        ///// <returns></returns>
        //public static async Task<HttpResponseMessage> PostRequest<T>(HttpClient httpClient, T dataContent, string url, Dictionary<string, string> headers = null)
        //{
        //    using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, url))
        //    {
        //        if (headers != null)
        //        {
        //            foreach (KeyValuePair<string, string> entry in headers)
        //            {
        //                request.Headers.Add(entry.Key, entry.Value);
        //            }
        //        }

        //        using (var content = new StringContent(JsonConvert.SerializeObject(dataContent), Encoding.UTF8, "application/json"))
        //        {
        //            request.Content = content;
        //            return await httpClient.SendAsync(request);
        //        }
        //    }
        //}

        //public static async Task<O> PostSimpleAsyncD<T, O>(this HttpClient client, T input, string route, List<KeyValuePair<string, string>> customHeaders = null)
        //{
        //    var request = new HttpRequestMessage()
        //    {
        //        RequestUri = new Uri(client.BaseAddress.AbsoluteUri + route),
        //        Method = HttpMethod.Post,
        //        Content = new StringContent(JsonConvert.SerializeObject(input), Encoding.UTF8, "application/json")
        //    };
        //    if (customHeaders != null && customHeaders.Any())
        //    {
        //        foreach (var h in customHeaders)
        //        {
        //            request.Headers.Add(h.Key, h.Value);
        //        }
        //    }

        //    using (var res = await client.SendAsync(request))
        //    {
        //        return await res.Content.ReadAsAsync<O>();
        //    }
        //}

        //public static async Task<O> PostSimpleAsync<T, O>(this HttpClient client, T input, string route)
        //{
        //    int retryTimes = 1;
        //    string inputString = JsonConvert.SerializeObject(input);
        //    do
        //    {
        //        try
        //        {
        //            var Content = new StringContent(inputString, Encoding.UTF8, "application/json");
        //            using (var response = await client.PostAsync($"{client.BaseAddress}{route}", Content))
        //            {
        //                if (!response.IsSuccessStatusCode)
        //                {
        //                    var httpStatusCode = response.StatusCode;
        //                    var msg = $"call {route} fail. http status code = {httpStatusCode}";
        //                    CommonLogger.LogError(msg);
        //                    return default(O);
        //                }

        //                return await response.Content.ReadAsAsync<O>();
        //            }
        //        }
        //        catch (HttpRequestException requestException)
        //        {
        //            //if (requestException.InnerException is WebException webException && webException.Status == WebExceptionStatus.NameResolutionFailure)
        //            if (requestException.InnerException is SocketException webException && webException.SocketErrorCode == SocketError.HostNotFound)
        //            {
        //                CommonLogger.LogError($"call {route} retry {retryTimes}. | inputString={inputString} | {requestException.GetAllExceptionMessages()} | {requestException.StackTrace}");
        //                await Task.Delay((retryTimes * 2) * 1000);
        //            }
        //            else
        //            {
        //                CommonLogger.LogError($"call {route} exception. | inputString={inputString} | {requestException.GetAllExceptionMessages()} | {requestException.StackTrace}");
        //                break;
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            CommonLogger.LogError($"call {route} exception. | inputString={inputString} | {ex.Message} | {ex.StackTrace}");
        //            break;
        //        }
        //    } while (++retryTimes <= 3);

        //    if (retryTimes > 3)
        //    {
        //        CommonLogger.LogError($"call {route} failed. inputString = {inputString}");
        //    }

        //    return default(O);
        //}

        //public static async Task<HttpResponseMessage> SendSimpleAsync(this HttpClient client, string requestUrl, HttpMethod method, string content = null, Dictionary<string, string> headers = null)
        //{
        //    int retryTimes = 1;
        //    do
        //    {
        //        var request = new HttpRequestMessage(method, requestUrl);
        //        if (content != null) request.Content = new StringContent(content, Encoding.UTF8, "application/json");

        //        try
        //        {
        //            if (headers != null && headers.Any())
        //            {
        //                foreach (KeyValuePair<string, string> entry in headers)
        //                {
        //                    request.Headers.Add(entry.Key, entry.Value);
        //                }
        //            }

        //            return await client.SendAsync(request);
        //        }
        //        catch (HttpRequestException requestException)
        //        {
        //            if (requestException.InnerException is SocketException webException && webException.SocketErrorCode == SocketError.HostNotFound)
        //            {
        //                CommonLogger.LogError($"call {request.RequestUri} retry {retryTimes}.");
        //                await Task.Delay((retryTimes * 2) * 1000);
        //            }
        //            else
        //            {
        //                CommonLogger.LogError($"call {request.RequestUri} exception. | {requestException.Message} | {requestException.StackTrace}");
        //                break;
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            CommonLogger.LogError($"call {request.RequestUri} exception. | {ex.Message} | {ex.StackTrace}");
        //            break;
        //        }
        //    } while (++retryTimes <= 3);

        //    if (retryTimes > 3)
        //    {
        //        CommonLogger.LogError($"call {requestUrl} failed.");
        //    }
        //    return new HttpResponseMessage { StatusCode = System.Net.HttpStatusCode.InternalServerError };
        //}

        //public static async Task<O> GetSimpleAsync<O>(this HttpClient client, string route)
        //{
        //    int retryTimes = 1;
        //    do
        //    {
        //        try
        //        {
        //            using (var response = await client.GetAsync(route))
        //            {
        //                if (!response.IsSuccessStatusCode) return default(O);

        //                return await response.Content.ReadAsAsync<O>();
        //            }
        //        }
        //        catch (HttpRequestException requestException)
        //        {
        //            if (requestException.InnerException is SocketException webException && webException.SocketErrorCode == SocketError.HostNotFound)
        //            {
        //                CommonLogger.LogError($"call {route} retry {retryTimes}.");
        //                await Task.Delay((retryTimes * 2) * 1000);
        //            }
        //            else
        //            {
        //                CommonLogger.LogError($"call {route} exception. | {requestException.Message} | {requestException.StackTrace}");
        //                break;
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            CommonLogger.LogError($"call {route} exception. | {ex.Message} | {ex.StackTrace}");
        //            break;
        //        }
        //    } while (++retryTimes <= 3);

        //    if (retryTimes > 3)
        //    {
        //        CommonLogger.LogError($"call {route} failed.");
        //    }
        //    return default(O);
        //}

        ///// <summary>
        ///// Method to invoke delete request
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="httpClient"></param>
        ///// <param name="url"></param>
        ///// <param name="authToken"></param>
        ///// <returns></returns>
        //public static async Task<HttpResponseMessage> DeleteRequest(HttpClient httpClient, string url, Dictionary<string, string> headers = null)
        //{
        //    using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Delete, url))
        //    {
        //        if (headers != null)
        //        {
        //            foreach (KeyValuePair<string, string> entry in headers)
        //            {
        //                request.Headers.Add(entry.Key, entry.Value);
        //            }
        //        }

        //        return await httpClient.SendAsync(request);
        //    }
        //}

        ///// <summary>
        ///// Method to get request to Api
        ///// </summary>
        ///// <param name="httpClient"></param>
        ///// <param name="url"></param>
        ///// <param name="authToken"></param>
        ///// <returns></returns>
        //public static async Task<HttpResponseMessage> GetRequest(HttpClient httpClient, string url, Dictionary<string, string> headers = null)
        //{
        //    using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, url))
        //    {
        //        if (headers != null)
        //        {
        //            foreach (KeyValuePair<string, string> entry in headers)
        //            {
        //                request.Headers.Add(entry.Key, entry.Value);
        //            }
        //        }

        //        return await httpClient.SendAsync(request);
        //    }
        //}

        /// <summary>
        /// ServiceOverview is used to get Api endpoints from Consul
        /// </summary>
        /// <param name="serviceOverview"></param>
        /// <param name="apiName"></param>
        /// <returns></returns>
        public static Uri GetServiceAddress(string apiName)
        {
            lock (apiName)
            {
                //ToDo
                return new Uri(apiName);
            }
        }

        ///// <summary>
        ///// Returns payload object from request header
        ///// </summary>
        ///// <param name="request"></param>
        ///// <returns></returns>
        //public static AuthTokenPayload GetPayload(Microsoft.AspNetCore.Http.HttpRequest request)
        //{
        //    try
        //    {
        //        var authToken = string.Empty;
        //        if (request.Headers.ContainsKey(B2BConstants.XToken))
        //        {
        //            authToken = Convert.ToString(request.Headers[B2BConstants.XToken]);
        //        }

        //        //var authToken = request.Cookies[B2BConstants.XToken] ?? string.Empty;
        //        if (string.IsNullOrEmpty(authToken))
        //        {
        //            authToken = request.Cookies[B2BConstants.XToken] ?? string.Empty;

        //            if (string.IsNullOrEmpty(authToken)) return null;
        //        }
        //        return TokenEngine.DecodeTokenPayload<AuthTokenPayload>(authToken);
        //    }
        //    catch (Exception e)
        //    {
        //        CommonLogger.LogError(e);
        //        throw;
        //    }
        //}
    }
}
