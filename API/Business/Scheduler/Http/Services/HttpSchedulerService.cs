using Business.Exceptions.Interfaces;
using Business.Http.Clients;
using Business.Http.Services;
using Business.Libraries.ServiceResult.Interfaces;
using Business.Management.Services.Interfaces;
using Business.Scheduler.DTOs;
using Business.Scheduler.Http.Services.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace Business.Scheduler.Http.Services
{
    public class HttpSchedulerService : HttpBaseService, IHttpSchedulerService
    {

        public HttpSchedulerService(IHttpContextAccessor accessor, IWebHostEnvironment env, IExId exId, IHttpAppClient httpAppClient, IGlobalConfig_PROVIDER remoteServices_Provider, IServiceResultFactory resultFact)
            : base(accessor, env, exId, httpAppClient, remoteServices_Provider, resultFact)
        {
            _remoteServiceName = "SchedulerService";
            _remoteServicePathName = "Scheduler";
        }





        public async Task<IServiceResult<CartItemsLockReadDTO>> CartItemsLock(CartItemsLockCreateDTO cartItemsToLock)
        {
            _method = HttpMethod.Post;
            _requestQuery = $"cartitem/lock";
            _content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(cartItemsToLock), _encoding, _mediaType);

            _useApiKey = true;

            return await HTTP_Request_Handler<CartItemsLockReadDTO>();
        }



        public async Task<IServiceResult<CartItemsLockReadDTO>> CartItemsUnLock(CartItemsLockDeleteDTO cartItemsToUnLock)
        {
            _method = HttpMethod.Delete;
            _requestQuery = $"cartitem/lock";
            _content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(cartItemsToUnLock), _encoding, _mediaType);

            _useApiKey = true;

            return await HTTP_Request_Handler<CartItemsLockReadDTO>();
        }


    }
}
