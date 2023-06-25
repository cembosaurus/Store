using Business.Identity.DTOs;
using Business.Libraries.ServiceResult.Interfaces;

namespace API_Gateway.HttpServices.Identity.Interfaces
{
    public interface IHttpIdentityService
    {
        Task<IServiceResult<string>> AuthenticateService(string apiKey);
        Task<IServiceResult<string>> Login(UserToLoginDTO user);
        Task<IServiceResult<UserAuthDTO>> Register(UserToRegisterDTO user);
    }
}
