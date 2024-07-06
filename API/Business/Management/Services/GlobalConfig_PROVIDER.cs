using Business.Libraries.ServiceResult.Interfaces;
using Business.Management.Appsettings;
using Business.Management.Appsettings.Interfaces;
using Business.Management.Appsettings.Models;
using Business.Management.Enums;
using Business.Management.Http.Services.Interfaces;
using Business.Management.Services.Interfaces;
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
        private IAppsettings_PROVIDER _appsettings_Provider;



        public GlobalConfig_PROVIDER(IWebHostEnvironment env, IServiceResultFactory resultFact, IConfig_Global_REPO config_global_Repo, IHttpManagementService httpManagementService, IAppsettings_PROVIDER appsettings_Provider)
        {
            _isProdEnv = env.IsProduction();
            _config_global_Repo = config_global_Repo;
            _httpManagementService = httpManagementService;
            _resultFact = resultFact;
            _appsettings_Provider = appsettings_Provider;
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

            if (serviceModels.IsNullOrEmpty())
                return _resultFact.Result<IEnumerable<RemoteService_AS_MODEL>>(null, false, $"Service models with HTTP Clients were NOT found ! n\\ Possible solution: Reload models from Management service Appsettings.");

            return _resultFact.Result<IEnumerable<RemoteService_AS_MODEL>>(serviceModels, true);
        }





        // Auth:

        public IServiceResult<string> GetApiKey()
        {
            var apiKey = _config_global_Repo.Auth.Apikey;

            return _resultFact.Result(apiKey, !string.IsNullOrWhiteSpace(apiKey), string.IsNullOrWhiteSpace(apiKey) 
                ? $"Api-Key not found in Global Config ! n\\ Possible solution: Reload Global Config from Management service." : "");
        }




        public IServiceResult<string> GetJWTKey()
        {
            var JWTKey = _config_global_Repo.Auth.JWTKey;

            return _resultFact.Result(JWTKey, !string.IsNullOrWhiteSpace(JWTKey), string.IsNullOrWhiteSpace(JWTKey)
                ? $"JWT-Key not found in Global Config ! n\\ Possible solution: Reload Global Config from Management service." : "");
        }





        // RabbiotMQ:

        public IServiceResult<RabbitMQ_AS_MODEL> GetRabbitMQ()
        {
            var rabbitMQ = _config_global_Repo.RabbitMQ.Data;

            return _resultFact.Result(rabbitMQ, rabbitMQ != null, rabbitMQ == null
                ? $"RabbitMQ data were not found in Global Config ! n\\ Possible solution: Reload Global Config from Management service." : "");
        }







        // ------------------------------------------------------------------------------------ WRITE: ------------------------------------------------------------------------------

        // HTTP
        // For ALL services !
        // UPDATE Remote Services models by GET response from Management service:
        public async Task<IServiceResult<Config_Global_AS_MODEL>> ReLoad()
        {
            var result = await _httpManagementService.GetGlobalConfig();

            if (!result.Status || result.Data == null)
                return _resultFact.Result<Config_Global_AS_MODEL>(null, false, $"Global Config was NOT received from Management service ! Reason: {result.Message}");

            result = Update(result.Data);

            return result;
        }



        // HTTP
        // For ALL services !
        // UPDATE Remote Services models by GET response from Management service:
        public async Task<IServiceResult<IEnumerable<RemoteService_AS_MODEL>>> ReLoadRemoteServices()
        {
            var result = await _httpManagementService.GetAllRemoteServices();

            if (!result.Status || result.Data.IsNullOrEmpty())
                return _resultFact.Result<IEnumerable<RemoteService_AS_MODEL>>(null, false, $"Remote Services models were NOT received from Management service ! Reason: {result.Message}");

            result = UpdateRemoteServiceModels(result.Data);

            return result;
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

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("--> SUCCESS: ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Global Config was updated.");

            return _resultFact.Result(config, true);
        }




        // For MANAGEMENT service !
        // UPDATE Remote Services models by incoming PUT request from Management API service:

        // WARNING: in K8 only one replica (NOT all) of this service will be chosen for PUT request by load balancer!
        // Individual GET request from all replicas is prefered after handling HTTP 503 as a result of expired/changed/faulty URL.
        public IServiceResult<IEnumerable<RemoteService_AS_MODEL>> UpdateRemoteServiceModels(IEnumerable<RemoteService_AS_MODEL> servicesModels)
        {
            if (servicesModels.IsNullOrEmpty())
                return _resultFact.Result<IEnumerable<RemoteService_AS_MODEL>>(null, false, $"Services models for Management service were NOT provided !");

            var message = "WARNING ! There were missing data in provided Services models: ";

            foreach (var model in servicesModels)
            {
                var baseUrl = model.GetBaseUrl(TypeOfService.REST, _isProdEnv);

                if (string.IsNullOrWhiteSpace(model.Name) || string.IsNullOrWhiteSpace(baseUrl) || model.Type.IsNullOrEmpty())
                    message += $"n\\n\\- SERVICE: Name: '{model.Name}', Base URL: '{baseUrl}', Type: '{model.Type}'";

                foreach (var path in model.GetPaths(TypeOfService.REST))
                {
                    if (string.IsNullOrWhiteSpace(path.Name) || string.IsNullOrWhiteSpace(path.Route))
                        message += $"n\\ -- Path: '{path.Name}', Route: '{path.Route}'";
                }
            }

            _config_global_Repo.RemoteServices.Initialize(servicesModels.ToList());

            return _resultFact.Result(servicesModels, true);
        }


    }
}
