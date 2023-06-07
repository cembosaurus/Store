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

        private readonly HttpRequestMessage _request;
        private readonly HttpClient _httpClient;
        private readonly string _baseUri;
        private readonly Encoding _encoding = Encoding.UTF8;
        private readonly string _mediaType = "application/json";

        private static IHttpContextAccessor _accessor;
        private static string _token => _accessor == null ? "" : _accessor.HttpContext?.Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "") ?? "";

        public HttpIdentityClient(HttpClient httpClient, IConfiguration config, IHttpContextAccessor accessor)
        {
            _httpClient = httpClient;
            _baseUri = config.GetSection("RemoteServices:IdentityService").Value + "/api";
            _accessor = accessor;
            _request = new HttpRequestMessage { RequestUri = new Uri(_baseUri) };
            _request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(_mediaType));
            _request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _token);
        }





        public async Task<HttpResponseMessage> Register(UserToRegisterDTO user)
        {
            _request.Method = HttpMethod.Post;
            _request.RequestUri = new Uri(_request.RequestUri + $"/identity/register");
            _request.Content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(user), _encoding, _mediaType);

            Console.WriteLine($"---> REGISTERING user '{user.Name}' ....");

            return await _httpClient.SendAsync(_request);
        }



        public async Task<HttpResponseMessage> Login(UserToLoginDTO user)
        {
            _request.Method = HttpMethod.Post;
            _request.RequestUri = new Uri(_request.RequestUri + $"/identity/login");
            _request.Content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(user), _encoding, _mediaType);

            Console.WriteLine($"---> LOGGING IN user '{user.Name}' ....");

            return await _httpClient.SendAsync(_request);
        }


        public async Task<HttpResponseMessage> AuthenticateService(string apiKey)
        {
            _request.Method = HttpMethod.Post;
            _request.RequestUri = new Uri(_request.RequestUri + $"/identity/service/authenticate");

            // Api Key sent in header, not in body:
            _request.Headers.Add("ApiKey", apiKey);

            Console.WriteLine($"--> AUTHENTICATING service ....");

            return await _httpClient.SendAsync(_request);
        }

    }
}
