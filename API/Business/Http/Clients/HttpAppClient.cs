﻿using Business.Http.Clients.Interfaces;
using Business.Metrics.Http.Clients.Interfaces;



namespace Business.Http.Clients
{
    public class HttpAppClient : IHttpAppClient
    {

        private IHttpClient_Metrics _httpMetricsClient;


        // to implement Metrics into project replace call to HttpClient by HttpMetricsClient:

        public HttpAppClient(IHttpClient_Metrics httpMetricsClient) 
        {
            _httpMetricsClient = httpMetricsClient;
        }





        public async Task<HttpResponseMessage> SendAsync(HttpRequestMessage requestMessage, bool bypassMetrics = default)
        {
            return await (bypassMetrics ?
                _httpMetricsClient.HtpClient.SendAsync(requestMessage) :
                _httpMetricsClient.SendAsync(requestMessage));
        }





    }
}
