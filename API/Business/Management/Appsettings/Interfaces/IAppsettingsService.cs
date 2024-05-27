using Business.Libraries.ServiceResult.Interfaces;
using Business.Management.Appsettings.Models;

namespace Business.Management.Appsettings.Interfaces
{
    public interface IAppsettingsService
    {
        IServiceResult<IEnumerable<Service_Model_AS>> GetAllRemoteServicesURL();
        IServiceResult<string> GetApiKey();
        IServiceResult<Service_Model_AS> GetRemoteServiceURL(string name);
    }
}
