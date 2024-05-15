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

        private HttpRequestMessage _requestMessage;
        private readonly HttpClient _httpClient;
        private readonly string _requestUri;
        private string _requestQuery;
        private HttpMethod _method;
        private HttpContent _content;
        private readonly Encoding _encoding = Encoding.UTF8;
        private readonly string _mediaType = "application/json";

        private static IHttpContextAccessor _accessor;
        private static string _token => _accessor == null ? "" : _accessor.HttpContext?.Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "") ?? "";

        public HttpIdentityClient(HttpClient httpClient, IConfiguration config, IHttpContextAccessor accessor)
        {
            _httpClient = httpClient;
            _requestUri = config.GetSection("RemoteServices:IdentityService").Value + "/api";
            _accessor = accessor;
        }





        public async Task<HttpResponseMessage> Register(UserToRegisterDTO user)
        {
            _method = HttpMethod.Post;
            _requestQuery = $"/identity/register";
            _content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(user), _encoding, _mediaType);

            InitializeHttpRequestMessage();

            Console.WriteLine($"---> REGISTERING user '{user.Name}' ....");

            return await _httpClient.SendAsync(_requestMessage);
        }



        public async Task<HttpResponseMessage> Login(UserToLoginDTO user)
        {
            _method = HttpMethod.Post;
            _requestQuery = $"/identity/login";
            _content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(user), _encoding, _mediaType);

            InitializeHttpRequestMessage();

            Console.WriteLine($"---> LOGGING IN user '{user.Name}' ....");

            return await _httpClient.SendAsync(_requestMessage);
        }


        public async Task<HttpResponseMessage> AuthenticateService(string apiKey)
        {
            _method = HttpMethod.Post;
            _requestQuery = $"/identity/service/authenticate";

            InitializeHttpRequestMessage();

            // Api Key transported in header, not in body:
            _requestMessage.Headers.Add("ApiKey", apiKey);

            Console.WriteLine($"--> AUTHENTICATING service ....");

            return await _httpClient.SendAsync(_requestMessage);
        }





        private void InitializeHttpRequestMessage()
        {
            _requestMessage = new HttpRequestMessage { RequestUri = new Uri(_requestUri + _requestQuery) };         //----- To Do: Base URL not fetched !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            _requestMessage.Method = _method;
            _requestMessage.Content = _content;
            _requestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(_mediaType));
            _requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _token);
        }
    }
}
