using Business.Inventory.DTOs.Item;
using Business.Inventory.Http.Interfaces;
using Business.Libraries.Http.Interfaces;
using Business.Libraries.ServiceResult;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Net.Http.Headers;
using System.Net.Http.Headers;
using System.Text;

namespace Business.Inventory.Http
{
    public class HttpItemClient : IHttpItemClient
    {

        private readonly HttpClient _httpClient;
        private readonly string _baseUri;
        private static IHttpContextAccessor _accessor;
        private static string _token => _accessor == null ? "" : _accessor.HttpContext?.Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "") ?? "";

        public HttpItemClient(HttpClient httpClient, IConfiguration config, IHttpContextAccessor accessor)
        {
            _httpClient = httpClient;
            _baseUri = config.GetSection("RemoteServices:InventoryService").Value + "/api/item";
            _accessor = accessor;
        }




        public async Task<HttpResponseMessage> GetItems(IEnumerable<int> itemIds = default)
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(_baseUri),
                Content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(itemIds), Encoding.UTF8, "application/json")
            };

            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _token);

            Console.WriteLine($"---> GETTING Items .....");

            return await _httpClient.SendAsync(request);
        }



        public async Task<HttpResponseMessage> GetItemById(int itemId)
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"{_baseUri}/{itemId}")
            };

            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _token);

            Console.WriteLine($"---> GETTING Item '{itemId}' ....");

            return await _httpClient.SendAsync(request);
        }



        public async Task<HttpResponseMessage> AddItem(ItemCreateDTO itemDTO)
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri(_baseUri),
                Content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(itemDTO), Encoding.UTF8, "application/json")
            };

            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _token);

            Console.WriteLine($"---> ADDING Item '{itemDTO.Name}' ....");

            return await _httpClient.SendAsync(request);
        }



        public async Task<HttpResponseMessage> DeleteItem(int itemId)
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Delete,
                RequestUri = new Uri($"{_baseUri}/{itemId}")
            };

            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _token);

            Console.WriteLine($"---> DELETING Item '{itemId}' ....");

            return await _httpClient.SendAsync(request);
        }



        public async Task<HttpResponseMessage> UpdateItem(int itemId, ItemUpdateDTO itemDTO)
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Put,
                RequestUri = new Uri($"{_baseUri}/{itemId}"),
                Content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(itemDTO), Encoding.UTF8, "application/json")
            };

            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _token);

            Console.WriteLine($"---> UPDATING Item '{itemId}': '{itemDTO.Name}' ....");

            return await _httpClient.SendAsync(request);
        }




    }
}
