using Business.Identity.Http.Clients.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Net.Http.Headers;
using System.Net.Http.Headers;
using System.Text;

namespace Business.Identity.Http.Clients
{
    public class HttpUserClient : IHttpUserClient
    {

        private HttpRequestMessage _request;
        private readonly HttpClient _httpClient;
        private readonly string _baseUri;
        private readonly Encoding _encoding = Encoding.UTF8;
        private readonly string _mediaType = "application/json";

        private static IHttpContextAccessor _accessor;
        private static string _token => _accessor == null ? "" : _accessor.HttpContext?.Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "") ?? "";

        public HttpUserClient(HttpClient httpClient, IConfiguration config, IHttpContextAccessor accessor)
        {
            _httpClient = httpClient;
            _baseUri = config.GetSection("RemoteServices:IdentityService").Value + "/api";
            _accessor = accessor;
        }




        public async Task<HttpResponseMessage> EditUserRoles(int id, IEnumerable<string> roles)
        {
            InitializeHttpRequestMessage(
                HttpMethod.Put,
                $"/user/{id}/changeroles",
                new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(roles), _encoding, _mediaType)
            );

            Console.WriteLine($"---> UPDATING roles for user '{id}'....");

            return await _httpClient.SendAsync(_request);
        }


        public async Task<HttpResponseMessage> GetAllUsers()
        {
            InitializeHttpRequestMessage(
                HttpMethod.Get,
                $"/user"
            );

            Console.WriteLine($"---> GETTING all users ....");

            return await _httpClient.SendAsync(_request);
        }


        public async Task<HttpResponseMessage> GetAllUsersWithRoles()
        {
            InitializeHttpRequestMessage(
                HttpMethod.Get,
                $"/user/withroles"
            );

            Console.WriteLine($"---> GETTING all users with roles ....");

            return await _httpClient.SendAsync(_request);
        }


        public async Task<HttpResponseMessage> GetUserById(int id)
        {
            InitializeHttpRequestMessage(
                HttpMethod.Get,
                $"/user/{id}"
            );

            Console.WriteLine($"---> GETTING user '{id}'....");

            return await _httpClient.SendAsync(_request);
        }


        public async Task<HttpResponseMessage> GetUserByName(string name)
        {
            InitializeHttpRequestMessage(
                HttpMethod.Get,
                $"/user/name/{name}"
            );

            Console.WriteLine($"---> GETTING user '{name}'....");

            return await _httpClient.SendAsync(_request);
        }


        public async Task<HttpResponseMessage> GetUserWithRoles(int id)
        {
            InitializeHttpRequestMessage(
                HttpMethod.Get,
                $"/user/{id}/withroles"
            );

            Console.WriteLine($"---> GETTING user '{id}' with roles ....");

            return await _httpClient.SendAsync(_request);
        }


        public async Task<HttpResponseMessage> GetCurrentUser()
        {
            InitializeHttpRequestMessage(
                HttpMethod.Get,
                $"/user/current"
            );

            Console.WriteLine($"---> GETTING current user ....");

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
