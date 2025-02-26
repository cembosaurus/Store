using AutoMapper;
using Business.Enums;
using Business.Libraries.ServiceResult.Interfaces;
using Business.Management.Appsettings.Interfaces;
using Business.Management.Appsettings.Models;
using Business.Management.Enums;
using Business.Management.Http.Services.Interfaces;
using Business.Management.Services.Interfaces;
using Business.Tools;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;




namespace Business.Management.Services
{
    public class GlobalConfig_PROVIDER : IGlobalConfig_PROVIDER
    {

        private readonly bool _isProdEnv;
        private IConfig_Global_REPO _config_global_Repo;
        private readonly IHttpManagementService _httpManagementService;
        private readonly IServiceResultFactory _resultFact;
        private IMapper _mapper;
        private readonly ConsoleWriter _cw;

        public GlobalConfig_PROVIDER(IWebHostEnvironment env, IServiceResultFactory resultFact, IConfig_Global_REPO config_global_Repo, IHttpManagementService httpManagementService, IMapper mapper, ConsoleWriter cw)
        {
            _isProdEnv = env.IsProduction();
            _config_global_Repo = config_global_Repo;
            _httpManagementService = httpManagementService;
            _resultFact = resultFact;
            _mapper = mapper;
            _cw = cw;
        }





        // ------------------------------------------------------------------------------------ READ: ------------------------------------------------------------------------------

        // Global Config:

        public IServiceResult<Config_Global_AS_MODEL> GetGlobalConfig()
        {
            var globalConfigModel = _config_global_Repo.GlobalConfig;

            if (globalConfigModel == null)
                return _resultFact.Result(globalConfigModel, false, $"Global Config model was not found in repository !");

            return _resultFact.Result(globalConfigModel, true);
        }



        // Remote Services:

        public IServiceResult<bool> IsEmpty_RemoteServiceModels()
        {
            return _resultFact.Result(_config_global_Repo.RemoteServices.IsEmpty(), true);
        }


        public IServiceResult<RemoteService_AS_MODEL> GetRemoteServiceByName(string name) 
        {
            if(string.IsNullOrWhiteSpace(name))
                return _resultFact.Result<RemoteService_AS_MODEL>(null, false, "Remote Service name was NOT provided !");
            if(_config_global_Repo.RemoteServices.IsEmpty())
                return _resultFact.Result<RemoteService_AS_MODEL>(null, false, $"Remote Services repo is empty ! n\\ Possible solution: Reload models from Management service.");

            var serviceModel = _config_global_Repo.RemoteServices.GetByName(name);

            if (serviceModel == null)
                return _resultFact.Result<RemoteService_AS_MODEL>(null, false, $"Service '{name}' was NOT found ! n\\ Possible solution: Reload models from Management service.");
            if (string.IsNullOrWhiteSpace(serviceModel.GetBaseUrl(TypeOfService.REST, _isProdEnv)))
                return _resultFact.Result<RemoteService_AS_MODEL>(null, false, $"Service '{name}' was found but there is missing Base URL !");

            return _resultFact.Result(serviceModel, true);
        }


        public IServiceResult<RemoteService_AS_MODEL> GetRemoteServiceByBaseURL(string baseURL)
        {
            if (string.IsNullOrWhiteSpace(baseURL))
                return _resultFact.Result<RemoteService_AS_MODEL>(null, false, "Remote Service Base URL was NOT provided !");
            if (_config_global_Repo.RemoteServices.IsEmpty())
                return _resultFact.Result<RemoteService_AS_MODEL>(null, false, $"Remote Services repo is empty ! n\\ Possible solution: Reload models from Management service.");

            var serviceModel = _config_global_Repo.RemoteServices.GetByBaseURL(baseURL);

            if (serviceModel == null)
                return _resultFact.Result(serviceModel, false, $"Service model with Base URL '{baseURL}' was NOT found !");

            return _resultFact.Result(serviceModel, true);
        }



