using Business.Libraries.ServiceResult.Interfaces;
using Business.Management.Appsettings.Models;

namespace Business.Management.Http.Services.Interfaces
{
    public interface IHttpManagementService
    {
        Task<IServiceResult<IEnumerable<RemoteService_AS_MODEL>>> GetAllRemoteServices();
        Task<IServiceResult<Config_Global_AS_MODEL>> GetGlobalConfig();
    }
}
