using Business.Libraries.ServiceResult.Interfaces;
using Business.Management.Appsettings.Interfaces;
using Business.Management.Appsettings.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;



namespace Business.Management.Appsettings
{
    public class AppsettingsService : IAppsettingsService
    {

        private IOptionsMonitor<List<ServiceURL>> _monitorServicesURLs;
        private readonly IServiceScopeFactory _serviceFactory;


        public AppsettingsService(IOptionsMonitor<List<ServiceURL>> monitorServicesURLs, IServiceScopeFactory serviceFactory)
        {
            _monitorServicesURLs = monitorServicesURLs;
            _serviceFactory = serviceFactory;
        }




        public IServiceResult<IEnumerable<ServiceURL>> GetAllRemoteServicesURL()
        {
            var urlResult = _monitorServicesURLs.CurrentValue;


            // create scope of IServiceResultFactory inside this singleton:
            using (var scope = _serviceFactory.CreateScope())
            {
                var resultFact = scope.ServiceProvider.GetService<IServiceResultFactory>();

                return resultFact.Result<IEnumerable<ServiceURL>>(urlResult, true);
            }
        }


        public IServiceResult<ServiceURL> GetRemoteServiceURL(string name)
        {
            var urlResult = _monitorServicesURLs.CurrentValue;

            var url = urlResult.FirstOrDefault(url => url.Name == name);

            // create scope of IServiceResultFactory inside this singleton:
            using (var scope = _serviceFactory.CreateScope())
            {
                var resultFact = scope.ServiceProvider.GetService<IServiceResultFactory>();

                if(url == null)
                {
                    return resultFact.Result(url, false, $"URL foro service '{name}' NOT found in Appsettings !");
                }
                
                return resultFact.Result(url, true);
            }
        }


    }
}
