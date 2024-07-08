using Business.Libraries.ServiceResult.Interfaces;
using Business.Management.Appsettings.DTOs;
using Business.Management.Appsettings.Models;
using Business.Management.Models;

namespace Business.Management.Http.Services.Interfaces
{
    public interface IHttpManagementService
    {
        Task<IServiceResult<RemoteService_AS_DTO>> AddRemoteService(RemoteService_AS_DTO service);
        Task<IServiceResult<IEnumerable<RemoteService_AS_DTO>>> GetAllRemoteServices();
        Task<IServiceResult<RemoteService_AS_DTO>> GetGlobalConfig();
        Task<IServiceResult<RemoteService_AS_DTO>> PostRemoteServiceID(ServiceID_MODEL serviceID);
    }
}
