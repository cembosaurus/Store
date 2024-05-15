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






        public IServiceResult<ServiceURL> GetServiceURLByName(string name) 
        {
            if(name.IsNullOrEmpty())
                return _resultFact.Result<ServiceURL>(null, false, "Remote Service name was NOT provided !");
            if(_remoteServicesInfo_Repo.GetAllURLs().IsNullOrEmpty())
                return _resultFact.Result<ServiceURL>(null, false, $"Local URLs DB is empty ! n\\ Possible solution: Get URL from Management service.");

            var serviceURL = _remoteServicesInfo_Repo.GetByName(name);

            if (serviceURL == null)
                return _resultFact.Result<ServiceURL>(null, false, $"Service '{name}' was NOT found ! n\\ Possible solution: Get URL from Management service.");
            if (serviceURL.GetUrl(TypeOfService.REST, _isProdEnv).IsNullOrEmpty())
                return _resultFact.Result<ServiceURL>(null, false, $"Service '{name}' was found but there is missing Base URL !");

            return _resultFact.Result(serviceURL, true);
        }


        public IServiceResult<ServiceURL> GetServiceURLByBaseURL(string baseURL)
        {
            if (baseURL.IsNullOrEmpty())
                return _resultFact.Result<ServiceURL>(null, false, "Remote Service Base URL was NOT provided !");
            if (_remoteServicesInfo_Repo.GetAllURLs().IsNullOrEmpty())
                return _resultFact.Result<ServiceURL>(null, false, $"Local URLs DB is empty ! n\\ Possible solution: Get URL from Management service.");

            var serviceURL = _remoteServicesInfo_Repo.GetByBaseURL(baseURL);

            if (serviceURL == null)
                return _resultFact.Result(serviceURL, false, $"Service with Base URL '{baseURL}' was NOT found !");

            return _resultFact.Result(serviceURL, true);
        }



        public IServiceResult<string> BuildServiceURL(string serviceName, string pathName)
        {
            if (serviceName.IsNullOrEmpty())
                return _resultFact.Result("", false, "Remote Service name was NOT provided !");
            if (pathName.IsNullOrEmpty())
                return _resultFact.Result("", false, $"Path for Remote Service '{serviceName}' was NOT provided !");

            var urlResult = GetServiceURLByName(serviceName);

            if(!urlResult.Status)
                return _resultFact.Result("", false, urlResult.Message);

            var url = urlResult.Data.GetUrl(TypeOfService.REST, _isProdEnv);
            var path = urlResult.Data.GetPathByName(pathName, TypeOfService.REST);

            if (path == null || path.IsNullOrEmpty())
                return _resultFact.Result("", false, $"Path: '{pathName}' for Remote Service: '{serviceName}' NOT found !");
            
            return _resultFact.Result(url += "/" + path, true);
        }



        public IServiceResult<string> BuildServiceURL(ServiceURL serviceUrl, string pathName)
        {
            if (serviceUrl.Name.IsNullOrEmpty())
                return _resultFact.Result("", false, "Remote Service name is missing !");
            if (pathName.IsNullOrEmpty())
                return _resultFact.Result("", false, $"Path for Remote Service '{serviceUrl.Name}' was NOT provided !");
            if (serviceUrl.GetPathByName(pathName, TypeOfService.REST).IsNullOrEmpty())
                return _resultFact.Result("", false, $"Path '{pathName}' for Remote Service '{serviceUrl.Name}' NOT found !");

            var url = serviceUrl.GetUrl(TypeOfService.REST, _isProdEnv);
            var path = serviceUrl.GetPathByName(pathName, TypeOfService.REST);

            if (path == null || path.IsNullOrEmpty())
                return _resultFact.Result("", false, $"Path: '{pathName}' for Remote Service: '{serviceUrl.Name}' NOT found !");

            return _resultFact.Result(url += "/" + path, true);
        }




        // For ALL services
        // UPDATE Remote Services URLs by GET response from Management service:
        public async Task<IServiceResult<IEnumerable<ServiceURL>>> LoadAllRemoteServicesURL()
        {
            var result = await _httpManagementService.GetAllRemoteServicesURLs();

            if (!result.Status || result.Data.IsNullOrEmpty())
                return _resultFact.Result<IEnumerable<ServiceURL>>(null, false, $"Remote Services URLs was NOT received from Management service ! Reason: {result.Message}");

            InitializeRemoteServicesURLs(result.Data);

            return result;
        }



        // For MANAGEMENT service !
        // UPDATE Remote Services URLs by PUT request from Management service:

        // WARNING: in K8 only one replica (NOT all) of this service will be chosen for PUT request by load balancer!
        // Individual GET request from all replicas is prefered after handling HTTP 503 as a result of old/faulty URL.
        public IServiceResult<IEnumerable<ServiceURL>> InitializeRemoteServicesURLs(IEnumerable<ServiceURL> servicesURLs)
        {
            Console.WriteLine($"--> UPDATING Services URLs'......");

            if (servicesURLs.IsNullOrEmpty())
                return _resultFact.Result<IEnumerable<ServiceURL>>(null, false, $"Services URLs for Management service was NOT provided !");

            var message = "Warning ! There were missing data in provided Services URLs:";

            foreach (var url in servicesURLs)
            {
                var baseUrl = url.GetUrl(TypeOfService.REST, _isProdEnv);

                if (url.Name.IsNullOrEmpty() || baseUrl.IsNullOrEmpty() || url.Type.IsNullOrEmpty())
                    message += $"n\\SERVICE: Name: '{url.Name}', Base URL: '{baseUrl}', Type: '{url.Type}'";

                foreach (var path in url.GetPaths(TypeOfService.REST))
                {
                    if (path.Name.IsNullOrEmpty() || path.Route.IsNullOrEmpty())
                        message += $"n\\ - PATH: Name: '{path.Name}', Route: '{path.Route}'";
                }
            }

            _remoteServicesInfo_Repo.InitializeDB(servicesURLs.ToList());

            return _resultFact.Result(servicesURLs, true, message);
        }
    }
}
