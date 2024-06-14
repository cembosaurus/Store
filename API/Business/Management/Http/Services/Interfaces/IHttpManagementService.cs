using Business.Libraries.ServiceResult.Interfaces;
using Business.Management.Appsettings.Models;

namespace Business.Management.Http.Services.Interfaces
{
    public interface IHttpManagementService
    {
        Task<IServiceResult<IEnumerable<RemoteService_MODEL_AS>>> GetAllRemoteServices();
    }
}
