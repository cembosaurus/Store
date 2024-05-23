using Business.Libraries.ServiceResult.Interfaces;
using Business.Management.Appsettings.Interfaces;
using Business.Management.Appsettings.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;



namespace Business.Management.Appsettings
{
    public class AppsettingsService : IAppsettingsService
    {

        private IOptionsMonitor<Config_Global> _config_global;
        private readonly IServiceScopeFactory _serviceFactory;


        public AppsettingsService(IOptionsMonitor<Config_Global> config_global, IServiceScopeFactory serviceFactory)
        {
            _config_global = config_global;
            _serviceFactory = serviceFactory;
        }




        public IServiceResult<IEnumerable<ServiceURL_AS>> GetAllRemoteServicesURL()
        {
            var urlResult = _config_global.CurrentValue.RemoteServices;


            // create scope of IServiceResultFactory inside this singleton:
            using (var scope = _serviceFactory.CreateScope())
            {
                var resultFact = scope.ServiceProvider.GetService<IServiceResultFactory>();

                return resultFact.Result(urlResult, true);
            }
        }


        public IServiceResult<ServiceURL_AS> GetRemoteServiceURL(string name)
        {
            var urlResult = _config_global.CurrentValue.RemoteServices;

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



        public IServiceResult<string> GetApiKey()
        {
            var apiKey = _config_global.CurrentValue.Auth.ApiKey;


            // create scope of IServiceResultFactory inside this singleton:
            using (var scope = _serviceFactory.CreateScope())
            {
                var resultFact = scope.ServiceProvider.GetService<IServiceResultFactory>();

                if (string.IsNullOrWhiteSpace(apiKey))
                {
                    return resultFact.Result("", false, $"API-Key was NOT found in Appsettings !");
                }

                return resultFact.Result(apiKey, true);
            }
        }


    }
}
