using Business.Exceptions.Interfaces;
using Business.Http.Clients;
using Business.Http.Services;
using Business.Libraries.ServiceResult.Interfaces;
using Business.Management.Services.Interfaces;
using Business.Ordering.DTOs;
using Business.Payment.DTOs;
using Business.Payment.Http.Services.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace Business.Payment.Http.Services
{
    public class HttpPaymentService : HttpBaseService, IHttpPaymentService
    {

        public HttpPaymentService(IHttpContextAccessor accessor, IWebHostEnvironment env, IExId exId, IHttpAppClient httpAppClient, IGlobalConfig_PROVIDER remoteServices_Provider, IServiceResultFactory resultFact)
            : base(accessor, env, exId, httpAppClient, remoteServices_Provider, resultFact)
        {
            _remoteServiceName = "PaymentService";
            _remoteServicePathName = "Payment";
        }




        public async Task<IServiceResult<OrderReadDTO>> MakePayment(OrderPaymentCreateDTO order)
        {
            _method = HttpMethod.Post;
            _requestQuery = $"";
            _content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(order), _encoding, _mediaType);

            return await HTTP_Request_Handler<OrderReadDTO>();
        }

    }
}
