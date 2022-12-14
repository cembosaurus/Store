using Business.Inventory.DTOs.ItemPrice;
using Business.Inventory.Http.Interfaces;
using Business.Libraries.Http.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Net.Http.Headers;
using System.Net.Http.Headers;
using System.Text;

namespace Business.Inventory.Http
{
    public class HttpItemPriceClient : IHttpItemPriceClient
    {

        private readonly HttpClient _httpClient;
        private readonly string _baseUri;
        private static IHttpContextAccessor _accessor;
        private static string _token => _accessor == null ? "" : _accessor.HttpContext?.Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "") ?? "";

        public HttpItemPriceClient(HttpClient httpClient, IConfiguration config, IHttpContextAccessor accessor)
        {
            _httpClient = httpClient;
            _baseUri = config.GetSection("RemoteServices:InventoryService").Value + "/api/itemprice";
            _accessor = accessor;
        }





        public async Task<HttpResponseMessage> GetItemPrices(IEnumerable<int> itemIds = default)
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(_baseUri),
                Content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(itemIds), Encoding.UTF8, "application/json")
            };
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _token);

            Console.WriteLine("---> GETTING Item prices ....");

            return await _httpClient.SendAsync(request);
        }



        public async Task<HttpResponseMessage> GetItemPriceById(int itemId)
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(_baseUri + $"/{itemId}"),
            };
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _token);

            Console.WriteLine($"---> GETTING Item price '{itemId}' ....");

            return await _httpClient.SendAsync(request);
        }




        public async Task<HttpResponseMessage> GetItemPricesByIds(IEnumerable<int> itemIds)
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(_baseUri + "/foritems"),
                Content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(itemIds), Encoding.UTF8, "application/json")
            };
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _token);

            Console.WriteLine($"---> GETTING Item prices ....");

            return await _httpClient.SendAsync(request);
        }




        public async Task<HttpResponseMessage> EditItemPrice(int itemId, ItemPriceUpdateDTO itemPriceUpdateDTO)
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Put,
                RequestUri = new Uri(_baseUri + $"/{itemId}"),
                Content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(itemPriceUpdateDTO), Encoding.UTF8, "application/json")
            };
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _token);

            Console.WriteLine($"---> UPDATING Item price '{itemId}' ....");

            return await _httpClient.SendAsync(request);
        }




    }
}
