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

        private readonly HttpRequestMessage _request;
        private readonly HttpClient _httpClient;
        private readonly string _baseUri;
        private readonly Encoding _encoding = Encoding.UTF8;
        private readonly string _mediaType = "application/json";

        private static IHttpContextAccessor _accessor;
        private static string _token => _accessor == null ? "" : _accessor.HttpContext?.Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "") ?? "";

        public HttpItemPriceClient(HttpClient httpClient, IConfiguration config, IHttpContextAccessor accessor)
        {
            _httpClient = httpClient;
            _baseUri = config.GetSection("RemoteServices:InventoryService").Value + "/api/itemprice";
            _accessor = accessor;
            _request = new HttpRequestMessage { RequestUri = new Uri(_baseUri) };
            _request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(_mediaType));
            _request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _token);
        }





        public async Task<HttpResponseMessage> GetItemPrices(IEnumerable<int> itemIds)
        {
            _request.Method = HttpMethod.Get;
            _request.RequestUri = new Uri(_baseUri + $"{(itemIds != null && itemIds.Any() ? "" : "/all")}");
            _request.Content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(itemIds), _encoding, _mediaType);

            Console.WriteLine("---> GETTING Item prices ....");

            return await _httpClient.SendAsync(_request);
        }



        public async Task<HttpResponseMessage> GetItemPriceById(int itemId)
        {
            _request.Method = HttpMethod.Get;
            _request.RequestUri = new Uri(_baseUri + $"/{itemId}");

            Console.WriteLine($"---> GETTING Item price '{itemId}' ....");

            return await _httpClient.SendAsync(_request);
        }



        public async Task<HttpResponseMessage> UpdateItemPrice(int itemId, ItemPriceUpdateDTO itemPriceUpdateDTO)
        {
            _request.Method = HttpMethod.Put;
            _request.RequestUri = new Uri(_baseUri + $"/{itemId}");
            _request.Content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(itemPriceUpdateDTO), _encoding, _mediaType);

            Console.WriteLine($"---> UPDATING Item price '{itemId}' ....");

            return await _httpClient.SendAsync(_request);
        }




    }
}
