using Business.Libraries.ServiceResult.Interfaces;
using Business.Management.Appsettings.Models;

namespace Business.Management.Appsettings.Interfaces
{
    public interface IAppsettings_PROVIDER
    {
        IServiceResult<Config_Global_MODEL_AS> GetGlobalConfig();
        IServiceResult<IEnumerable<RemoteService_MODEL_AS>> GetAllRemoteServicesModels();
        IServiceResult<string> GetApiKey();
        IServiceResult<string> GetJWTKey();
        IServiceResult<RabbitMQ_MODEL_AS> GetRabbitMQ();
        IServiceResult<RemoteService_MODEL_AS> GetRemoteServiceModel(string name);
    }
}
