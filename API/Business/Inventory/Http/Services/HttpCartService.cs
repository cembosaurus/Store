using Business.Libraries.ServiceResult;
using Business.Libraries.ServiceResult.Interfaces;
using Business.Ordering.DTOs;
using Business.Ordering.Http.Clients.Interfaces;
using Business.Scheduler.DTOs;
using Business.Inventory.Http.Services.Interfaces;

namespace Business.Inventory.Http.Services
{
    public class HttpCartService : IHttpCartService
    {

        private readonly IHttpCartClient _httpCartClient;
        private readonly IServiceResultFactory _resultFact;

        public HttpCartService(IHttpCartClient httpCartClient, IServiceResultFactory resultFact)
        {
            _httpCartClient = httpCartClient;
            _resultFact = resultFact;
        }




        public async Task<IServiceResult<CartReadDTO>> CreateCart(int userId)
        {
            var response = await _httpCartClient.CreateCart(userId);

            if (!response.IsSuccessStatusCode)
                return _resultFact.Result<CartReadDTO>(null, false, $"{response.ReasonPhrase}: {response.RequestMessage.Method}, {response.RequestMessage.RequestUri}");

            var content = response.Content.ReadAsStringAsync().Result;

            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<ServiceResult<CartReadDTO>>(content);

            return result;
        }

        public async Task<IServiceResult<CartReadDTO>> DeleteCart(int id)
        {
            var response = await _httpCartClient.DeleteCart(id);

            if (!response.IsSuccessStatusCode)
                return _resultFact.Result<CartReadDTO>(null, false, $"{response.ReasonPhrase}: {response.RequestMessage.Method}, {response.RequestMessage.RequestUri}");

            var content = response.Content.ReadAsStringAsync().Result;

            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<ServiceResult<CartReadDTO>>(content);

            return result;
        }

        public async Task<IServiceResult<bool>> ExistsCartByCartId(Guid cartId)
        {
            var response = await _httpCartClient.ExistsCartByCartId(cartId);

            if (!response.IsSuccessStatusCode)
                return _resultFact.Result(false, false, $"{response.ReasonPhrase}: {response.RequestMessage.Method}, {response.RequestMessage.RequestUri}");

            var content = response.Content.ReadAsStringAsync().Result;

            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<ServiceResult<bool>>(content);

            return result;
        }

        public async Task<IServiceResult<IEnumerable<CartReadDTO>>> GetCards()
        {
            var response = await _httpCartClient.GetCards();

            if (!response.IsSuccessStatusCode)
                return _resultFact.Result<IEnumerable<CartReadDTO>>(null, false, $"{response.ReasonPhrase}: {response.RequestMessage.Method}, {response.RequestMessage.RequestUri}");

            var content = response.Content.ReadAsStringAsync().Result;

            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<ServiceResult<IEnumerable<CartReadDTO>>>(content);

            return result;
        }

        public async Task<IServiceResult<CartReadDTO>> GetCartByUserId(int userId)
        {
            var response = await _httpCartClient.GetCartByUserId(userId);

            if (!response.IsSuccessStatusCode)
                return _resultFact.Result<CartReadDTO>(null, false, $"{response.ReasonPhrase}: {response.RequestMessage.Method}, {response.RequestMessage.RequestUri}");

            var content = response.Content.ReadAsStringAsync().Result;

            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<ServiceResult<CartReadDTO>>(content);

            return result;
        }

        public async Task<IServiceResult<CartReadDTO>> UpdateCart(int userId, CartUpdateDTO cartUpdateDTO)
        {
            var response = await _httpCartClient.UpdateCart(userId, cartUpdateDTO);

            if (!response.IsSuccessStatusCode)
                return _resultFact.Result<CartReadDTO>(null, false, $"{response.ReasonPhrase}: {response.RequestMessage.Method}, {response.RequestMessage.RequestUri}");

            var content = response.Content.ReadAsStringAsync().Result;

            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<ServiceResult<CartReadDTO>>(content);

            return result;
        }


        public async Task<IServiceResult<IEnumerable<CartItemsLockReadDTO>>> RemoveExpiredItemsFromCart(IEnumerable<CartItemsLockDeleteDTO> cartItemLocks)
        {
            var response = await _httpCartClient.RemoveExpiredItemsFromCart(cartItemLocks);

            if (!response.IsSuccessStatusCode)
                return _resultFact.Result<IEnumerable<CartItemsLockReadDTO>>(null, false, response.StatusCode.ToString());

            var content = response.Content.ReadAsStringAsync().Result;

            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<ServiceResult<IEnumerable<CartItemsLockReadDTO>>>(content);

            return result;
        }
    }
}
