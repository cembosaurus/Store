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

        private HttpRequestMessage _request;
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
        }





        public async Task<HttpResponseMessage> Register(UserToRegisterDTO user)
        {
            InitializeHttpRequestMessage(
                HttpMethod.Post,
                $"/identity/register",
                new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(user), _encoding, _mediaType)
            );

            Console.WriteLine($"---> REGISTERING user '{user.Name}' ....");

            return await _httpClient.SendAsync(_request);
        }



        public async Task<HttpResponseMessage> Login(UserToLoginDTO user)
        {
            InitializeHttpRequestMessage(
                HttpMethod.Post,
                $"/identity/login",
                new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(user), _encoding, _mediaType)
            );

            Console.WriteLine($"---> LOGGING IN user '{user.Name}' ....");

            return await _httpClient.SendAsync(_request);
        }


        public async Task<HttpResponseMessage> AuthenticateService(string apiKey)
        {
            InitializeHttpRequestMessage(
                HttpMethod.Post,
                $"/identity/service/authenticate"
            );

            // Api Key sent in header, not in body:
            _request.Headers.Add("ApiKey", apiKey);

            Console.WriteLine($"--> AUTHENTICATING service ....");

            return await _httpClient.SendAsync(_request);
        }





        private void InitializeHttpRequestMessage(HttpMethod method, string uri, HttpContent content = default)
        {
            _request = new HttpRequestMessage { RequestUri = new Uri(_baseUri + uri) };
            _request.Method = method;
            _request.Content = content;
            _request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(_mediaType));
            _request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _token);
        }
    }
}
