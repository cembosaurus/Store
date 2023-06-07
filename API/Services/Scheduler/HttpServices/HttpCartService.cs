using Business.Libraries.ServiceResult;
using Business.Libraries.ServiceResult.Interfaces;
using Business.Ordering.Http.Interfaces;
using Business.Scheduler.DTOs;
using Scheduler.HttpServices.Interfaces;

namespace Scheduler.HttpServices
{
    public class HttpCartService : IHttpCartService
    {
        private readonly IHttpCartClient _httpCartClient;
        private readonly IServiceResultFactory _resutlFact;

        public HttpCartService(IHttpCartClient httpCartClient, IServiceResultFactory resutlFact)
        {
            _httpCartClient = httpCartClient;
            _resutlFact = resutlFact;
        }




        public async Task<IServiceResult<bool>> ExistsCartByCartId(Guid cartId)
        {
            var response = await _httpCartClient.ExistsCartByCartId(cartId);

            if (!response.IsSuccessStatusCode)
                return _resutlFact.Result(false, false, response.StatusCode.ToString());

            var content = response.Content.ReadAsStringAsync().Result;

            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<ServiceResult<bool>>(content);

            return result;
        }



        public async Task<IServiceResult<IEnumerable<CartItemsLockReadDTO>>> RemoveExpiredItemsFromCart(IEnumerable<CartItemsLockDeleteDTO> cartItemLocks)
        {
            var response = await _httpCartClient.RemoveExpiredItemsFromCart(cartItemLocks);

            if (!response.IsSuccessStatusCode)
                return _resutlFact.Result<IEnumerable<CartItemsLockReadDTO>>(null, false, response.StatusCode.ToString());

            var content = response.Content.ReadAsStringAsync().Result;

            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<ServiceResult<IEnumerable<CartItemsLockReadDTO>>>(content);

            return result;
        }

    }
}
