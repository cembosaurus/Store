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
    public class RemoteServicesInfoService : IRemoteServicesInfoService
    {
        private readonly bool _isProdEnv;
        private IRemoteServicesInfo_Repo _remoteServicesInfo_Repo;
        private readonly IHttpManagementService _httpManagementService;
        private readonly IServiceResultFactory _resultFact;



        public RemoteServicesInfoService(IHostingEnvironment env, IServiceResultFactory resultFact, IRemoteServicesInfo_Repo remoteServicesInfo_repo, IHttpManagementService httpManagementService)
        {
            _isProdEnv = env.IsProduction();
            _remoteServicesInfo_Repo = remoteServicesInfo_repo;
            _httpManagementService = httpManagementService;
            _resultFact = resultFact;
        }






        public IServiceResult<Service_Model_AS> GetServiceByName(string name) 
        {
            if(string.IsNullOrWhiteSpace(name))
                return _resultFact.Result<Service_Model_AS>(null, false, "Remote Service name was NOT provided !");
            if(_remoteServicesInfo_Repo.GetAllURLs().IsNullOrEmpty())
                return _resultFact.Result<Service_Model_AS>(null, false, $"Local URLs DB is empty ! n\\ Possible solution: Get URL from Management service.");

            var serviceURL = _remoteServicesInfo_Repo.GetByName(name);

            if (serviceURL == null)
                return _resultFact.Result<Service_Model_AS>(null, false, $"Service '{name}' was NOT found ! n\\ Possible solution: Get URL from Management service.");
            if (string.IsNullOrWhiteSpace(serviceURL.GetBaseUrl(TypeOfService.REST, _isProdEnv)))
                return _resultFact.Result<Service_Model_AS>(null, false, $"Service '{name}' was found but there is missing Base URL !");

            return _resultFact.Result(serviceURL, true);
        }


        public IServiceResult<Service_Model_AS> GetServiceByBaseURL(string baseURL)
        {
            if (string.IsNullOrWhiteSpace(baseURL))
                return _resultFact.Result<Service_Model_AS>(null, false, "Remote Service Base URL was NOT provided !");
            if (_remoteServicesInfo_Repo.GetAllURLs().IsNullOrEmpty())
                return _resultFact.Result<Service_Model_AS>(null, false, $"Local URLs DB is empty ! n\\ Possible solution: Get URL from Management service.");

            var serviceURL = _remoteServicesInfo_Repo.GetByBaseURL(baseURL);

            if (serviceURL == null)
                return _resultFact.Result(serviceURL, false, $"Service with Base URL '{baseURL}' was NOT found !");

            return _resultFact.Result(serviceURL, true);
        }



        public IServiceResult<string> GetServiceURL_WithPath(string serviceName, string pathName)
        {
            if (string.IsNullOrWhiteSpace(serviceName))
                return _resultFact.Result("", false, "Remote Service name was NOT provided !");
            if (string.IsNullOrWhiteSpace(pathName))
                return _resultFact.Result("", false, $"Path for Remote Service '{serviceName}' was NOT provided !");

            var urlResult = GetServiceByName(serviceName);

            if(!urlResult.Status)
                return _resultFact.Result("", false, urlResult.Message);

            var urlWithPath = urlResult.Data.GetUrlWithPath(TypeOfService.REST, pathName, _isProdEnv);

            if (string.IsNullOrWhiteSpace(urlWithPath))
                return _resultFact.Result("", false, $"Path: '{pathName}' for Remote Service: '{serviceName}' NOT found !");
            
            return _resultFact.Result(urlWithPath, true);
        }



        public IServiceResult<string> GetServiceURL_WithPath(Service_Model_AS serviceUrl, string pathName)
        {
            if (string.IsNullOrWhiteSpace(serviceUrl.Name))
                return _resultFact.Result("", false, "Remote Service name is missing !");
            if (string.IsNullOrWhiteSpace(pathName))
                return _resultFact.Result("", false, $"Path for Remote Service '{serviceUrl.Name}' was NOT provided !");
            if (string.IsNullOrWhiteSpace(serviceUrl.GetPathByName(TypeOfService.REST, pathName)))
                return _resultFact.Result("", false, $"Path '{pathName}' for Remote Service '{serviceUrl.Name}' NOT found !");

            var urlWithPath = serviceUrl.GetUrlWithPath(TypeOfService.REST, pathName, _isProdEnv);

            if (string.IsNullOrWhiteSpace(urlWithPath))
                return _resultFact.Result("", false, $"Path: '{pathName}' for Remote Service: '{serviceUrl.Name}' NOT found !");

            return _resultFact.Result(urlWithPath, true);
        }




        // For ALL services !
        // UPDATE Remote Services URLs by GET response from Management service:
        public async Task<IServiceResult<IEnumerable<Service_Model_AS>>> LoadServiceModels()
        {
            var result = await _httpManagementService.GetAllRemoteServices();

            if (!result.Status || result.Data.IsNullOrEmpty())
                return _resultFact.Result<IEnumerable<Service_Model_AS>>(null, false, $"Remote Services URLs was NOT received from Management service ! Reason: {result.Message}");

            UpdateServiceModels(result.Data);

            return result;
        }



        // For MANAGEMENT service !
        // UPDATE Remote Services URLs by incoming PUT request from Management API service:

        // WARNING: in K8 only one replica (NOT all) of this service will be chosen for PUT request by load balancer!
        // Individual GET request from all replicas is prefered after handling HTTP 503 as a result of old/faulty URL.
        public IServiceResult<IEnumerable<Service_Model_AS>> UpdateServiceModels(IEnumerable<Service_Model_AS> servicesModels)
        {
            Console.WriteLine($"--> UPDATING Services URLs'......");

            if (servicesModels.IsNullOrEmpty())
                return _resultFact.Result<IEnumerable<Service_Model_AS>>(null, false, $"Services URLs for Management service was NOT provided !");

            var message = "Warning ! There were missing data in provided Services URLs:";

            foreach (var model in servicesModels)
            {
                var baseUrl = model.GetBaseUrl(TypeOfService.REST, _isProdEnv);

                if (string.IsNullOrWhiteSpace(model.Name) || string.IsNullOrWhiteSpace(baseUrl) || model.Type.IsNullOrEmpty())
                    message += $"n\\SERVICE: Name: '{model.Name}', Base URL: '{baseUrl}', Type: '{model.Type}'";

                foreach (var path in model.GetPaths(TypeOfService.REST))
                {
                    if (string.IsNullOrWhiteSpace(path.Name) || string.IsNullOrWhiteSpace(path.Route))
                        message += $"n\\ - PATH: Name: '{path.Name}', Route: '{path.Route}'";
                }
            }

            _remoteServicesInfo_Repo.InitializeDB(servicesModels.ToList());

            return _resultFact.Result(servicesModels, true, message);
        }
    }
}
