using Business.Libraries.ServiceResult.Interfaces;
using Business.Management.Appsettings.Models;

namespace Business.Management.Appsettings.Interfaces
{
    public interface IAppsettings_PROVIDER
    {
        IServiceResult<IEnumerable<RemoteService_AS_MODEL>> GetAllRemoteServicesModels();
        IServiceResult<string> GetApiKey();
        IServiceResult<Config_Global_AS_MODEL> GetGlobalConfig();
        IServiceResult<string> GetJWTKey();
        IServiceResult<Persistence_AS_MODEL> GetPersistence();
        IServiceResult<RabbitMQ_AS_MODEL> GetRabbitMQ();
        IServiceResult<RemoteService_AS_MODEL> GetRemoteServiceModel(string name);
    }
}
