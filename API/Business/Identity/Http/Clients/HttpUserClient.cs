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

        private readonly HttpRequestMessage _request;
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
            _request = new HttpRequestMessage { RequestUri = new Uri(_baseUri) };
            _request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(_mediaType));
            _request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _token);
        }




        public async Task<HttpResponseMessage> EditUserRoles(int id, IEnumerable<string> roles)
        {
            _request.Method = HttpMethod.Put;
            _request.RequestUri = new Uri(_request.RequestUri + $"/user/{id}/changeroles");
            _request.Content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(roles), _encoding, _mediaType);

            Console.WriteLine($"---> UPDATING roles for user '{id}'....");

            return await _httpClient.SendAsync(_request);
        }


        public async Task<HttpResponseMessage> GetAllUsers()
        {
            _request.Method = HttpMethod.Get;
            _request.RequestUri = new Uri(_request.RequestUri + $"/user");

            Console.WriteLine($"---> GETTING all users ....");

            return await _httpClient.SendAsync(_request);
        }


        public async Task<HttpResponseMessage> GetAllUsersWithRoles()
        {
            _request.Method = HttpMethod.Get;
            _request.RequestUri = new Uri(_request.RequestUri + $"/user/withroles");

            Console.WriteLine($"---> GETTING all users with roles ....");

            return await _httpClient.SendAsync(_request);
        }


        public async Task<HttpResponseMessage> GetUserById(int id)
        {
            _request.Method = HttpMethod.Get;
            _request.RequestUri = new Uri(_request.RequestUri + $"/user/{id}");

            Console.WriteLine($"---> GETTING user '{id}'....");

            return await _httpClient.SendAsync(_request);
        }


        public async Task<HttpResponseMessage> GetUserByName(string name)
        {
            _request.Method = HttpMethod.Get;
            _request.RequestUri = new Uri(_request.RequestUri + $"/user/name/{name}");

            Console.WriteLine($"---> GETTING user '{name}'....");

            return await _httpClient.SendAsync(_request);
        }


        public async Task<HttpResponseMessage> GetUserWithRoles(int id)
        {
            _request.Method = HttpMethod.Get;
            _request.RequestUri = new Uri(_request.RequestUri + $"/user/{id}/withroles");

            Console.WriteLine($"---> GETTING user '{id}' with roles ....");

            return await _httpClient.SendAsync(_request);
        }


        public async Task<HttpResponseMessage> GetCurrentUser()
        {
            _request.Method = HttpMethod.Get;
            _request.RequestUri = new Uri(_request.RequestUri + $"/user/current");

            Console.WriteLine($"---> GETTING current user ....");

            return await _httpClient.SendAsync(_request);
        }
    }
}
