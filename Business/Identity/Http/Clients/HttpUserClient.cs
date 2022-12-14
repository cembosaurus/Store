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

        private readonly HttpClient _httpClient;
        private readonly string _baseUri;
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
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Put,
                RequestUri = new Uri($"{_baseUri}/user/{id}/changeroles"),
                Content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(roles), Encoding.UTF8, "application/json")
            };

            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _token);

            Console.WriteLine($"---> UPDATING roles for user '{id}'....");

            return await _httpClient.SendAsync(request);
        }


        public async Task<HttpResponseMessage> GetAllUsers()
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"{_baseUri}/user")
            };

            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _token);

            Console.WriteLine($"---> GETTING all users ....");

            return await _httpClient.SendAsync(request);
        }


        public async Task<HttpResponseMessage> GetAllUsersWithRoles()
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"{_baseUri}/user/withroles")
            };

            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _token);

            Console.WriteLine($"---> GETTING all users with roles ....");

            return await _httpClient.SendAsync(request);
        }


        public async Task<HttpResponseMessage> GetUserById(int id)
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"{_baseUri}/user/{id}")
            };

            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _token);

            Console.WriteLine($"---> GETTING user '{id}'....");

            return await _httpClient.SendAsync(request);
        }


        public async Task<HttpResponseMessage> GetUserByName(string name)
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"{_baseUri}/user/name/{name}")
            };

            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _token);

            Console.WriteLine($"---> GETTING user '{name}'....");

            return await _httpClient.SendAsync(request);
        }


        public async Task<HttpResponseMessage> GetUserWithRoles(int id)
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"{_baseUri}/user/{id}/withroles")
            };

            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _token);

            Console.WriteLine($"---> GETTING user '{id}' with roles ....");

            return await _httpClient.SendAsync(request);
        }


        public async Task<HttpResponseMessage> GetCurrentUser()
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"{_baseUri}/user/current")
            };

            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _token);

            Console.WriteLine($"---> GETTING current user ....");

            return await _httpClient.SendAsync(request);
        }
    }
}
