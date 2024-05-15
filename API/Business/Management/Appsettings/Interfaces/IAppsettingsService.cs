using Business.Libraries.ServiceResult.Interfaces;
using Business.Management.Appsettings.Models;

namespace Business.Management.Appsettings.Interfaces
{
    public interface IAppsettingsService
    {
        IServiceResult<IEnumerable<ServiceURL>> GetAllRemoteServicesURL();
        IServiceResult<ServiceURL> GetRemoteServiceURL(string name);
    }
}
