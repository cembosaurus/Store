using Business.Exceptions.Interfaces;
using Business.Http;
using Business.Http.Interfaces;
using Business.Libraries.ServiceResult.Interfaces;
using Business.Management.Appsettings.Interfaces;
using Business.Management.Services.Interfaces;
using Business.Ordering.DTOs;
using Business.Payment.DTOs;
using Business.Payment.Http.Services.Interfaces;
using Microsoft.AspNetCore.Hosting;



namespace Business.Payment.Http.Services
{
    public class HttpPaymentService : HttpBaseService, IHttpPaymentService
    {

        public HttpPaymentService(IHostingEnvironment env, IExId exId, IAppsettings_PROVIDER appsettingsService, IHttpAppClient httpAppClient, IServiceResultFactory resultFact, IRemoteServices_PROVIDER remoteServices_Provider)
            : base(env, exId, appsettingsService, httpAppClient, remoteServices_Provider, resultFact)
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
