using Business.Libraries.ServiceResult;
using Business.Libraries.ServiceResult.Interfaces;
using Business.Ordering.DTOs;
using Business.Payment.DTOs;
using Business.Payment.Http.Interfaces;
using Ordering.HttpServices.Interfaces;

namespace Ordering.HttpServices
{
    public class HttpPaymentService : IHttpPaymentService
    {

        private readonly IHttpPaymentClient _httpPaymentClient;
        private readonly IServiceResultFactory _resultFact;

        public HttpPaymentService(IHttpPaymentClient httpPaymentClient, IServiceResultFactory resultFact)
        {
            _httpPaymentClient = httpPaymentClient;
            _resultFact = resultFact;
        }




        public async Task<IServiceResult<OrderReadDTO>> MakePayment(OrderPaymentCreateDTO order)
        {
            var response = await _httpPaymentClient.MakePayment(order);

            if (!response.IsSuccessStatusCode)
                return _resultFact.Result<OrderReadDTO>(null, false, response.StatusCode.ToString());

            var content = response.Content.ReadAsStringAsync().Result;

            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<ServiceResult<OrderReadDTO>>(content);

            return result;
        }

    }
}
