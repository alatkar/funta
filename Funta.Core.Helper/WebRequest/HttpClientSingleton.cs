using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Funta.Core.Helper.WebRequest
{
    public class HttpClientSingleton
    {
        private readonly ConcurrentDictionary<Uri, Lazy<HttpClient>> _httpClients =
                         new ConcurrentDictionary<Uri, Lazy<HttpClient>>();
        private const int ConnectionLeaseTimeout = 60 * 1000;

        public HttpClientSingleton()
        {
            ServicePointManager.DnsRefreshTimeout = (int)TimeSpan.FromMinutes(1).TotalMilliseconds;
            ServicePointManager.DefaultConnectionLimit = 1024;
        }

        public HttpClient GetOrCreate(
           Uri baseAddress,
           IDictionary<string, string> defaultRequestHeaders = null,
           TimeSpan? timeout = null,
           long? maxResponseContentBufferSize = null,
           HttpMessageHandler handler = null)
        {
            return _httpClients.GetOrAdd(baseAddress,
                             uri => new Lazy<HttpClient>(() =>
                             {
                                 var client = handler == null ? new HttpClient { BaseAddress = baseAddress } :
                                                   new HttpClient(handler, disposeHandler: false) { BaseAddress = baseAddress };
                                 setRequestTimeout(timeout, client);
                                 setMaxResponseBufferSize(maxResponseContentBufferSize, client);
                                 setDefaultHeaders(defaultRequestHeaders, client);
                                 setConnectionLeaseTimeout(baseAddress, client);
                                 return client;
                             },
                             LazyThreadSafetyMode.ExecutionAndPublication)).Value;
        }

        public void Dispose()
        {
            foreach (var httpClient in _httpClients.Values)
            {
                httpClient.Value.Dispose();
            }
        }

        private static void setConnectionLeaseTimeout(Uri baseAddress, HttpClient client)
        {
            client.DefaultRequestHeaders.ConnectionClose = false;
            ServicePointManager.FindServicePoint(baseAddress).ConnectionLeaseTimeout = ConnectionLeaseTimeout;
        }

        private static void setDefaultHeaders(IDictionary<string, string> defaultRequestHeaders, HttpClient client)
        {
            if (defaultRequestHeaders == null)
            {
                return;
            }
            foreach (var item in defaultRequestHeaders)
            {
                client.DefaultRequestHeaders.Add(item.Key, item.Value);
            }
        }

        private static void setMaxResponseBufferSize(long? maxResponseContentBufferSize, HttpClient client)
        {
            if (maxResponseContentBufferSize.HasValue)
            {
                client.MaxResponseContentBufferSize = maxResponseContentBufferSize.Value;
            }
        }

        private static void setRequestTimeout(TimeSpan? timeout, HttpClient client)
        {
            if (timeout.HasValue)
            {
                client.Timeout = timeout.Value;
            }
        }
    }

    public static class HttpClientSend
    {
        public static async Task<T> Send<T>(string host, string url, CancellationToken cancellationToken, object stringContent = null, Dictionary<string, string> formUrlEncodedContent = null, string ver = "", bool isHttps = false, bool isPost = true)
        {
            try
            {
                HttpClientSingleton _httpClientFactory = new HttpClientSingleton();

                var Content = new StringContent(JsonConvert.SerializeObject(stringContent),
                                                        Encoding.UTF8,
                                                        "application/json");
                Dictionary<string, string> header = new Dictionary<string, string>();
                if (!string.IsNullOrEmpty(ver))
                    header["api-version"] = ver;

                var httpClient = _httpClientFactory.GetOrCreate(new Uri(host), header);

                if (isHttps)
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
                                               | SecurityProtocolType.Tls11
                                               | SecurityProtocolType.Tls12;
                HttpResponseMessage responseMessage;
                if (formUrlEncodedContent != null)
                    responseMessage = await httpClient.PostAsync(url, new FormUrlEncodedContent(formUrlEncodedContent), cancellationToken);
                else
                {
                    if (isPost)
                        responseMessage = await httpClient.PostAsync(url, Content);
                    else
                        responseMessage = await httpClient.GetAsync(url);
                }

                if (responseMessage.IsSuccessStatusCode)
                {
                    var responseContent = await responseMessage.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<T>(responseContent);
                }
                else
                {
                    return default(T);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
