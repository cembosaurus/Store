using Business.Exceptions.Interfaces;
using Business.Http.Clients.Interfaces;
using Business.Http.Services;
using Business.Libraries.ServiceResult.Interfaces;
using Business.Management.Appsettings.DTOs;
using Business.Management.Appsettings.Interfaces;
using Business.Management.Appsettings.Models;
using Business.Management.Http.Services.Interfaces;
using Business.Tools;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using System.Net;

namespace Business.Management.Http.Services
{
    // sends HTTP messages to Management API service.
    // URL routes: source Appsettings

    public class HttpManagementService : HttpBaseService, IHttpManagementService
    {

        private readonly IHttpAppClient _httpAppClient;
        private IAppsettings_PROVIDER _appsettings_Provider;


        public HttpManagementService(IWebHostEnvironment env, IExId exId, IAppsettings_PROVIDER appsettings_Provider, IHttpAppClient httpAppClient, IServiceResultFactory resultFact, ConsoleWriter cm)
            : base(env, exId, httpAppClient, resultFact, cm)
        {
            _remoteServiceName = "ManagementService";
            _remoteServicePathName = "GlobalConfig";
            _httpAppClient = httpAppClient;
            _appsettings_Provider = appsettings_Provider;
        }





        protected async override Task<HttpResponseMessage> Send()
        {
            try
            {
                return await _httpAppClient.SendAsync(_requestMessage);
            }
            catch (Exception ex) when (_exId.Http_503(ex))
            {
                return _requestMessage.CreateErrorResponse(HttpStatusCode.ServiceUnavailable, $"HTTP 503: Respoonse from '{_remoteServiceName}'. Message: {ex.Message}", ex);
            }
        }


        protected override IServiceResult<RemoteService_AS_MODEL> GetServiceModel_FromLocalGlobalConfig()
        {
            return _appsettings_Provider.GetRemoteServiceModel(_remoteServiceName);
        }


        protected override IServiceResult<bool> AddApiKeyToHeader()
        {
            var apiKeyResult = _appsettings_Provider.GetApiKey();

            if (apiKeyResult.Status)
                _requestHeaders.Add("x-api-key", apiKeyResult.Data ?? "");

            return _resultFact.Result(apiKeyResult.Status, apiKeyResult.Status, apiKeyResult.Message);
        }






        public async Task<IServiceResult<Config_Global_AS_DTO>> GetGlobalConfig()
        {
            _method = HttpMethod.Get;
            _requestQuery = $"";

             return await HTTP_Request_Handler<Config_Global_AS_DTO>();
        }


        public async Task<IServiceResult<IEnumerable<RemoteService_AS_DTO>>> GetAllRemoteServices()
        {
            _method = HttpMethod.Get;
            _requestQuery = $"{"services"}";

            return await HTTP_Request_Handler<IEnumerable<RemoteService_AS_DTO>>();
        }




        public async Task<IServiceResult<RemoteService_AS_DTO>> AddRemoteService(RemoteService_AS_DTO service)
        {
            _method = HttpMethod.Post;
            _requestQuery = $"{"services/add"}";
            _content = new StringContent(JsonConvert.SerializeObject(service), _encoding, _mediaType);

            return await HTTP_Request_Handler<RemoteService_AS_DTO>();
        }

    }
}
