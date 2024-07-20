using Business.Http.Clients.Interfaces;
using Business.Metrics.Http.Clients;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;



namespace Business.Http.Clients
{
    public class HttpAppClient : HttpMetricsClient, IHttpAppClient
    {

        public HttpAppClient(HttpClient httpClient, IHttpContextAccessor accessor, IConfiguration config) 
            : base(httpClient, accessor, config)
        {

        }





        public async Task<HttpResponseMessage> Send(HttpRequestMessage requestMessage)
        {
            var _result = await base.Send(requestMessage);

            return _result;
        }





    }
}