        public IServiceResult<string> GetRemoteServiceURL_WithPath(string serviceName, string pathName)
        {
            if (string.IsNullOrWhiteSpace(serviceName))
                return _resultFact.Result("", false, "Remote Service name was NOT provided !");
            if (string.IsNullOrWhiteSpace(pathName))
                return _resultFact.Result("", false, $"Path for Remote Service '{serviceName}' was NOT provided !");

            var modelResult = GetRemoteServiceByName(serviceName);

            if(!modelResult.Status)
                return _resultFact.Result("", false, modelResult.Message);

            var urlWithPath = modelResult.Data.GetUrlWithPath(TypeOfService.REST, pathName, _isProdEnv);

            if (string.IsNullOrWhiteSpace(urlWithPath))
                return _resultFact.Result("", false, $"Model of remote service: '{serviceName}' doesn't contain path '{pathName}' !");
            
            return _resultFact.Result(urlWithPath, true);
        }



        public IServiceResult<string> GetRemoteServiceURL_WithPath(RemoteService_AS_MODEL serviceModel, string pathName)
        {
            if (string.IsNullOrWhiteSpace(serviceModel.Name))
                return _resultFact.Result("", false, "Remote Service name is missing !");
            if (string.IsNullOrWhiteSpace(pathName))
                return _resultFact.Result("", false, $"Path for Remote Service '{serviceModel.Name}' was NOT provided !");

            //................................... NOT every remote service URL has path:
            //if (string.IsNullOrWhiteSpace(serviceUrl.GetPathByName(TypeOfService.REST, pathName)))
            //    return _resultFact.Result("", false, $"Path '{pathName}' for Remote Service '{serviceUrl.Name}' NOT found !");

            var urlWithPath = serviceModel.GetUrlWithPath(TypeOfService.REST, pathName, _isProdEnv);

            if (string.IsNullOrWhiteSpace(urlWithPath))
                return _resultFact.Result("", false, $"Path: '{pathName}' for Remote Service: '{serviceModel.Name}' NOT found !");

            return _resultFact.Result(urlWithPath, true);
        }




        public IServiceResult<IEnumerable<RemoteService_AS_MODEL>> GetRemoteServices_WithGlobalConfig()
        {
            if (_config_global_Repo.RemoteServices.IsEmpty())
                return _resultFact.Result<IEnumerable<RemoteService_AS_MODEL>>(null, false, $"Remote Services repo is empty ! n\\ Possible solution: Reload models from Management service Appsettings.");

            var serviceModels = _config_global_Repo.RemoteServices.GetByPathName("GlobalConfig");

            if (!serviceModels.Any())
                return _resultFact.Result<IEnumerable<RemoteService_AS_MODEL>>(null, false, $"Service models with HTTP Clients were NOT found ! n\\ Possible solution: Reload models from Management service Appsettings.");

            return _resultFact.Result<IEnumerable<RemoteService_AS_MODEL>>(serviceModels, true);
        }





        // Auth:

        public IServiceResult<string> GetApiKey()
        {
            var apiKey = _config_global_Repo.Auth.Apikey;

            return _resultFact.Result(apiKey, !string.IsNullOrWhiteSpace(apiKey), string.IsNullOrWhiteSpace(apiKey) ? "API-Key was NOT found in Global Config !" : "");

        }




        public IServiceResult<string> GetJWTKey()
        {
            var JWTKey = _config_global_Repo.Auth.JWTKey;

            return _resultFact.Result(JWTKey, !string.IsNullOrWhiteSpace(JWTKey), string.IsNullOrWhiteSpace(JWTKey) ? "JWT-Key was NOT found in Global Config !" : "");
        }





        // RabbiotMQ:

        public IServiceResult<RabbitMQ_AS_MODEL> GetRabbitMQ()
        {
            var rabbitMQ = _config_global_Repo.RabbitMQ.Data;

            return _resultFact.Result(rabbitMQ, rabbitMQ != null, rabbitMQ == null ? $"RabbitMQ config was not found in Global Config !" : "");
        }





        // Persistence:

        public IServiceResult<Persistence_AS_MODEL> GetPersistence()
        {
            var persistence = _config_global_Repo.Persistence.Data;

            return _resultFact.Result(persistence, persistence != null, persistence == null ? $"Persistence config was not found in Global Config !" : "");
        }







