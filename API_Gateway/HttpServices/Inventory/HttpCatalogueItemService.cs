using API_Gateway.HttpServices.Inventory.Interfaces;
using Business.Inventory.DTOs.CatalogueItem;
using Business.Inventory.DTOs.Item;
using Business.Inventory.Http.Interfaces;
using Business.Libraries.ServiceResult;
using Business.Libraries.ServiceResult.Interfaces;

namespace API_Gateway.HttpServices.Inventory
{
    public class HttpCatalogueItemService : IHttpCatalogueItemService
    {

        private readonly IHttpCatalogueItemClient _httpCatalogueItemClient;
        private readonly IServiceResultFactory _resultFact;

        public HttpCatalogueItemService(IHttpCatalogueItemClient httpCatalogueItemClient, IServiceResultFactory resultFact)
        {
            _httpCatalogueItemClient = httpCatalogueItemClient;
            _resultFact = resultFact;
        }





        public async Task<IServiceResult<ExtrasReadDTO>> AddExtrasToCatalogueItem(int id, ExtrasAddDTO extrasAddDTO)
        {
            var response = await _httpCatalogueItemClient.AddExtrasToCatalogueItem(id, extrasAddDTO);

            if (!response.IsSuccessStatusCode)
                return _resultFact.Result<ExtrasReadDTO>(null, false, $"{response.ReasonPhrase}: {response.RequestMessage.Method}, {response.RequestMessage.RequestUri}");

            var content = response.Content.ReadAsStringAsync().Result;

            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<ServiceResult<ExtrasReadDTO>>(content);

            return result;
        }



        public async Task<IServiceResult<int>> AddAmountToStock(int itemId, int amount)
        {
            var response = await _httpCatalogueItemClient.AddAmountToStock(itemId, amount);

            if (!response.IsSuccessStatusCode)
                return _resultFact.Result(0, false, $"{response.ReasonPhrase}: {response.RequestMessage.Method}, {response.RequestMessage.RequestUri}");

            var content = response.Content.ReadAsStringAsync().Result;

            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<ServiceResult<int>>(content);

            return result;
        }



        public async Task<IServiceResult<CatalogueItemReadDTO>> CreateCatalogueItem(int itemId, CatalogueItemCreateDTO catalogueItemCreateDTO)
        {
            var response = await _httpCatalogueItemClient.CreateCatalogueItem(itemId, catalogueItemCreateDTO);

            if (!response.IsSuccessStatusCode)
                return _resultFact.Result<CatalogueItemReadDTO>(null, false, $"{response.ReasonPhrase}: {response.RequestMessage.Method}, {response.RequestMessage.RequestUri}");

            var content = response.Content.ReadAsStringAsync().Result;

            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<ServiceResult<CatalogueItemReadDTO>>(content);

            return result;
        }



        public async Task<IServiceResult<bool>> ExistsCatalogueItemById(int id)
        {
            var response = await _httpCatalogueItemClient.ExistsCatalogueItemById(id);

            if (!response.IsSuccessStatusCode)
                return _resultFact.Result(false, false, $"{response.ReasonPhrase}: {response.RequestMessage.Method}, {response.RequestMessage.RequestUri}");

            var content = response.Content.ReadAsStringAsync().Result;

            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<ServiceResult<bool>>(content);

            return result;
        }



        public async Task<IServiceResult<CatalogueItemReadDTO>> GetCatalogueItemById(int id)
        {
            var response = await _httpCatalogueItemClient.GetCatalogueItemById(id);

            if (!response.IsSuccessStatusCode)
                return _resultFact.Result<CatalogueItemReadDTO>(null, false, $"{response.ReasonPhrase}: {response.RequestMessage.Method}, {response.RequestMessage.RequestUri}");

            var content = response.Content.ReadAsStringAsync().Result;

            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<ServiceResult<CatalogueItemReadDTO>>(content);

            return result;
        }



