using Business.Libraries.ServiceResult;
using Business.Libraries.ServiceResult.Interfaces;
using Business.Ordering.DTOs;
using Business.Ordering.Http.Interfaces;
using Identity.HttpServices.Interfaces;

namespace Identity.HttpServices
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





        public async Task<IServiceResult<CartReadDTO>> CreateCart(int userId)
        {
            var response = await _httpCartClient.CreateCart(userId);

            if (!response.IsSuccessStatusCode)
                return _resutlFact.Result<CartReadDTO>(null, false, response.StatusCode.ToString());

            var content = response.Content.ReadAsStringAsync().Result;

            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<ServiceResult<CartReadDTO>>(content);

            return result;
        }



    }
}
