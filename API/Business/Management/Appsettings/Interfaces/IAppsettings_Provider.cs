using Business.Libraries.ServiceResult.Interfaces;
using Business.Management.Appsettings.Models;

namespace Business.Management.Appsettings.Interfaces
{
    public interface IAppsettings_PROVIDER
    {
        IServiceResult<IEnumerable<RemoteService_MODEL_AS>> GetAllRemoteServicesModels();
        IServiceResult<string> GetApiKey();
        IServiceResult<RemoteService_MODEL_AS> GetRemoteServiceModel(string name);
    }
}
