using Business.Libraries.ServiceResult.Interfaces;
using Business.Management.Appsettings.Interfaces;
using Business.Management.Appsettings.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;



namespace Business.Management.Appsettings
{
    //________ Singleton________
    public class Appsettings_PROVIDER : IAppsettings_PROVIDER
    {

        private IOptionsMonitor<Config_Global_MODEL_AS> _config_global;
        private readonly IServiceScopeFactory _serviceFactory;


        public Appsettings_PROVIDER(IOptionsMonitor<Config_Global_MODEL_AS> config_global, IServiceScopeFactory serviceFactory)
        {
            _config_global = config_global;
            _serviceFactory = serviceFactory;
        }




        public IServiceResult<IEnumerable<RemoteService_MODEL_AS>> GetAllRemoteServicesModels()
        {
           // create scope of IServiceResultFactory inside this singleton:
            using (var scope = _serviceFactory.CreateScope())
            {
                var resultFact = scope.ServiceProvider.GetService<IServiceResultFactory>();

                var configResult = _config_global.CurrentValue.RemoteServices;
                if(configResult.IsNullOrEmpty())
                    return resultFact.Result<IEnumerable<RemoteService_MODEL_AS>>(configResult, false, $"Global config data were NOT found in Appsettings !");

                return resultFact.Result<IEnumerable<RemoteService_MODEL_AS>>(configResult, true);
            }
        }


        public IServiceResult<RemoteService_MODEL_AS> GetRemoteServiceModel(string name)
        {
            // create scope of IServiceResultFactory inside this singleton:
            using (var scope = _serviceFactory.CreateScope())
            {
                var resultFact = scope.ServiceProvider.GetService<IServiceResultFactory>();

                var configResult = _config_global.CurrentValue.RemoteServices;

                if (configResult.IsNullOrEmpty())
                    return resultFact.Result<RemoteService_MODEL_AS>(null, false, $"Global config data were NOT found in Appsettings !");

                var model = configResult.FirstOrDefault(url => url.Name == name);

                if (model == null)
                {
                    return resultFact.Result(model, false, $"URL for service '{name}' was NOT found in Appsettings !");
                }
                
                return resultFact.Result(model, true);
            }
        }



        public IServiceResult<string> GetApiKey()
        {
            // create scope of IServiceResultFactory inside this singleton:
            using (var scope = _serviceFactory.CreateScope())
            {
                var resultFact = scope.ServiceProvider.GetService<IServiceResultFactory>();

                var apiKey = _config_global.CurrentValue.Auth.ApiKey;

                if (string.IsNullOrWhiteSpace(apiKey))
                {
                    return resultFact.Result("", false, $"API-Key was NOT found in Appsettings !");
                }

                return resultFact.Result(apiKey, true);
            }
        }



        public IServiceResult<string> GetJWTKey()
        {
            // create scope of IServiceResultFactory inside this singleton:
            using (var scope = _serviceFactory.CreateScope())
            {
                var resultFact = scope.ServiceProvider.GetService<IServiceResultFactory>();

                var JWTKey = _config_global.CurrentValue.Auth.JWTKey;

                if (string.IsNullOrWhiteSpace(JWTKey))
                {
                    return resultFact.Result("", false, $"JWT-Key was NOT found in Appsettings !");
                }

                return resultFact.Result(JWTKey, true);
            }
        }



        public IServiceResult<RabbitMQ_MODEL_AS> GetRabbitMQ()
        {
            // create scope of IServiceResultFactory inside this singleton:
            using (var scope = _serviceFactory.CreateScope())
            {
                var resultFact = scope.ServiceProvider.GetService<IServiceResultFactory>();

                var host = _config_global.CurrentValue.RabbitMQ.Host;
                var port = _config_global.CurrentValue.RabbitMQ.Port;

                if (string.IsNullOrWhiteSpace(host) || string.IsNullOrWhiteSpace(port))
                {
                    return resultFact.Result<RabbitMQ_MODEL_AS>(null, false, $"Missing or incomplete RabbitMQ config data were found in Appsettings ! Host: '{host}', Port: '{port}'");
                }

                return resultFact.Result(new RabbitMQ_MODEL_AS { Host = host, Port = port}, true);
            }
        }


    }
}
