using Business.Identity.DTOs;
using Business.Libraries.ServiceResult.Interfaces;

namespace Scheduler.HttpServices.Interfaces
{
    public interface IHttpIdentityService
    {
        Task<IServiceResult<UserAuthDTO>> Login(UserToLoginDTO user);
        Task<IServiceResult<UserAuthDTO>> Register(UserToRegisterDTO user);
    }
}
