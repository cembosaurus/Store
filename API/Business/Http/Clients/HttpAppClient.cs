using Business.Http.Clients.Interfaces;
using Business.Metrics.Http.Clients.Interfaces;



namespace Business.Http.Clients
{
    public class HttpAppClient : IHttpAppClient
    {

        private IHttpClient_Metrics _httpMetricsClient;
        private HttpResponseMessage _httpResponseMessage;


        public HttpAppClient(IHttpClient_Metrics httpMetricsClient) 
        {
            _httpMetricsClient = httpMetricsClient;
        }





        public async Task<HttpResponseMessage> SendAsync(HttpRequestMessage requestMessage)
        {
            // to implement Metrics into project replace call to HttpClient by HttpMetricsClient:

            try
            {
                _httpResponseMessage = await _httpMetricsClient.SendAsync(requestMessage);

            }
            catch (Exception)
            {

                throw;
            }

            return _httpResponseMessage;
        }





    }
}
