using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Net.Http.Headers;

namespace Mango.GatewaySolution.Services
{
    public sealed class HttpClientService 
    {
        private readonly ILogger<HttpClientService> _logger;

        public HttpClientService(ILogger<HttpClientService> logger)
        {
            _logger = logger;
        }


        public async Task<string> GetAsync(string requestUri, Dictionary<string, string> additionalHeaders = null)
        {
            using HttpClientHandler httpClientHandler = new HttpClientHandler();
            using HttpClient httpClient = new HttpClient(httpClientHandler);
            httpClient.Timeout = TimeSpan.FromMinutes(10);
            AddHeaders(httpClient, additionalHeaders);
            return await ProcessingHttpResponse(await httpClient.GetAsync(requestUri));
        }

        public async Task<string> PostAsync<T>(string requestUri, T request, Dictionary<string, string> additionalHeaders = null) where T : class
        {
            using HttpClientHandler httpClientHandler = new HttpClientHandler();
            using HttpClient httpClient = new HttpClient(httpClientHandler);
            AddHeaders(httpClient, additionalHeaders);
            return await ProcessingHttpResponse(await httpClient.PostAsync(requestUri, new StringContent(JsonConvert.SerializeObject(request))
            {
                Headers =
                        {
                            ContentType = new MediaTypeHeaderValue("application/json")
                        }
            }));
        }

        public async Task<string> DeleteAsync(string requestUri, Dictionary<string, string> additionalHeaders = null)
        {
            using HttpClientHandler httpClientHandler = new HttpClientHandler();
            using HttpClient httpClient = new HttpClient(httpClientHandler);
            AddHeaders(httpClient, additionalHeaders);
            var httpResponseMessage = await httpClient.DeleteAsync(requestUri);
            return await ProcessingHttpResponse(await httpClient.DeleteAsync(requestUri));
        }

        public async Task<string> PutAsync<T>(string requestUri, T request, Dictionary<string, string> additionalHeaders = null) where T : class
        {
            using HttpClientHandler httpClientHandler = new HttpClientHandler();
            using HttpClient httpClient = new HttpClient(httpClientHandler);
            AddHeaders(httpClient, additionalHeaders);

            var json = JsonConvert.SerializeObject(request, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                NullValueHandling = NullValueHandling.Ignore
            });

            var httpContent = new StringContent(json);
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var httpResponseMessage = await httpClient.PutAsync(requestUri, httpContent);
            return await ProcessingHttpResponse(await httpClient.PutAsync(requestUri, httpContent));
        }

        public async Task<string> PatchAsync<T>(string requestUri, T request, Dictionary<string, string> additionalHeaders = null) where T : class
        {
            using HttpClientHandler httpClientHandler = new HttpClientHandler();
            using HttpClient httpClient = new HttpClient(httpClientHandler);
            AddHeaders(httpClient, additionalHeaders);

            var json = JsonConvert.SerializeObject(request, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                NullValueHandling = NullValueHandling.Ignore
            });

            var httpContent = new StringContent(json);
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            return await ProcessingHttpResponse(await httpClient.PatchAsync(requestUri, httpContent));
        }


        public async Task<string> PostAsFormData(string requestUri, List<KeyValuePair<string, string>> request, Dictionary<string, string> additionalHeaders = null)
        {
            using HttpClientHandler httpClientHandler = new HttpClientHandler();
            using HttpClient httpClient = new HttpClient(httpClientHandler);
            AddHeaders(httpClient, additionalHeaders);
            var formContent = new FormUrlEncodedContent(request);
            return await ProcessingHttpResponse(await httpClient.PostAsync(requestUri, formContent));
        }


        private void AddHeaders(HttpClient httpClient, Dictionary<string, string> additionalHeaders, bool ignoreDefaultAccept = false)
        {
            if (!ignoreDefaultAccept)
                httpClient.DefaultRequestHeaders.Add("Accept", "application/json");

            //No additional headers to be added
            if (additionalHeaders == null)
                return;

            foreach (KeyValuePair<string, string> current in additionalHeaders)
            {
                httpClient.DefaultRequestHeaders.Add(current.Key, current.Value);
            }
        }

        private async Task<string> ProcessingHttpResponse(HttpResponseMessage httpResponseMessage)
        {
            if (!httpResponseMessage.IsSuccessStatusCode)
            {
                _logger.LogError($"Error while api call {await httpResponseMessage.Content.ReadAsStringAsync()}");
                throw new Exception("Error while processing");
            }
            return await httpResponseMessage.Content.ReadAsStringAsync();
        }
    }
}
