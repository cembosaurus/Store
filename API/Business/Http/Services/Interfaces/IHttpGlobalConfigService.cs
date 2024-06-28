using Business.Libraries.ServiceResult.Interfaces;
using Business.Management.Appsettings.Models;

namespace Business.Http.Services.Interfaces
{
    public interface IHttpGlobalConfigService
    {
        Task<IServiceResult<IEnumerable<RemoteService_AS_MODEL>>> PostGlobalConfig(Config_Global_AS_MODEL globalConfig_Model);
        Task<IServiceResult<IEnumerable<RemoteService_AS_MODEL>>> PostGlobalConfigToMultipleServices();
    }
}
