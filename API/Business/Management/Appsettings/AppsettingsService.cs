﻿using Business.Libraries.ServiceResult.Interfaces;
using Business.Management.Appsettings.Interfaces;
using Business.Management.Appsettings.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;



namespace Business.Management.Appsettings
{
    //________ Singleton________
    public class AppsettingsService : IAppsettingsService
    {

        private IOptionsMonitor<Config_Global_Model_AS> _config_global;
        private readonly IServiceScopeFactory _serviceFactory;


        public AppsettingsService(IOptionsMonitor<Config_Global_Model_AS> config_global, IServiceScopeFactory serviceFactory)
        {
            _config_global = config_global;
            _serviceFactory = serviceFactory;
        }




        public IServiceResult<IEnumerable<Service_Model_AS>> GetAllRemoteServicesModels()
        {
           // create scope of IServiceResultFactory inside this singleton:
            using (var scope = _serviceFactory.CreateScope())
            {
                var resultFact = scope.ServiceProvider.GetService<IServiceResultFactory>();

                var configResult = _config_global.CurrentValue.RemoteServices;
                if(configResult.IsNullOrEmpty())
                    return resultFact.Result(configResult, false, $"Global config data were NOT found in Appsettings !");

                return resultFact.Result(configResult, true);
            }
        }


        public IServiceResult<Service_Model_AS> GetRemoteServiceModel(string name)
        {
            // create scope of IServiceResultFactory inside this singleton:
            using (var scope = _serviceFactory.CreateScope())
            {
                var resultFact = scope.ServiceProvider.GetService<IServiceResultFactory>();

                var configResult = _config_global.CurrentValue.RemoteServices;

                if (configResult.IsNullOrEmpty())
                    return resultFact.Result<Service_Model_AS>(null, false, $"Global config data were NOT found in Appsettings !");

                var model = configResult.FirstOrDefault(url => url.Name == name);

                if (model == null)
                {
                    return resultFact.Result(model, false, $"URL for service '{name}' NOT found in Appsettings !");
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


    }
}
