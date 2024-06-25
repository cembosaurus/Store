using Business.Exceptions.Interfaces;
using Business.Http.Clients;
using Business.Http.Services;
using Business.Libraries.ServiceResult.Interfaces;
using Business.Management.Appsettings.Interfaces;
using Business.Management.Services.Interfaces;
using Business.Scheduler.DTOs;
using Business.Scheduler.Http.Services.Interfaces;
using Microsoft.AspNetCore.Hosting;



namespace Business.Scheduler.Http.Services
{
    public class HttpSchedulerService : HttpBaseService, IHttpSchedulerService
    {

        public HttpSchedulerService(IWebHostEnvironment env, IExId exId, IAppsettings_PROVIDER appsettingsService, IHttpAppClient httpAppClient, IServiceResultFactory resultFact, IGlobal_Settings_PROVIDER remoteServices_Provider)
            : base(env, exId, appsettingsService, httpAppClient, remoteServices_Provider, resultFact)
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