        // ------------------------------------------------------------------------------------ WRITE: ------------------------------------------------------------------------------

        // HTTP
        // For ALL services !
        // UPDATE Remote Services models by GET response from Management service:
        public async Task<IServiceResult<Config_Global_AS_MODEL>> DownloadGlobalConfig()
        {
            var httpResult = await _httpManagementService.GetGlobalConfig();

            if (!httpResult.Status || httpResult.Data == null)
                return _resultFact.Result<Config_Global_AS_MODEL>(null, false, $"Global Config was NOT received from Management service ! Reason: {httpResult.Message}");

            var result = _mapper.Map<Config_Global_AS_MODEL>(httpResult.Data);

            return Update(result);
        }



        // HTTP
        // For ALL services !
        // UPDATE Remote Services models by GET response from Management service:
        public async Task<IServiceResult<IEnumerable<RemoteService_AS_MODEL>>> DownloadRemoteServicesModels()
        {
            var httpResult = await _httpManagementService.GetAllRemoteServices();

            if (!httpResult.Status || !httpResult.Data.Any())
                return _resultFact.Result<IEnumerable<RemoteService_AS_MODEL>>(null, false, $"Remote Services models were NOT received from Management service ! Reason: {httpResult.Message}");

            var result = _mapper.Map<IEnumerable<RemoteService_AS_MODEL>>(httpResult.Data);

            return UpdateRemoteServiceModels(result);
        }







        // For MANAGEMENT service !
        // UPDATE Remote Services models by incoming PUT request from Management API service:

        // WARNING: in K8 only one replica (NOT all) of this service will be chosen for PUT request by load balancer!
        // Individual GET request from all replicas is prefered after handling HTTP 503 as a result of expired/changed/faulty URL.
        public IServiceResult<Config_Global_AS_MODEL> Update(Config_Global_AS_MODEL config)
        {
            if (config == null)
                return _resultFact.Result<Config_Global_AS_MODEL>(null, false, $"Global Config data for Management service was NOT provided !");

            _config_global_Repo.Initialize(config);

            _cw.Message("Global Config", "Global Config Provider", "Update", TypeOfInfo.INFO, "Global Config Updated.");

            return _resultFact.Result(config, true);
        }




        // For MANAGEMENT service !
        // UPDATE Remote Services models by incoming PUT request from Management API service:

        // WARNING: in K8 only one replica (NOT all) of this service will be chosen for PUT request by load balancer!
        // Individual GET request from all replicas is prefered after handling HTTP 503 as a result of expired/changed/faulty URL.
        public IServiceResult<IEnumerable<RemoteService_AS_MODEL>> UpdateRemoteServiceModels(IEnumerable<RemoteService_AS_MODEL> servicesModels)
        {
            if (!servicesModels.Any())
                return _resultFact.Result<IEnumerable<RemoteService_AS_MODEL>>(null, false, $"Services models for Management service were NOT provided !");

            var message = "WARNING ! There were missing data in provided Services models: ";

            foreach (var model in servicesModels)
            {
                var baseUrl = model.GetBaseUrl(TypeOfService.REST, _isProdEnv);

                if (string.IsNullOrWhiteSpace(model.Name) || string.IsNullOrWhiteSpace(baseUrl) || !model.Type.Any())
                    message += $"n\\n\\- SERVICE: Name: '{model.Name}', Base URL: '{baseUrl}', Type: '{model.Type}'";

                foreach (var path in model.GetPaths(TypeOfService.REST))
                {
                    if (string.IsNullOrWhiteSpace(path.Name) || string.IsNullOrWhiteSpace(path.Route))
                        message += $"n\\ -- Path: '{path.Name}', Route: '{path.Route}'";
                }
            }

            _config_global_Repo.RemoteServices.Initialize(servicesModels.ToList());

            _cw.Message("Global Config", "Global Config Provider", "Update", TypeOfInfo.INFO, "Remote Services Updated.");

            return _resultFact.Result(servicesModels, true);
        }


    }
}
