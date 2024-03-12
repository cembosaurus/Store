using Business.Libraries.ServiceResult.Interfaces;

namespace API_Gateway.Services.Management.Interfaces
{
    public interface IRemoteServices
    {
        IServiceResult<IEnumerable<IConfigurationSection>> GetAllRemoteServicesInfo();
    }
}
