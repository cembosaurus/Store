using Business.Inventory.DTOs.CatalogueItem;
using Business.Inventory.DTOs.Item;
using Business.Inventory.DTOs.ItemPrice;
using Business.Inventory.Http.Clients.Interfaces;
using Business.Libraries.ServiceResult;
using Business.Libraries.ServiceResult.Interfaces;
using Ordering.HttpServices.Interfaces;

namespace Ordering.HttpServices
{
    public class HttpInventoryService : IHttpInventoryService
    {

        private readonly IHttpItemPriceClient _httpItemPriceClient;
        private readonly IHttpCatalogueItemClient _httpCatalogueItemClient;
        private readonly IHttpItemClient _httpItemClient;
        private readonly IServiceResultFactory _resutlFact;

        public HttpInventoryService(IHttpItemPriceClient httpItemPriceClient, IHttpCatalogueItemClient httpCatalogueItemClient, IHttpItemClient httpItemClient, IServiceResultFactory resutlFact)
        {
            _httpItemPriceClient = httpItemPriceClient;
            _httpCatalogueItemClient = httpCatalogueItemClient;
            _httpItemClient = httpItemClient;
            _resutlFact = resutlFact;
        }






        public async Task<IServiceResult<IEnumerable<ItemReadDTO>>> GetItems(IEnumerable<int> itemIds = default)
        {
            var response = await _httpItemClient.GetItems(itemIds);

            if (!response.IsSuccessStatusCode)
                return _resutlFact.Result<IEnumerable<ItemReadDTO>>(null, false, response.StatusCode.ToString());

            var content = response.Content.ReadAsStringAsync().Result;

            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<ServiceResult<IEnumerable<ItemReadDTO>>>(content);

            return result;
        }



        public async Task<IServiceResult<ItemReadDTO>> GetItemById(int itemId)
        {
            var response = await _httpItemClient.GetItemById(itemId);

            if (!response.IsSuccessStatusCode)
                return _resutlFact.Result<ItemReadDTO>(null, false, response.StatusCode.ToString());

            var content = response.Content.ReadAsStringAsync().Result;

            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<ServiceResult<ItemReadDTO>>(content);

            return result;
        }



        public async Task<IServiceResult<IEnumerable<CatalogueItemReadDTO>>> GetCatalogueItems(IEnumerable<int> itemIds = default)
        {
            var response = await _httpCatalogueItemClient.GetCatalogueItems(itemIds);

            if (!response.IsSuccessStatusCode)
                return _resutlFact.Result<IEnumerable<CatalogueItemReadDTO>>(null, false, response.StatusCode.ToString());

            var content = response.Content.ReadAsStringAsync().Result;

            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<ServiceResult<IEnumerable<CatalogueItemReadDTO>>>(content);

            return result;
        }



        public async Task<IServiceResult<CatalogueItemReadDTO>> GetCatalogueItemById(int itemId)
        {
            var response = await _httpCatalogueItemClient.GetCatalogueItemById(itemId);

            if (!response.IsSuccessStatusCode)
                return _resutlFact.Result<CatalogueItemReadDTO>(null, false, response.StatusCode.ToString());

            var content = response.Content.ReadAsStringAsync().Result;

            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<ServiceResult<CatalogueItemReadDTO>>(content);

            return result;
        }



        public async Task<IServiceResult<IEnumerable<ItemPriceReadDTO>>> GetItemPrices(IEnumerable<int> itemIds = default)
        {
            var response = await _httpItemPriceClient.GetItemPrices(itemIds);

            if (!response.IsSuccessStatusCode)
                return _resutlFact.Result<IEnumerable<ItemPriceReadDTO>>(null, false, response.StatusCode.ToString());

            var content = response.Content.ReadAsStringAsync().Result;

            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<ServiceResult<IEnumerable<ItemPriceReadDTO>>>(content);

            return result;
        }


        public async Task<IServiceResult<ItemPriceReadDTO>> GetItemPriceById(int itemId)
        {
            var response = await _httpItemPriceClient.GetItemPriceById(itemId);

            if (!response.IsSuccessStatusCode)
                return _resutlFact.Result<ItemPriceReadDTO>(null, false, response.StatusCode.ToString());

            var content = response.Content.ReadAsStringAsync().Result;

            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<ServiceResult<ItemPriceReadDTO>>(content);

            return result;
        }



        public async Task<IServiceResult<int>> GetInstockCount(int itemId)
        {
            var response =  await _httpCatalogueItemClient.GetInstockCount(itemId);

            if (!response.IsSuccessStatusCode)
                return _resutlFact.Result(0, false, response.StatusCode.ToString());

            var content = response.Content.ReadAsStringAsync().Result;

            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<ServiceResult<int>>(content);

            return result;
        }



        public async Task<IServiceResult<int>> AddAmountToStock(int itemId, int amount)
        {
            var response = await _httpCatalogueItemClient.AddAmountToStock(itemId, amount);

            if (!response.IsSuccessStatusCode)
                return _resutlFact.Result(0, false, response.StatusCode.ToString());

            var content = response.Content.ReadAsStringAsync().Result;

            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<ServiceResult<int>>(content);

            return result;
        }



        public async Task<IServiceResult<int>> RemoveAmountFromStock(int itemId, int amount)
        {
            var response = await _httpCatalogueItemClient.RemoveFromStockAmount(itemId, amount);

            if (!response.IsSuccessStatusCode)
                return _resutlFact.Result(0, false, response.StatusCode.ToString());

            var content = response.Content.ReadAsStringAsync().Result;

            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<ServiceResult<int>>(content);

            return result;
        }



    }
}
