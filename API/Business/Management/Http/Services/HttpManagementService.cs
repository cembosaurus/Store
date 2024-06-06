using Business.Http;
using Business.Http.Interfaces;
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
        private readonly IServiceResultFactory _resultFact;



        public HttpManagementService(IHostingEnvironment env, IAppsettingsService appsettingsService, IHttpAppClient httpAppClient, IServiceResultFactory resultFact)
            : base(env, appsettingsService, httpAppClient, resultFact)
        {
            _remoteServiceName = "ManagementService";
            _remoteServicePathName = "RemoteService";
            _httpAppClient = httpAppClient;
            _resultFact = resultFact;           
        }





        protected async override Task<HttpResponseMessage> Send()
        {         
            return await _httpAppClient.Send(_requestMessage);
        }


        public async Task<IServiceResult<IEnumerable<Service_Model_AS>>> GetAllRemoteServices()
        {
            _method = HttpMethod.Get;
            _requestQuery = $"{"url/all"}";

            return await HTTP_Request_Handler<IEnumerable<Service_Model_AS>>();
        }

    }
}
