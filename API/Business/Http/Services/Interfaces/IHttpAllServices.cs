using Business.Libraries.ServiceResult.Interfaces;
using Business.Management.Appsettings.DTOs;
using Business.Management.Appsettings.Models;

namespace Business.Http.Services.Interfaces
{
    public interface IHttpAllServices
    {
        Task<IServiceResult<Config_Global_AS_DTO>> PostGlobalConfig(Config_Global_AS_DTO globalConfig_Model);
        Task<IServiceResult<IEnumerable<KeyValuePair<RemoteService_AS_MODEL, bool>>>> PostGlobalConfigToMultipleServices(bool allowRequestToManagementAPIService);
    }
}
