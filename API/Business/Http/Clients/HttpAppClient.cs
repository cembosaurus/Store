using Business.Http.Clients.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;



namespace Business.Http.Clients
{
    public class HttpAppClient : IHttpAppClient
    {

        private HttpClient _httpClient;


        public HttpAppClient(HttpClient httpClient, IWebHostEnvironment env)
        {
            _httpClient = httpClient;
            SetDebugTimeout(env.IsProduction());
        }





        public async Task<HttpResponseMessage> SendAsync(HttpRequestMessage requestMessage)
        {
            return await _httpClient.SendAsync(requestMessage);
        }


        private void SetDebugTimeout(bool prodEnv)
        { 
            if(!prodEnv)
                _httpClient.Timeout = TimeSpan.FromMinutes(3);
        }



    }
}