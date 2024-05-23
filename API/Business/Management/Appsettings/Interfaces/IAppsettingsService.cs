using Business.Libraries.ServiceResult.Interfaces;
using Business.Management.Appsettings.Models;

namespace Business.Management.Appsettings.Interfaces
{
    public interface IAppsettingsService
    {
        IServiceResult<IEnumerable<ServiceURL_AS>> GetAllRemoteServicesURL();
        IServiceResult<string> GetApiKey();
        IServiceResult<ServiceURL_AS> GetRemoteServiceURL(string name);
    }
}
