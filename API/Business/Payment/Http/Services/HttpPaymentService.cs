﻿using Business.Exceptions.Interfaces;
using Business.Http.Clients.Interfaces;
using Business.Http.Services;
using Business.Http.Services.Interfaces;
using Business.Libraries.ServiceResult.Interfaces;
using Business.Management.Services.Interfaces;
using Business.Ordering.DTOs;
using Business.Payment.DTOs;
using Business.Payment.Http.Services.Interfaces;
using Business.Tools;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;



namespace Business.Payment.Http.Services
{
    public class HttpPaymentService : HttpBaseService, IHttpBaseService, IHttpPaymentService
    {

        public HttpPaymentService(IHttpContextAccessor accessor, IWebHostEnvironment env, IExId exId, IHttpAppClient httpAppClient, IGlobalConfig_PROVIDER remoteServices_Provider, IServiceResultFactory resultFact, ConsoleWriter cm)
            : base(accessor, env, exId, httpAppClient, remoteServices_Provider, resultFact, cm)
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
