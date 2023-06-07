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

        private readonly HttpRequestMessage _request;
        private readonly HttpClient _httpClient;
        private readonly string _baseUri;
        private readonly Encoding _encoding = Encoding.UTF8;
        private readonly string _mediaType = "application/json";

        private static IHttpContextAccessor _accessor;
        private static string _token => _accessor == null ? "" : _accessor.HttpContext?.Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "") ?? "";

        public HttpAddressClient(HttpClient httpClient, IConfiguration config, IHttpContextAccessor accessor)
        {
            _httpClient = httpClient;
            _baseUri = config.GetSection("RemoteServices:IdentityService").Value + "/api";
            _accessor = accessor;
            _request = new HttpRequestMessage { RequestUri = new Uri(_baseUri) };
            _request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(_mediaType));
            _request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _token);
        }




        public async Task<HttpResponseMessage> GetAllAddresses()
        {
            _request.Method = HttpMethod.Get;
            _request.RequestUri = new Uri(_request.RequestUri + $"/address/all");

            Console.WriteLine($"---> GETTING all Addresses ....");

            return await _httpClient.SendAsync(_request);
        }


        public async Task<HttpResponseMessage> GetAddressByAddressId(int addressId)
        {
            _request.Method = HttpMethod.Get;
            _request.RequestUri = new Uri(_request.RequestUri + $"/address/{addressId}");

            Console.WriteLine($"---> GETTING Address '{addressId}' ....");

            return await _httpClient.SendAsync(_request);
        }



        public async Task<HttpResponseMessage> GetAddressesByAddressIds(IEnumerable<int> addressesIds)
        {
            _request.Method = HttpMethod.Get;
            _request.RequestUri = new Uri(_request.RequestUri + $"/address");
            _request.Content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(addressesIds), _encoding, _mediaType);

            Console.WriteLine($"---> GETTING addresses by Ids ....");

            return await _httpClient.SendAsync(_request);
        }



        public async Task<HttpResponseMessage> ExistsAddressByAddressId(int addressId)
        {
            _request.Method = HttpMethod.Get;
            _request.RequestUri = new Uri(_request.RequestUri + $"/address/{addressId}/exists");

            Console.WriteLine($"---> EXISTS address '{addressId}'....");

            return await _httpClient.SendAsync(_request);
        }



        public async Task<HttpResponseMessage> AddAddressToUser(int userId, AddressCreateDTO addressDto)
        {
            _request.Method = HttpMethod.Post;
            _request.RequestUri = new Uri(_request.RequestUri + $"/address/{userId}");
            _request.Content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(addressDto), _encoding, _mediaType);

            Console.WriteLine($"---> ADDING address to user '{userId}'....");

            return await _httpClient.SendAsync(_request);
        }



        public async Task<HttpResponseMessage> DeleteAddress(int id)
        {
            _request.Method = HttpMethod.Delete;
            _request.RequestUri = new Uri(_request.RequestUri + $"/address/{id}");

            Console.WriteLine($"---> DELETING address '{id}'....");

            return await _httpClient.SendAsync(_request);
        }



        public async Task<HttpResponseMessage> GetAddressesByUserId(int userId)
        {
            _request.Method = HttpMethod.Get;
            _request.RequestUri = new Uri(_request.RequestUri + $"/address/user/{userId}");

            Console.WriteLine($"---> GETTTING address for user '{userId}'....");

            return await _httpClient.SendAsync(_request);
        }



        public async Task<HttpResponseMessage> SearchAddress(SearchAddressModel searchModel)
        {
            _request.Method = HttpMethod.Get;
            _request.RequestUri = new Uri(_request.RequestUri + $"/address");
            _request.Content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(searchModel), _encoding, _mediaType);

            Console.WriteLine($"---> SEARCHING for address ....");

            return await _httpClient.SendAsync(_request);
        }



        public async Task<HttpResponseMessage> UpdateAddress(int id, AddressUpdateDTO addressDto)
        {
            _request.Method = HttpMethod.Put;
            _request.RequestUri = new Uri(_request.RequestUri + $"/address/{id}");
            _request.Content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(addressDto), _encoding, _mediaType);

            Console.WriteLine($"---> UPDATING address '{id}'....");

            return await _httpClient.SendAsync(_request);
        }
    }
}
