using API.Services.Interfaces;

namespace API.Services
{
    public class HttpClientService : IHttpClientService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public HttpClientService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public HttpClient CreateClient()
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(GetBaseAddress());
            
            return client;
        }

        public string GetBaseAddress()
        {
            return "https://recruitment.highfieldqualifications.com";
        }
    }

}
