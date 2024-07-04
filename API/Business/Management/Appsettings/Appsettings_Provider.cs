﻿using Business.Libraries.ServiceResult.Interfaces;
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


        private IOptionsMonitor<Config_Global_AS_MODEL> _appsettings_monitor;
        private readonly IServiceScopeFactory _serviceFactory;



        public Appsettings_PROVIDER(IOptionsMonitor<Config_Global_AS_MODEL> appsettings_monitor, IServiceScopeFactory serviceFactory)
        {
            _appsettings_monitor = appsettings_monitor;
            _serviceFactory = serviceFactory;
        }






        // ------------------------------------------------------------------------------------ READ: ------------------------------------------------------------------------------


        public IServiceResult<IEnumerable<RemoteService_AS_MODEL>> GetAllRemoteServicesModels()
        {
            using (var scope = _serviceFactory.CreateScope())
            {
                var resultFact = scope.ServiceProvider.GetService<IServiceResultFactory>();

                var remoteServices = _appsettings_monitor.CurrentValue.RemoteServices;

                return resultFact.Result<IEnumerable<RemoteService_AS_MODEL>>(remoteServices, !remoteServices.IsNullOrEmpty(), remoteServices.IsNullOrEmpty() 
                    ? $"Remote Service models were NOT found in Global Appsettings !" : "");
            }
        }


        public IServiceResult<RemoteService_AS_MODEL> GetRemoteServiceModel(string name)
        {
            using (var scope = _serviceFactory.CreateScope())
            {
                var resultFact = scope.ServiceProvider.GetService<IServiceResultFactory>();

                var remoteServices = _appsettings_monitor.CurrentValue.RemoteServices;

                if (remoteServices.IsNullOrEmpty())
                    return resultFact.Result<RemoteService_AS_MODEL>(null, false, $"Global config data were NOT found in Appsettings !");

                var model = remoteServices.FirstOrDefault(s => s.Name == name);
                
                return resultFact.Result(model, model != null, model == null ? $"URL for service '{name}' was NOT found in Appsettings !" : "");
            }
        }




        public IServiceResult<string> GetApiKey()
        {
            using (var scope = _serviceFactory.CreateScope())
            {
                var resultFact = scope.ServiceProvider.GetService<IServiceResultFactory>();

                var apiKey = _appsettings_monitor.CurrentValue.Auth.ApiKey;

                return resultFact.Result(apiKey, !string.IsNullOrWhiteSpace(apiKey), string.IsNullOrWhiteSpace(apiKey) ? "API-Key was NOT found in Appsettings !" : "");
            }
        }



        public IServiceResult<string> GetJWTKey()
        {
            using (var scope = _serviceFactory.CreateScope())
            {
                var resultFact = scope.ServiceProvider.GetService<IServiceResultFactory>();

                var JWTKey = _appsettings_monitor.CurrentValue.Auth.JWTKey;

                return resultFact.Result(JWTKey, !string.IsNullOrWhiteSpace(JWTKey), string.IsNullOrWhiteSpace(JWTKey) ? $"JWT-Key was NOT found in Appsettings !" : "");
            }
        }



        public IServiceResult<RabbitMQ_AS_MODEL> GetRabbitMQ()
        {
            using (var scope = _serviceFactory.CreateScope())
            {
                var resultFact = scope.ServiceProvider.GetService<IServiceResultFactory>();

                var rabbitMQ = _appsettings_monitor.CurrentValue.RabbitMQ;
                
                return resultFact.Result(rabbitMQ, rabbitMQ != null, rabbitMQ == null ? $"RabbitMQ data were NOT found in Appsettings !" : "");
            }
        }



        public IServiceResult<Config_Global_AS_MODEL> GetGlobalConfig()
        {
            using (var scope = _serviceFactory.CreateScope())
            {
                var resultFact = scope.ServiceProvider.GetService<IServiceResultFactory>();

                var globalConfig = _appsettings_monitor.CurrentValue;

                return resultFact.Result(globalConfig, globalConfig != null, globalConfig == null ? $"Global Config data were NOT found in Appsettings !" : "");
            }
        }


    }
}
