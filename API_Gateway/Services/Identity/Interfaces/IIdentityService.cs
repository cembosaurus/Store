using Business.Identity.DTOs;
using Business.Libraries.ServiceResult.Interfaces;

namespace API_Gateway.Services.Identity.Interfaces
{
    public interface IIdentityService
    {
        Task<IServiceResult<string>> AuthenticateService(string apiKey);
        Task<IServiceResult<UserAuthDTO>> Login(UserToLoginDTO user);
        Task<IServiceResult<UserAuthDTO>> Register(UserToRegisterDTO user);
    }
}
