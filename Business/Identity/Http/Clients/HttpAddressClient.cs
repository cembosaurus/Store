using Business.Identity.DTOs;
using Business.Identity.Http.Clients.Interfaces;
using Business.Identity.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Net.Http.Headers;
using System.Net.Http.Headers;
using System.Text;

namespace Business.Identity.Http.Clients
{
    public class HttpAddressClient : IHttpAddressClient
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUri;
        private static IHttpContextAccessor _accessor;

        private static string _token => _accessor == null ? "" : _accessor.HttpContext?.Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "") ?? "";

        public HttpAddressClient(HttpClient httpClient, IConfiguration config, IHttpContextAccessor accessor)
        {
            _httpClient = httpClient;
            _baseUri = config.GetSection("RemoteServices:IdentityService").Value + "/api";
            _accessor = accessor;
        }




        public async Task<HttpResponseMessage> GetAllAddresses()
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"{_baseUri}/address/all")
            };

            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _token);

            Console.WriteLine($"---> GETTING all Addresses ....");

            return await _httpClient.SendAsync(request);
        }


        public async Task<HttpResponseMessage> GetAddressByAddressId(int addressId)
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"{_baseUri}/address/{addressId}")
            };

            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _token);

            Console.WriteLine($"---> GETTING Address '{addressId}' ....");

            return await _httpClient.SendAsync(request);
        }



        public async Task<HttpResponseMessage> GetAddressesByAddressIds(IEnumerable<int> addressesIds)
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"{_baseUri}/address"),
                Content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(addressesIds), Encoding.UTF8, "application/json")
            };

            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _token);

            Console.WriteLine($"---> GETTING addresses by Ids ....");

            return await _httpClient.SendAsync(request);
        }



        public async Task<HttpResponseMessage> ExistsAddressByAddressId(int addressId)
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"{_baseUri}/address/{addressId}/exists")
            };

            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _token);

            Console.WriteLine($"---> EXISTS address '{addressId}'....");

            return await _httpClient.SendAsync(request);
        }



        public async Task<HttpResponseMessage> AddAddressToUser(int userId, AddressCreateDTO addressDto)
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri($"{_baseUri}/address/{userId}"),
                Content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(addressDto), Encoding.UTF8, "application/json")
            };

            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _token);

            Console.WriteLine($"---> ADDING address to user '{userId}'....");

            return await _httpClient.SendAsync(request);
        }



        public async Task<HttpResponseMessage> DeleteAddress(int id)
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Delete,
                RequestUri = new Uri($"{_baseUri}/address/{id}")
            };

            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _token);

            Console.WriteLine($"---> DELETING address '{id}'....");

            return await _httpClient.SendAsync(request);
        }



        public async Task<HttpResponseMessage> GetAddressesByUserId(int userId)
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"{_baseUri}/address/user/{userId}")
            };

            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _token);

            Console.WriteLine($"---> GETTTING address for user '{userId}'....");

            return await _httpClient.SendAsync(request);
        }



        public async Task<HttpResponseMessage> SearchAddress(SearchAddressModel searchModel)
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"{_baseUri}/address"),
                Content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(searchModel), Encoding.UTF8, "application/json")
            };

            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _token);

            Console.WriteLine($"---> SEARCHING for address ....");

            return await _httpClient.SendAsync(request);
        }



        public async Task<HttpResponseMessage> UpdateAddress(int id, AddressUpdateDTO addressDto)
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Put,
                RequestUri = new Uri($"{_baseUri}/address/{id}"),
                Content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(addressDto), Encoding.UTF8, "application/json")
            };

            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _token);

            Console.WriteLine($"---> UPDATING address '{id}'....");

            return await _httpClient.SendAsync(request);
        }
    }
}
