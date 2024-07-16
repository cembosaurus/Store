using Business.Libraries.ServiceResult.Interfaces;
using Business.Management.Appsettings.DTOs;


namespace Business.Management.Http.Services.Interfaces
{
    public interface IHttpManagementService
    {
        Task<IServiceResult<RemoteService_AS_DTO>> AddRemoteService(RemoteService_AS_DTO service);
        Task<IServiceResult<IEnumerable<RemoteService_AS_DTO>>> GetAllRemoteServices();
        Task<IServiceResult<Config_Global_AS_DTO>> GetGlobalConfig();
    }
}
