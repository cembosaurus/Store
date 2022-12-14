using Business.Identity.DTOs;
using Business.Libraries.ServiceResult.Interfaces;

namespace Identity.Services.Interfaces
{
    public interface IIdentityService
    {
        Task<IServiceResult<UserAuthDTO>> Register(UserToRegisterDTO userToRegister);
        Task<IServiceResult<UserAuthDTO>> Login(UserToLoginDTO user);
        Task AddDefaultUsers();
        Task AddRoles();
        Task<IServiceResult<string>> CreateTokenForService();
    }
}
