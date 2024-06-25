using Business.Http.Clients;
using Business.Http.Services;
using Business.Libraries.ServiceResult.Interfaces;
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


        public HttpManagementService(IWebHostEnvironment env, IAppsettings_PROVIDER appsettings_Provider, IHttpAppClient httpAppClient, IServiceResultFactory resultFact)
            : base(env, appsettings_Provider, httpAppClient, resultFact)
        {
            _remoteServiceName = "ManagementService";
            _remoteServicePathName = "GlobalConfig";
            _httpAppClient = httpAppClient;
            _appsettings_Provider = appsettings_Provider;
        }





        protected async override Task<HttpResponseMessage> Send()
        {         
            return await _httpAppClient.Send(_requestMessage);
        }

        protected override IServiceResult<RemoteService_AS_MODEL> GetServiceModel()
        {
            return _appsettings_Provider.GetRemoteServiceModel(_remoteServiceName);
        }





        public async Task<IServiceResult<Config_Global_AS_MODEL>> GetGlobalConfig()
        {
            _method = HttpMethod.Get;
            _requestQuery = $"";

            _useApiKey = true;

            return await HTTP_Request_Handler<Config_Global_AS_MODEL>();
        }


        public async Task<IServiceResult<IEnumerable<RemoteService_AS_MODEL>>> GetAllRemoteServices()
        {
            _method = HttpMethod.Get;
            _requestQuery = $"{"services"}";

            _useApiKey = true;

            return await HTTP_Request_Handler<IEnumerable<RemoteService_AS_MODEL>>();
        }

    }
}
