using Business.Libraries.ServiceResult;
using Business.Libraries.ServiceResult.Interfaces;
using Business.Scheduler.DTOs;
using Business.Scheduler.Http.Interfaces;
using Ordering.HttpServices.Interfaces;

namespace Ordering.HttpServices
{
    public class HttpSchedulerService : IHttpSchedulerService
    {

        private readonly IHttpSchedulerClient _httpSchedulerClient;
        private readonly IServiceResultFactory _resutlFact;

        public HttpSchedulerService(IHttpSchedulerClient httpSchedulerClient, IServiceResultFactory resutlFact)
        {
            _httpSchedulerClient  = httpSchedulerClient;
            _resutlFact = resutlFact;
        }





        public async Task<IServiceResult<CartItemsLockReadDTO>> CartItemsLock(CartItemsLockCreateDTO cartItemsToLock)
        {
            var response = await _httpSchedulerClient.CartItemsLock(cartItemsToLock);

            if (!response.IsSuccessStatusCode)
                return _resutlFact.Result<CartItemsLockReadDTO>(null, false, response.StatusCode.ToString());

            var content = response.Content.ReadAsStringAsync().Result;

            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<ServiceResult<CartItemsLockReadDTO>>(content);

            return result;
        }



        public async Task<IServiceResult<CartItemsLockReadDTO>> CartItemsUnLock(CartItemsLockDeleteDTO cartItemsToUnLock)
        {
            var response = await _httpSchedulerClient.CartItemsUnLock(cartItemsToUnLock);

            if (!response.IsSuccessStatusCode)
                return _resutlFact.Result<CartItemsLockReadDTO>(null, false, response.StatusCode.ToString());

            var content = response.Content.ReadAsStringAsync().Result;

            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<ServiceResult<CartItemsLockReadDTO>>(content);

            return result;
        }


    }
}
