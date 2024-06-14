using Business.Http;
using Business.Http.Interfaces;
using Business.Libraries.ServiceResult.Interfaces;
using Business.Management.Appsettings;
using Business.Management.Appsettings.Interfaces;
using Business.Management.Appsettings.Models;
using Business.Management.Http.Services.Interfaces;
using Microsoft.AspNetCore.Hosting;



namespace Business.Management.Http.Services
{
    public class HttpManagementService : HttpBaseService, IHttpManagementService
    {

        private readonly IHttpAppClient _httpAppClient;
        private IAppsettings_PROVIDER _appsettings_Provider;


        public HttpManagementService(IHostingEnvironment env, IAppsettings_PROVIDER appsettings_Provider, IHttpAppClient httpAppClient, IServiceResultFactory resultFact)
            : base(env, appsettings_Provider, httpAppClient, resultFact)
        {
            _remoteServiceName = "ManagementService";
            _remoteServicePathName = "RemoteService";
            _httpAppClient = httpAppClient;
            _appsettings_Provider = appsettings_Provider;
        }





        protected async override Task<HttpResponseMessage> Send()
        {         
            return await _httpAppClient.Send(_requestMessage);
        }

        protected override IServiceResult<RemoteService_MODEL_AS> GetServiceModel()
        {
            return _appsettings_Provider.GetRemoteServiceModel(_remoteServiceName);
        }



        public async Task<IServiceResult<IEnumerable<RemoteService_MODEL_AS>>> GetAllRemoteServices()
        {
            _method = HttpMethod.Get;
            _requestQuery = $"{"url/all"}";

            _useApiKey = true;

            return await HTTP_Request_Handler<IEnumerable<RemoteService_MODEL_AS>>();
        }

    }
}
