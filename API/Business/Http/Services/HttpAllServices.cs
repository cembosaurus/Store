using Business.Http.Clients.Interfaces;
using Business.Http.Services.Interfaces;
using Business.Libraries.ServiceResult.Interfaces;
using Business.Management.Appsettings.Models;
using Business.Management.Enums;
using Business.Management.Services.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.IdentityModel.Tokens;



namespace Business.Http.Services
{
    // sends HTTP messages to ALL (specific) API services
    // URL source: Global Config

    public class HttpAllServices : HttpBaseService, IHttpAllServices
    {

        private readonly IHttpAppClient _httpAppClient;



        public HttpAllServices(IWebHostEnvironment env, IHttpAppClient httpAppClient, IServiceResultFactory resultFact, IGlobalConfig_PROVIDER globalConfig_Provider)
            : base(env, httpAppClient, resultFact)
        {
            _httpAppClient = httpAppClient;
            _globalConfig_Provider = globalConfig_Provider;
        }






        protected async override Task<HttpResponseMessage> Send()
        {
            return await _httpAppClient.Send(_requestMessage);
        }



        public async Task<IServiceResult<Config_Global_AS_MODEL>> PostGlobalConfig(Config_Global_AS_MODEL globalConfig_Model)
        {
            _method = HttpMethod.Put;
            _requestQuery = $"";
            _content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(globalConfig_Model), _encoding, _mediaType);

            return await HTTP_Request_Handler<Config_Global_AS_MODEL>();
        }




        public async Task<IServiceResult<IEnumerable<KeyValuePair<RemoteService_AS_MODEL, bool>>>> PostGlobalConfigToMultipleServices()
        {
            _remoteServicePathName = "GlobalConfig";

            var globalConfig_Model = _globalConfig_Provider.GetGlobalConfig();

            var result = new List<KeyValuePair<RemoteService_AS_MODEL, bool>>();

            if (!globalConfig_Model.Status)
                return _resultFact.Result<IEnumerable<KeyValuePair<RemoteService_AS_MODEL, bool>>>(null, false, "Global Config DB not found!");
            if (globalConfig_Model.Data.RemoteServices.IsNullOrEmpty())
                return _resultFact.Result<IEnumerable<KeyValuePair<RemoteService_AS_MODEL, bool>>>(null, false, "Remote Services were not found in Global Config DB!");


            foreach (var model in globalConfig_Model.Data.RemoteServices)
            {
                if (!model.GetPathByName(TypeOfService.REST, _remoteServicePathName).IsNullOrEmpty())
                {
                    //if (model.Name == "ManagementService")
                    //    continue;

                    _remoteServiceName = model.Name;

                    try
                    {
                        var requestResult = await PostGlobalConfig(globalConfig_Model.Data);

                        result.Add(new KeyValuePair<RemoteService_AS_MODEL, bool>(model, requestResult.Status));
                    }
                    catch (Exception ex)
                    {
                        result.Add(new KeyValuePair<RemoteService_AS_MODEL, bool>(model, false));

                        Console.ForegroundColor = ConsoleColor.Red; 
                        Console.Write("FAIL: ");
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.WriteLine($"HTTP request to '{_remoteServiceName}' --> Message: {ex.Message}");
                    }
                }
            }
            

            return _resultFact.Result<IEnumerable<KeyValuePair<RemoteService_AS_MODEL, bool>>>(result, true);
        }
    }
}
