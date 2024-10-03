namespace IoTDataCollector.API.Helpers
{
    public class HttpClientHelper
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public HttpClientHelper(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<HttpResponseMessage> PostAsync(string url, HttpContent content = null)
        {
            var client = _httpClientFactory.CreateClient();
            return await client.PostAsync(url, content);
        }
    }
}
