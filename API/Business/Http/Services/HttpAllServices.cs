using AutoMapper;
using Business.Enums;
using Business.Exceptions.Interfaces;
using Business.Http.Clients.Interfaces;
using Business.Http.Services.Interfaces;
using Business.Libraries.ServiceResult.Interfaces;
using Business.Management.Appsettings.DTOs;
using Business.Management.Appsettings.Models;
using Business.Management.Enums;
using Business.Management.Services.Interfaces;
using Business.Tools;
using Microsoft.AspNetCore.Hosting;
using Microsoft.IdentityModel.Tokens;
using System.Net;



namespace Business.Http.Services
{
    // sends HTTP messages to ALL (specific) API services
    // API services URLs source: Global Config

    public class HttpAllServices : HttpBaseService, IHttpBaseService, IHttpAllServices
    {

        private readonly IHttpAppClient _httpAppClient;
        private IMapper _mapper;
        private ConsoleWriter _cm;


        public HttpAllServices(IWebHostEnvironment env, IExId exId, IHttpAppClient httpAppClient, IServiceResultFactory resultFact, IGlobalConfig_PROVIDER globalConfig_Provider, IMapper mapper, ConsoleWriter cm)
            : base(env, exId, httpAppClient, resultFact, cm)
        {
            _httpAppClient = httpAppClient;
            _globalConfig_Provider = globalConfig_Provider;
            _mapper = mapper;
            _cm = cm;
        }






        protected async override Task<HttpResponseMessage> Send()
        {
            try
            {
                return await _httpAppClient.SendAsync(_requestMessage);
            }
            catch (Exception ex) when (_exId.IsHttp_503(ex))
            {
                return _requestMessage.CreateErrorResponse(HttpStatusCode.ServiceUnavailable, $"HTTP 503: Respoonse from '{_remoteServiceName}'. Message: {ex.Message}", ex);
            }
        }



        public async Task<IServiceResult<Config_Global_AS_DTO>> PostGlobalConfig(Config_Global_AS_DTO globalConfig_Model)
        {
            _method = HttpMethod.Post;
            _requestQuery = $"";
            _content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(globalConfig_Model), _encoding, _mediaType);

            return await HTTP_Request_Handler<Config_Global_AS_DTO>();
        }




        public async Task<IServiceResult<IEnumerable<KeyValuePair<RemoteService_AS_MODEL, bool>>>> PostGlobalConfigToMultipleServices(bool allowRequestToManagementAPIService)
        {
            _remoteServicePathName = "GlobalConfig";

            var globalConfig_Model = _globalConfig_Provider.GetGlobalConfig();

            var globalConfig_DTO = _mapper.Map<Config_Global_AS_DTO>(globalConfig_Model.Data);

            var result = new List<KeyValuePair<RemoteService_AS_MODEL, bool>>();

            if (!globalConfig_Model.Status)
                return _resultFact.Result<IEnumerable<KeyValuePair<RemoteService_AS_MODEL, bool>>>(null, false, "Global Config DB not found!");
            if (globalConfig_Model.Data == null || !globalConfig_Model.Data.RemoteServices.Any())
                return _resultFact.Result<IEnumerable<KeyValuePair<RemoteService_AS_MODEL, bool>>>(null, false, "Remote Services config data were not found in Global Config DB!");


            // send Http requests to specific API services:
            foreach (var model in globalConfig_Model.Data.RemoteServices)
            {
                if (!string.IsNullOrWhiteSpace(model.GetPathByName(TypeOfService.REST, _remoteServicePathName)))
                {
                    // prevent Management service from sending HTTP request to itself:
                    if (!allowRequestToManagementAPIService && model.Name == "ManagementService")
                    {
                        result.Add(new KeyValuePair<RemoteService_AS_MODEL, bool>(model, true));

                        continue;
                    }

                    _remoteServiceName = model.Name;

                    try
                    {
                        _cm.Message("HTTP Post (outgoing)", _remoteServiceName, "Global Config update", TypeOfInfo.INFO, "Sending...");

                        var requestResult = await PostGlobalConfig(globalConfig_DTO);

                        _cm.Message("HTTP Response (incoming)", _remoteServiceName, "Global Config update", requestResult.Status ? TypeOfInfo.SUCCESS : TypeOfInfo.FAIL, $"{(requestResult.Status ? "Response received." : requestResult.Message)}");

                        result.Add(new KeyValuePair<RemoteService_AS_MODEL, bool>(model, requestResult.Status));
                    }
                    catch (Exception ex)
                    {
                        result.Add(new KeyValuePair<RemoteService_AS_MODEL, bool>(model, false));

                        _cm.Message("HTTP Response (incoming)", _remoteServiceName, _requestURL + "/" + _requestQuery, TypeOfInfo.FAIL, ex.Message);
                    }
                }
            }
            

            return _resultFact.Result<IEnumerable<KeyValuePair<RemoteService_AS_MODEL, bool>>>(result, true);
        }
    }
}
