using Business.Libraries.ServiceResult.Interfaces;
using Business.Management.Appsettings.Models;

namespace Business.Management.Appsettings.Interfaces
{
    public interface IAppsettings_Provider
    {
        IServiceResult<ICollection<Service_Model_AS>> GetAllRemoteServicesModels();
        IServiceResult<string> GetApiKey();
        IServiceResult<Service_Model_AS> GetRemoteServiceModel(string name);
    }
}
