using Business.Inventory.DTOs.ItemPrice;
using Business.Inventory.Http.Clients.Interfaces;
using Business.Inventory.Http.Services.Interfaces;
using Business.Libraries.ServiceResult;
using Business.Libraries.ServiceResult.Interfaces;



namespace Business.Inventory.Http.Services
{
    public class HttpItemPriceService : IHttpItemPriceService
    {

        private readonly IHttpItemPriceClient _httpItemPriceClient;
        private readonly IServiceResultFactory _resultFact;

        public HttpItemPriceService(IHttpItemPriceClient httpItemPriceClient, IServiceResultFactory resultFact)
        {
            _httpItemPriceClient = httpItemPriceClient;
            _resultFact = resultFact;
        }




        public async Task<IServiceResult<ItemPriceReadDTO>> GetItemPriceById(int id)
        {
            var response = await _httpItemPriceClient.GetItemPriceById(id);

            if (!response.IsSuccessStatusCode)
                return _resultFact.Result<ItemPriceReadDTO>(null, false, $"{response.ReasonPhrase}: {response.RequestMessage.Method}, {response.RequestMessage.RequestUri}");

            var content = response.Content.ReadAsStringAsync().Result;

            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<ServiceResult<ItemPriceReadDTO>>(content);

            return result;
        }



        public async Task<IServiceResult<IEnumerable<ItemPriceReadDTO>>> GetItemPrices(IEnumerable<int> itemIds = default)
        {
            var response = await _httpItemPriceClient.GetItemPrices(itemIds);

            if (!response.IsSuccessStatusCode)
                return _resultFact.Result<IEnumerable<ItemPriceReadDTO>>(null, false, $"{response.ReasonPhrase}: {response.RequestMessage.Method}, {response.RequestMessage.RequestUri}");

            var content = response.Content.ReadAsStringAsync().Result;

            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<ServiceResult<IEnumerable<ItemPriceReadDTO>>>(content);

            return result;
        }



        public async Task<IServiceResult<ItemPriceReadDTO>> UpdateItemPrice(int itemId, ItemPriceUpdateDTO itemPriceEditDTO)
        {
            var response = await _httpItemPriceClient.UpdateItemPrice(itemId, itemPriceEditDTO);

            if (!response.IsSuccessStatusCode)
                return _resultFact.Result<ItemPriceReadDTO>(null, false, $"{response.ReasonPhrase}: {response.RequestMessage.Method}, {response.RequestMessage.RequestUri}");

            var content = response.Content.ReadAsStringAsync().Result;

            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<ServiceResult<ItemPriceReadDTO>>(content);

            return result;
        }
    }
}
