﻿using Business.Libraries.ServiceResult.Interfaces;
using Business.Management.Appsettings.Models;

namespace Business.Management.Services.Interfaces
{
    public interface IGlobalConfig_PROVIDER
    {
        Task<IServiceResult<Config_Global_AS_MODEL>> DownloadGlobalConfig();
        Task<IServiceResult<IEnumerable<RemoteService_AS_MODEL>>> DownloadRemoteServicesModels();
        IServiceResult<string> GetApiKey();
        IServiceResult<Config_Global_AS_MODEL> GetGlobalConfig();
        IServiceResult<string> GetJWTKey();
        IServiceResult<Persistence_AS_MODEL> GetPersistence();
        IServiceResult<RabbitMQ_AS_MODEL> GetRabbitMQ();
        IServiceResult<RemoteService_AS_MODEL> GetRemoteServiceByBaseURL(string baseURL);
        IServiceResult<RemoteService_AS_MODEL> GetRemoteServiceByName(string name);
        IServiceResult<IEnumerable<RemoteService_AS_MODEL>> GetRemoteServices_WithGlobalConfig();
        IServiceResult<string> GetRemoteServiceURL_WithPath(RemoteService_AS_MODEL serviceModel, string pathName);
        IServiceResult<string> GetRemoteServiceURL_WithPath(string serviceName, string pathName);
        IServiceResult<bool> IsEmpty_RemoteServiceModels();
        IServiceResult<Config_Global_AS_MODEL> Update(Config_Global_AS_MODEL config);
        IServiceResult<IEnumerable<RemoteService_AS_MODEL>> UpdateRemoteServiceModels(IEnumerable<RemoteService_AS_MODEL> servicesModels);
    }
}
