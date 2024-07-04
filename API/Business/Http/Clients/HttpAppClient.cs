using Business.Http.Clients.Interfaces;

namespace Business.Http.Clients
{
    public class HttpAppClient : IHttpAppClient
    {

        private readonly HttpClient _httpClient;



        public HttpAppClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }





        public async Task<HttpResponseMessage> Send(HttpRequestMessage requestMessage)
        {
            var result = await _httpClient.SendAsync(requestMessage);

            return result;
        }
    }
}