        public async Task<IServiceResult<IEnumerable<CatalogueItemReadDTO>>> GetCatalogueItems(IEnumerable<int> itemIds = null)
        {
            var response = await _httpCatalogueItemClient.GetCatalogueItems(itemIds);

            if (!response.IsSuccessStatusCode)
                return _resultFact.Result<IEnumerable<CatalogueItemReadDTO>>(null, false, $"{response.ReasonPhrase}: {response.RequestMessage.Method}, {response.RequestMessage.RequestUri}");

            var content = response.Content.ReadAsStringAsync().Result;

            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<ServiceResult<IEnumerable<CatalogueItemReadDTO>>>(content);

            return result;
        }



        public async Task<IServiceResult<CatalogueItemReadDTOWithExtras>> GetCatalogueItemWithExtrasById(int id)
        {
            var response = await _httpCatalogueItemClient.GetCatalogueItemWithExtrasById(id);

            if (!response.IsSuccessStatusCode)
                return _resultFact.Result<CatalogueItemReadDTOWithExtras>(null, false, $"{response.ReasonPhrase}: {response.RequestMessage.Method}, {response.RequestMessage.RequestUri}");

            var content = response.Content.ReadAsStringAsync().Result;

            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<ServiceResult<CatalogueItemReadDTOWithExtras>>(content);

            return result;
        }



        public async Task<IServiceResult<int>> GetInstockCount(int id)
        {
            var response = await _httpCatalogueItemClient.GetInstockCount(id);

            if (!response.IsSuccessStatusCode)
                return _resultFact.Result(0, false, $"{response.ReasonPhrase}: {response.RequestMessage.Method}, {response.RequestMessage.RequestUri}");

            var content = response.Content.ReadAsStringAsync().Result;

            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<ServiceResult<int>>(content);

            return result;
        }



        public async Task<IServiceResult<CatalogueItemReadDTO>> RemoveCatalogueItem(int id)
        {
            var response = await _httpCatalogueItemClient.RemoveCatalogueItem(id);

            if (!response.IsSuccessStatusCode)
                return _resultFact.Result<CatalogueItemReadDTO>(null, false, $"{response.ReasonPhrase}: {response.RequestMessage.Method}, {response.RequestMessage.RequestUri}");

            var content = response.Content.ReadAsStringAsync().Result;

            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<ServiceResult<CatalogueItemReadDTO>>(content);

            return result;
        }



        public async Task<IServiceResult<ExtrasReadDTO>> RemoveExtrasFromCatalogueItem(int id, ExtrasRemoveDTO extrasRemoveDTO)
        {
            var response = await _httpCatalogueItemClient.RemoveExtrasFromCatalogueItem(id, extrasRemoveDTO);

            if (!response.IsSuccessStatusCode)
                return _resultFact.Result<ExtrasReadDTO>(null, false, $"{response.ReasonPhrase}: {response.RequestMessage.Method}, {response.RequestMessage.RequestUri}");

            var content = response.Content.ReadAsStringAsync().Result;

            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<ServiceResult<ExtrasReadDTO>>(content);

            return result;
        }



        public async Task<IServiceResult<int>> RemoveFromStockAmount(int itemId, int amount)
        {
            var response = await _httpCatalogueItemClient.RemoveFromStockAmount(itemId, amount);

            if (!response.IsSuccessStatusCode)
                return _resultFact.Result(0, false, $"{response.ReasonPhrase}: {response.RequestMessage.Method}, {response.RequestMessage.RequestUri}");

            var content = response.Content.ReadAsStringAsync().Result;

            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<ServiceResult<int>>(content);

            return result;
        }



        public async Task<IServiceResult<CatalogueItemReadDTO>> UpdateCatalogueItem(int itemId, CatalogueItemUpdateDTO catalogueItemUpdateDTO)
        {
            var response = await _httpCatalogueItemClient.UpdateCatalogueItem(itemId, catalogueItemUpdateDTO);

            if (!response.IsSuccessStatusCode)
                return _resultFact.Result<CatalogueItemReadDTO>(null, false, $"{response.ReasonPhrase}: {response.RequestMessage.Method}, {response.RequestMessage.RequestUri}");

            var content = response.Content.ReadAsStringAsync().Result;

            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<ServiceResult<CatalogueItemReadDTO>>(content);

            return result;
        }
    }
}
