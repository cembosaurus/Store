using Business.Libraries.ServiceResult.Interfaces;
using Business.Management.Appsettings.Interfaces;
using Business.Management.Appsettings.Models;
using Business.Management.Enums;
using Business.Management.Http.Services.Interfaces;
using Business.Management.Services.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.IdentityModel.Tokens;


namespace Business.Management.Services
{
    public class Global_Settings_PROVIDER : IGlobal_Settings_PROVIDER
    {
        private readonly bool _isProdEnv;
        private IRemoteServices_REPO _remoteServices_Repo;
        private readonly IHttpManagementService _httpManagementService;
        private readonly IServiceResultFactory _resultFact;



        public Global_Settings_PROVIDER(IHostingEnvironment env, IServiceResultFactory resultFact, IConfig_Global_REPO config_global_Repo, IHttpManagementService httpManagementService)
        {
            _isProdEnv = env.IsProduction();
            _remoteServices_Repo = config_global_Repo.RemoteServices;
            _httpManagementService = httpManagementService;
            _resultFact = resultFact;
        }






        public IServiceResult<bool> IsEmpty()
        {
            return _resultFact.Result(_remoteServices_Repo.IsEmpty(), true);
        }


        public IServiceResult<RemoteService_MODEL_AS> GetRemoteServiceByName(string name) 
        {
            if(string.IsNullOrWhiteSpace(name))
                return _resultFact.Result<RemoteService_MODEL_AS>(null, false, "Remote Service name was NOT provided !");
            if(_remoteServices_Repo.IsEmpty())
                return _resultFact.Result<RemoteService_MODEL_AS>(null, false, $"Remote Services repo is empty ! n\\ Possible solution: Reload models from Management service.");

            var serviceModel = _remoteServices_Repo.GetByName(name);

            if (serviceModel == null)
                return _resultFact.Result<RemoteService_MODEL_AS>(null, false, $"Service '{name}' was NOT found ! n\\ Possible solution: Reload models from Management service.");
            if (string.IsNullOrWhiteSpace(serviceModel.GetBaseUrl(TypeOfService.REST, _isProdEnv)))
                return _resultFact.Result<RemoteService_MODEL_AS>(null, false, $"Service '{name}' was found but there is missing Base URL !");

            return _resultFact.Result(serviceModel, true);
        }


        public IServiceResult<RemoteService_MODEL_AS> GetRemoteServiceByBaseURL(string baseURL)
        {
            if (string.IsNullOrWhiteSpace(baseURL))
                return _resultFact.Result<RemoteService_MODEL_AS>(null, false, "Remote Service Base URL was NOT provided !");
            if (_remoteServices_Repo.IsEmpty())
                return _resultFact.Result<RemoteService_MODEL_AS>(null, false, $"Remote Services repo is empty ! n\\ Possible solution: Reload models from Management service.");

            var serviceModel = _remoteServices_Repo.GetByBaseURL(baseURL);

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



        public IServiceResult<string> GetRemoteServiceURL_WithPath(RemoteService_MODEL_AS serviceUrl, string pathName)
        {
            if (string.IsNullOrWhiteSpace(serviceUrl.Name))
                return _resultFact.Result("", false, "Remote Service name is missing !");
            if (string.IsNullOrWhiteSpace(pathName))
                return _resultFact.Result("", false, $"Path for Remote Service '{serviceUrl.Name}' was NOT provided !");

            //................................... NOT every remote service URL has path:
            //if (string.IsNullOrWhiteSpace(serviceUrl.GetPathByName(TypeOfService.REST, pathName)))
            //    return _resultFact.Result("", false, $"Path '{pathName}' for Remote Service '{serviceUrl.Name}' NOT found !");

            var urlWithPath = serviceUrl.GetUrlWithPath(TypeOfService.REST, pathName, _isProdEnv);

            if (string.IsNullOrWhiteSpace(urlWithPath))
                return _resultFact.Result("", false, $"Path: '{pathName}' for Remote Service: '{serviceUrl.Name}' NOT found !");

            return _resultFact.Result(urlWithPath, true);
        }




        public IServiceResult<IEnumerable<RemoteService_MODEL_AS>> GetRemoteServices_WithHTTPClient()
        {
            if (_remoteServices_Repo.IsEmpty())
                return _resultFact.Result<IEnumerable<RemoteService_MODEL_AS>>(null, false, $"Remote Services repo is empty ! n\\ Possible solution: Reload models from Management service Appsettings.");

            var serviceModels = _remoteServices_Repo.GetHttpClients();

            if (serviceModels.IsNullOrEmpty())
                return _resultFact.Result<IEnumerable<RemoteService_MODEL_AS>>(null, false, $"Service models with HTTP Clients were NOT found ! n\\ Possible solution: Reload models from Management service Appsettings.");

            return _resultFact.Result<IEnumerable<RemoteService_MODEL_AS>>(serviceModels, true);
        }




        public IServiceResult<bool> IsRemoteServiceHttpClient(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return _resultFact.Result(false, false, "Remote Service name was NOT provided !");

            var result = GetRemoteServiceByName(name);

            if (!result.Status)
                return _resultFact.Result(result.Status, result.Status, result.Message);

            return _resultFact.Result(result.Data.IsHTTPClient, result.Status);
        }








        // For ALL services !
        // UPDATE Remote Services models by GET response from Management service:
        public async Task<IServiceResult<IEnumerable<RemoteService_MODEL_AS>>> ReLoadRemoteServices()
        {
            var result = await _httpManagementService.GetAllRemoteServices();

            if (!result.Status || result.Data.IsNullOrEmpty())
                return _resultFact.Result<IEnumerable<RemoteService_MODEL_AS>>(null, false, $"Remote Services models were NOT received from Management service ! Reason: {result.Message}");

            result =  UpdateRemoteServiceModels(result.Data);

            return result;
        }



        // For MANAGEMENT service !
        // UPDATE Remote Services models by incoming PUT request from Management API service:

        // WARNING: in K8 only one replica (NOT all) of this service will be chosen for PUT request by load balancer!
        // Individual GET request from all replicas is prefered after handling HTTP 503 as a result of expired/changed/faulty URL.
        public IServiceResult<IEnumerable<RemoteService_MODEL_AS>> UpdateRemoteServiceModels(IEnumerable<RemoteService_MODEL_AS> servicesModels)
        {
            Console.WriteLine($"--> UPDATING Services models ......");

            if (servicesModels.IsNullOrEmpty())
                return _resultFact.Result<IEnumerable<RemoteService_MODEL_AS>>(null, false, $"Services models for Management service were NOT provided !");

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

            _remoteServices_Repo.InitializeDB(servicesModels.ToList());

            return _resultFact.Result(servicesModels, true);
        }
    }
}
