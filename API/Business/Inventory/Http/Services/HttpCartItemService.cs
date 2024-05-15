using Business.Libraries.ServiceResult;
using Business.Libraries.ServiceResult.Interfaces;
using Business.Ordering.DTOs;
using Business.Ordering.Http.Clients.Interfaces;
using Business.Inventory.Http.Services.Interfaces;

namespace Business.Inventory.Http.Services
{
    public class HttpCartItemService : IHttpCartItemService
    {

        private readonly IHttpCartItemClient _httpCartItemClient;
        private readonly IServiceResultFactory _resultFact;

        public HttpCartItemService(IHttpCartItemClient httpCartItemClient, IServiceResultFactory resultFact)
        {
            _httpCartItemClient = httpCartItemClient;
            _resultFact = resultFact;
        }





        public async Task<IServiceResult<IEnumerable<CartItemReadDTO>>> AddItemsToCart(int cartId, IEnumerable<CartItemUpdateDTO> itemsToAdd)
        {
            var response = await _httpCartItemClient.AddItemsToCart(cartId, itemsToAdd);

            if (!response.IsSuccessStatusCode)
                return _resultFact.Result<IEnumerable<CartItemReadDTO>>(null, false, $"{response.ReasonPhrase}: {response.RequestMessage.Method}, {response.RequestMessage.RequestUri}");

            var content = response.Content.ReadAsStringAsync().Result;

            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<ServiceResult<IEnumerable<CartItemReadDTO>>>(content);

            return result;
        }



        public async Task<IServiceResult<IEnumerable<CartItemReadDTO>>> DeleteCartItems(int userId, IEnumerable<int> itemIds)
        {
            var response = await _httpCartItemClient.DeleteCartItems(userId, itemIds);

            if (!response.IsSuccessStatusCode)
                return _resultFact.Result<IEnumerable<CartItemReadDTO>>(null, false, $"{response.ReasonPhrase}: {response.RequestMessage.Method}, {response.RequestMessage.RequestUri}");

            var content = response.Content.ReadAsStringAsync().Result;

            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<ServiceResult<IEnumerable<CartItemReadDTO>>>(content);

            return result;
        }



        public async Task<IServiceResult<IEnumerable<CartItemReadDTO>>> GetAllCardItems()
        {
            var response = await _httpCartItemClient.GetAllCardItems();

            if (!response.IsSuccessStatusCode)
                return _resultFact.Result<IEnumerable<CartItemReadDTO>>(null, false, $"{response.ReasonPhrase}: {response.RequestMessage.Method}, {response.RequestMessage.RequestUri}");

            var content = response.Content.ReadAsStringAsync().Result;

            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<ServiceResult<IEnumerable<CartItemReadDTO>>>(content);

            return result;
        }



        public async Task<IServiceResult<IEnumerable<CartItemReadDTO>>> GetCartItems(int userId)
        {
            var response = await _httpCartItemClient.GetCartItems(userId);

            if (!response.IsSuccessStatusCode)
                return _resultFact.Result<IEnumerable<CartItemReadDTO>>(null, false, $"{response.ReasonPhrase}: {response.RequestMessage.Method}, {response.RequestMessage.RequestUri}");

            var content = response.Content.ReadAsStringAsync().Result;

            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<ServiceResult<IEnumerable<CartItemReadDTO>>>(content);

            return result;
        }



        public async Task<IServiceResult<IEnumerable<CartItemReadDTO>>> RemoveCartItems(int cartId, IEnumerable<CartItemUpdateDTO> itemsToRemove)
        {
            var response = await _httpCartItemClient.RemoveCartItems(cartId, itemsToRemove);

            if (!response.IsSuccessStatusCode)
                return _resultFact.Result<IEnumerable<CartItemReadDTO>>(null, false, $"{response.ReasonPhrase}: {response.RequestMessage.Method}, {response.RequestMessage.RequestUri}");

            var content = response.Content.ReadAsStringAsync().Result;

            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<ServiceResult<IEnumerable<CartItemReadDTO>>>(content);

            return result;
        }
    }
}
