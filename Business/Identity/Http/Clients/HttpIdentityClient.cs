using Business.Identity.DTOs;
using Business.Identity.Http.Clients.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Net.Http.Headers;
using System.Net.Http.Headers;
using System.Text;

namespace Business.Identity.Http.Clients
{
    public class HttpIdentityClient : IHttpIdentityClient
    {


        private readonly HttpClient _httpClient;
        private readonly string _baseUri;
        private static IHttpContextAccessor _accessor;

        private static string _token => _accessor == null ? "" : _accessor.HttpContext?.Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "") ?? "";

        public HttpIdentityClient(HttpClient httpClient, IConfiguration config, IHttpContextAccessor accessor)
        {
            _httpClient = httpClient;
            _baseUri = config.GetSection("RemoteServices:IdentityService").Value + "/api";
            _accessor = accessor;
        }





        public async Task<HttpResponseMessage> Register(UserToRegisterDTO user)
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri($"{_baseUri}/identity/register"),
                Content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json")
            };

            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _token);

            Console.WriteLine($"---> REGISTERING user '{user.Name}' ....");

            return await _httpClient.SendAsync(request);
        }



        public async Task<HttpResponseMessage> Login(UserToLoginDTO user)
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri($"{_baseUri}/identity/login"),
                Content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json")
            };

            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _token);

            Console.WriteLine($"---> LOGGING IN user '{user.Name}' ....");

            return await _httpClient.SendAsync(request);
        }


        public async Task<HttpResponseMessage> AuthenticateService(string apiKey)
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri($"{_baseUri}/identity/service/authenticate")
            };

            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            // Api Key sent in header, not in body:
            request.Headers.Add("ApiKey", apiKey);

            Console.WriteLine($"--> AUTHENTICATING service ....");

            return await _httpClient.SendAsync(request);
        }

    }
}
