using Business.Http.Clients;
using Business.Http.Services.Interfaces;
using Business.Libraries.ServiceResult.Interfaces;
using Business.Management.Appsettings.Models;
using Business.Management.Enums;
using Business.Management.Services.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.IdentityModel.Tokens;



namespace Business.Http.Services
{
    public class HttpGlobalConfigService : HttpBaseService, IHttpGlobalConfigService
    {

        private readonly IHttpAppClient _httpAppClient;
        private IGlobalConfig_PROVIDER _globalConfig_Provider;



        public HttpGlobalConfigService(IWebHostEnvironment env, IHttpAppClient httpAppClient, IServiceResultFactory resultFact, IGlobalConfig_PROVIDER globalConfig_Provider)
            : base(env, httpAppClient, resultFact)
        {
            _httpAppClient = httpAppClient;
            _globalConfig_Provider = globalConfig_Provider;
        }






        protected async override Task<HttpResponseMessage> Send()
        {
            return await _httpAppClient.Send(_requestMessage);
        }



        public async Task<IServiceResult<IEnumerable<RemoteService_AS_MODEL>>> PostGlobalConfig(Config_Global_AS_MODEL globalConfig_Model)
        {
            _method = HttpMethod.Put;
            _requestQuery = $"";
            _content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(globalConfig_Model), _encoding, _mediaType);

            _useApiKey = true;

            return await HTTP_Request_Handler<IEnumerable<RemoteService_AS_MODEL>>();
        }




        public async Task<IServiceResult<IEnumerable<RemoteService_AS_MODEL>>> PostGlobalConfigToMultipleServices()
        {
            _remoteServicePathName = "GlobalConfig";

            var globalConfig_Model = _globalConfig_Provider.GetGlobalConfig();

            if (globalConfig_Model.Status)
                return _resultFact.Result<IEnumerable<RemoteService_AS_MODEL>>(null, false, "Global Config DB not found!");
            if (globalConfig_Model.Data.RemoteServices.IsNullOrEmpty())
                return _resultFact.Result<IEnumerable<RemoteService_AS_MODEL>>(null, false, "Remote Services were not found in Global Config DB!");


            foreach (var model in globalConfig_Model.Data.RemoteServices)
            {
                if (!model.GetPathByName(TypeOfService.REST, _remoteServicePathName).IsNullOrEmpty())
                {
                    _remoteServiceName = model.Name;

                    try
                    {
                        await PostGlobalConfig(globalConfig_Model.Data);
                    }
                    catch (Exception ex)
                    {
                        var e = ex.Message;
                    }
                }
            }


            return null;
        }
    }
}
