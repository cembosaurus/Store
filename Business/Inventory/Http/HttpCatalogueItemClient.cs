using Business.Inventory.DTOs.CatalogueItem;
using Business.Inventory.Http.Interfaces;
using Business.Libraries.Http.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Net.Http.Headers;
using System.Net.Http.Headers;
using System.Text;

namespace Business.Inventory.Http
{
    public class HttpCatalogueItemClient : IHttpCatalogueItemClient
    {

        private readonly HttpClient _httpClient;
        private readonly string _baseUri;
        private static IHttpContextAccessor _accessor;
        private static string _token => _accessor == null ? "" : _accessor.HttpContext?.Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "") ?? "";

        public HttpCatalogueItemClient(HttpClient httpClient, IConfiguration config, IHttpContextAccessor accessor) 
        {
            _httpClient = httpClient;
            _baseUri = config.GetSection("RemoteServices:InventoryService").Value + "/api/catalogueitem";
            _accessor = accessor;
        }





        public async Task<HttpResponseMessage> GetCatalogueItems(IEnumerable<int> itemIds = default)
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(_baseUri + $"{(itemIds != null && itemIds.Any() ? "" : "/all")}"),
                Content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(itemIds), Encoding.UTF8, "application/json")
            };

            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _token);

            Console.WriteLine($"---> GETTING Catalogue items ....");

            return await _httpClient.SendAsync(request);
        }



        public async Task<HttpResponseMessage> GetCatalogueItemById(int itemId)
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"{_baseUri}/{itemId}")
            };

            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _token);

            Console.WriteLine($"---> GETTING Catalogue item '{itemId}' ....");

            return await _httpClient.SendAsync(request);
        }



        public async Task<HttpResponseMessage> ExistsCatalogueItemById(int itemId)
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"{_baseUri}/{itemId}/exists")
            };

            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _token);

            Console.WriteLine($"---> EXISTS Catalogue item '{itemId}' ....");

            return await _httpClient.SendAsync(request);
        }



        public async Task<HttpResponseMessage> CreateCatalogueItem(int itemId, CatalogueItemCreateDTO catalogueItemCreateDTO)
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri($"{_baseUri}/{itemId}"),
                Content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(catalogueItemCreateDTO), Encoding.UTF8, "application/json")
            };

            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _token);

            Console.WriteLine($"---> CREATING Catalogue item '{itemId}' ....");

            return await _httpClient.SendAsync(request);
        }



        public async Task<HttpResponseMessage> UpdateCatalogueItem(int itemId, CatalogueItemUpdateDTO catalogueItemUpdateDTO)
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri($"{_baseUri}/{itemId}"),
                Content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(catalogueItemUpdateDTO), Encoding.UTF8, "application/json")
            };

            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _token);

            Console.WriteLine($"---> UPDATING Catalogue item '{itemId}' ....");

            return await _httpClient.SendAsync(request);
        }



        public async Task<HttpResponseMessage> RemoveCatalogueItem(int itemId)
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Delete,
                RequestUri = new Uri($"{_baseUri}/{itemId}")
            };

            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _token);

            Console.WriteLine($"---> REMOVING Catalogue item '{itemId}' ....");

            return await _httpClient.SendAsync(request);
        }



        public async Task<HttpResponseMessage> GetCatalogueItemWithExtrasById(int itemId)
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"{_baseUri}/{itemId}/extras")
            };

            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _token);

            Console.WriteLine($"---> GETTING Catalogue item with extras '{itemId}' with extras ....");

            return await _httpClient.SendAsync(request);
        }



        public async Task<HttpResponseMessage> AddExtrasToCatalogueItem(int itemId, ExtrasAddDTO extrasAddDTO)
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri($"{_baseUri}/{itemId}/extras"),
                Content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(extrasAddDTO), Encoding.UTF8, "application/json")
            };

            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _token);

            Console.WriteLine($"---> ADDING Catalogue item '{itemId}' extras ....");

            return await _httpClient.SendAsync(request);
        }



        public async Task<HttpResponseMessage> RemoveExtrasFromCatalogueItem(int itemId, ExtrasRemoveDTO extrasRemoveDTO)
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Delete,
                RequestUri = new Uri($"{_baseUri}/{itemId}/extras"),
                Content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(extrasRemoveDTO), Encoding.UTF8, "application/json")
            };

            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _token);

            Console.WriteLine($"---> REMOVING Catalogue item '{itemId}' extras ....");

            return await _httpClient.SendAsync(request);
        }



        public async Task<HttpResponseMessage> GetInstockCount(int itemId)
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"{_baseUri}/{itemId}/instock")
            };

            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _token);

            Console.WriteLine($"---> GETTING Instock amount for catalogue item '{itemId}' ....");

            return await _httpClient.SendAsync(request);

        }



        public async Task<HttpResponseMessage> RemoveFromStockAmount(int itemId, int amount)
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Put,
                RequestUri = new Uri($"{_baseUri}/{itemId}/fromstock/{amount}")
            };

            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _token);

            Console.WriteLine($"---> REMOVING amount '{amount}' from stock for catalogue item '{itemId}' ....");

            return await _httpClient.SendAsync(request);
        }



        public async Task<HttpResponseMessage> AddAmountToStock(int itemId, int amount)
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Put,
                RequestUri = new Uri($"{_baseUri}/{itemId}/tostock/{amount}")
            };

            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _token);

            Console.WriteLine($"---> ADDING amount '{amount}' to stock for catalogue item '{itemId}' ....");

            return await _httpClient.SendAsync(request);
        }


    }
}
