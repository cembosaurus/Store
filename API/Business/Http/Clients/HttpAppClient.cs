using Business.Http.Clients.Interfaces;
using Business.Metrics.Http.Clients.Interfaces;



namespace Business.Http.Clients
{
    public class HttpAppClient : IHttpAppClient
    {

        private IHttpClient_Metrics _httpClient;


        public HttpAppClient(IHttpClient_Metrics httpClient) 
        {
            _httpClient = httpClient;
        }





        public async Task<HttpResponseMessage> SendAsync(HttpRequestMessage requestMessage)
        {
            // to implement Metrics into project, inherit from HttpMetricsClient
            // and replace calling to HttpClient's SendAsync() by its SendAsync() method.
            var _result = await _httpClient.SendAsync(requestMessage);

            return _result;
        }





    }
}
