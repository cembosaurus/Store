using Business.Exceptions.Interfaces;
using Business.Http;
using Business.Http.Interfaces;
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

        public HttpSchedulerService(IHostingEnvironment env, IExId exId, IAppsettingsService appsettingsService, IHttpAppClient httpAppClient, IServiceResultFactory resultFact, IRemoteServicesInfo_Provider remoteServicesInfoService)
            : base(env, exId, appsettingsService, httpAppClient, remoteServicesInfoService, resultFact)
        {
            _remoteServiceName = "SchedulerService";
            _remoteServicePathName = "Scheduler";
        }





        public async Task<IServiceResult<CartItemsLockReadDTO>> CartItemsLock(CartItemsLockCreateDTO cartItemsToLock)
        {
            _method = HttpMethod.Post;
            _requestQuery = $"/cartitem/lock";
            _content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(cartItemsToLock), _encoding, _mediaType);

            return await HTTP_Request_Handler<CartItemsLockReadDTO>();
        }



        public async Task<IServiceResult<CartItemsLockReadDTO>> CartItemsUnLock(CartItemsLockDeleteDTO cartItemsToUnLock)
        {
            _method = HttpMethod.Delete;
            _requestQuery = $"/cartitem/lock";
            _content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(cartItemsToUnLock), _encoding, _mediaType);

            return await HTTP_Request_Handler<CartItemsLockReadDTO>();
        }


    }
}
