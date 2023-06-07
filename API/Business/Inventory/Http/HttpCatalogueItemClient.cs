using Business.Inventory.DTOs.CatalogueItem;
using Business.Inventory.Http.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace Business.Inventory.Http
{
    public class HttpCatalogueItemClient : IHttpCatalogueItemClient
    {

        private readonly HttpRequestMessage _request;
        private readonly HttpClient _httpClient;
        private readonly string _baseUri;
        private readonly Encoding _encoding = Encoding.UTF8;
        private readonly string _mediaType = "application/json";

        private static IHttpContextAccessor _accessor;
        private static string _token => _accessor == null ? "" : _accessor.HttpContext?.Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "") ?? "";


        public HttpCatalogueItemClient(HttpClient httpClient, IConfiguration config, IHttpContextAccessor accessor) 
        {
            _httpClient = httpClient;
            _baseUri = config.GetSection("RemoteServices:InventoryService").Value + "/api/catalogueitem";
            _accessor = accessor;
            _request = new HttpRequestMessage { RequestUri = new Uri(_baseUri) };
            _request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(_mediaType));
            _request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _token);
        }




        public async Task<HttpResponseMessage> GetCatalogueItems(IEnumerable<int> itemIds = default)
        {

            _request.Method = HttpMethod.Get;
            _request.RequestUri = new Uri(_request.RequestUri + $"{(itemIds != null && itemIds.Any() ? "" : "/all")}");
            _request.Content = new StringContent(JsonConvert.SerializeObject(itemIds), _encoding, _mediaType);           

            Console.WriteLine($"---> GETTING Catalogue items ....");

            return await _httpClient.SendAsync(_request);
        }



        public async Task<HttpResponseMessage> GetCatalogueItemById(int itemId)
        {
            _request.Method = HttpMethod.Get;
            _request.RequestUri = new Uri(_request.RequestUri + $"/{itemId}");

            Console.WriteLine($"---> GETTING Catalogue item '{itemId}' ....");

            return await _httpClient.SendAsync(_request);
        }



        public async Task<HttpResponseMessage> ExistsCatalogueItemById(int itemId)
        {
            _request.Method = HttpMethod.Get;
            _request.RequestUri = new Uri(_request.RequestUri + $"/{itemId}/exists");

            Console.WriteLine($"---> EXISTS Catalogue item '{itemId}' ....");

            return await _httpClient.SendAsync(_request);
        }



        public async Task<HttpResponseMessage> CreateCatalogueItem(int itemId, CatalogueItemCreateDTO catalogueItemCreateDTO)
        {
            _request.Method = HttpMethod.Post;
            _request.RequestUri = new Uri(_request.RequestUri +  $"/{itemId}");
            _request.Content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(catalogueItemCreateDTO), _encoding, _mediaType);

            Console.WriteLine($"---> CREATING Catalogue item '{itemId}' ....");

            return await _httpClient.SendAsync(_request);
        }



        public async Task<HttpResponseMessage> UpdateCatalogueItem(int itemId, CatalogueItemUpdateDTO catalogueItemUpdateDTO)
        {
            _request.Method = HttpMethod.Post;
            _request.RequestUri = new Uri(_request.RequestUri + $"/{itemId}");
            _request.Content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(catalogueItemUpdateDTO), _encoding, _mediaType);

            Console.WriteLine($"---> UPDATING Catalogue item '{itemId}' ....");

            return await _httpClient.SendAsync(_request);
        }



        public async Task<HttpResponseMessage> RemoveCatalogueItem(int itemId)
        {
            _request.Method = HttpMethod.Delete;
            _request.RequestUri = new Uri(_request.RequestUri + $"/{itemId}");

            Console.WriteLine($"---> REMOVING Catalogue item '{itemId}' ....");

            return await _httpClient.SendAsync(_request);
        }



        public async Task<HttpResponseMessage> GetCatalogueItemWithExtrasById(int itemId)
        {
            _request.Method = HttpMethod.Get;
            _request.RequestUri = new Uri(_request.RequestUri + $"/{itemId}/extras");

            Console.WriteLine($"---> GETTING Catalogue item with extras '{itemId}' with extras ....");

            return await _httpClient.SendAsync(_request);
        }



        public async Task<HttpResponseMessage> AddExtrasToCatalogueItem(int itemId, ExtrasAddDTO extrasAddDTO)
        {
            _request.Method = HttpMethod.Post;
            _request.RequestUri = new Uri(_request.RequestUri + $"/{itemId}/extras");
            _request.Content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(extrasAddDTO), _encoding, _mediaType);

            Console.WriteLine($"---> ADDING Catalogue item '{itemId}' extras ....");

            return await _httpClient.SendAsync(_request);
        }



        public async Task<HttpResponseMessage> RemoveExtrasFromCatalogueItem(int itemId, ExtrasRemoveDTO extrasRemoveDTO)
        {
            _request.Method = HttpMethod.Delete;
            _request.RequestUri = new Uri(_request.RequestUri + $"/{itemId}/extras");
            _request.Content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(extrasRemoveDTO), _encoding, _mediaType);

            Console.WriteLine($"---> REMOVING Catalogue item '{itemId}' extras ....");

            return await _httpClient.SendAsync(_request);
        }



        public async Task<HttpResponseMessage> GetInstockCount(int itemId)
        {
            _request.Method = HttpMethod.Get;
            _request.RequestUri = new Uri(_request.RequestUri + $"/{itemId}/instock");

            Console.WriteLine($"---> GETTING Instock amount for catalogue item '{itemId}' ....");

            return await _httpClient.SendAsync(_request);

        }



        public async Task<HttpResponseMessage> RemoveFromStockAmount(int itemId, int amount)
        {
            _request.Method = HttpMethod.Put;
            _request.RequestUri = new Uri(_request.RequestUri + $"/{itemId}/fromstock/{amount}");

            Console.WriteLine($"---> REMOVING amount '{amount}' from stock for catalogue item '{itemId}' ....");

            return await _httpClient.SendAsync(_request);
        }



        public async Task<HttpResponseMessage> AddAmountToStock(int itemId, int amount)
        {
            _request.Method = HttpMethod.Put;
            _request.RequestUri = new Uri(_request.RequestUri + $"/{itemId}/tostock/{amount}");

            Console.WriteLine($"---> ADDING amount '{amount}' to stock for catalogue item '{itemId}' ....");

            return await _httpClient.SendAsync(_request);
        }


    }
}
