using API_Gateway.HttpServices.Inventory.Interfaces;
using Business.Inventory.DTOs.Item;
using Business.Inventory.Http.Interfaces;
using Business.Libraries.ServiceResult;
using Business.Libraries.ServiceResult.Interfaces;

namespace API_Gateway.HttpServices.Inventory
{
    public class HttpItemService : IHttpItemService
    {
        private readonly IHttpItemClient _httpItemClient;
        private readonly IServiceResultFactory _resultFact;

        public HttpItemService(IHttpItemClient httpItemClient, IServiceResultFactory resultFact)
        {
            _httpItemClient = httpItemClient;
            _resultFact = resultFact;
        }





        public async Task<IServiceResult<IEnumerable<ItemReadDTO>>> GetItems(IEnumerable<int> itemIds = default)
        {
            var response = await _httpItemClient.GetItems(itemIds);

            if (!response.IsSuccessStatusCode)
                return _resultFact.Result<IEnumerable<ItemReadDTO>>(null, false, $"{response.ReasonPhrase}: {response.RequestMessage.Method}, {response.RequestMessage.RequestUri}");

            var content = response.Content.ReadAsStringAsync().Result;

            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<ServiceResult<IEnumerable<ItemReadDTO>>>(content);

            return result;
        }



        public async Task<IServiceResult<ItemReadDTO>> GetItemById(int id)
        {
            var response = await _httpItemClient.GetItemById(id);

            if (!response.IsSuccessStatusCode)
                return _resultFact.Result<ItemReadDTO>(null, false, $"{response.ReasonPhrase}: {response.RequestMessage.Method}, {response.RequestMessage.RequestUri}");

            var content = response.Content.ReadAsStringAsync().Result;

            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<ServiceResult<ItemReadDTO>>(content);

            return result;
        }



        public async Task<IServiceResult<ItemReadDTO>> AddItem(ItemCreateDTO item)
        {
            var response = await _httpItemClient.AddItem(item);

            if (!response.IsSuccessStatusCode)
                return _resultFact.Result<ItemReadDTO>(null, false, $"{response.ReasonPhrase}: {response.RequestMessage.Method}, {response.RequestMessage.RequestUri}");

            var content = response.Content.ReadAsStringAsync().Result;

            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<ServiceResult<ItemReadDTO>>(content);

            return result;
        }



        public async Task<IServiceResult<ItemReadDTO>> DeleteItem(int id)
        {
            var response = await _httpItemClient.DeleteItem(id);

            if (!response.IsSuccessStatusCode)
                return _resultFact.Result<ItemReadDTO>(null, false, $"{response.ReasonPhrase}: {response.RequestMessage.Method}, {response.RequestMessage.RequestUri}");

            var content = response.Content.ReadAsStringAsync().Result;

            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<ServiceResult<ItemReadDTO>>(content);

            return result;
        }



        public async Task<IServiceResult<ItemReadDTO>> UpdateItem(int id, ItemUpdateDTO item)
        {
            var response = await _httpItemClient.UpdateItem(id, item);

            if (!response.IsSuccessStatusCode)
                return _resultFact.Result<ItemReadDTO>(null, false, $"{response.ReasonPhrase}: {response.RequestMessage.Method}, {response.RequestMessage.RequestUri}");

            var content = response.Content.ReadAsStringAsync().Result;

            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<ServiceResult<ItemReadDTO>>(content);

            return result;
        }
    }
}
