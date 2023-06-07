using Business.Libraries.ServiceResult.Interfaces;
using Services.Identity.Models;

namespace Identity.Services.JWT.Interfaces
{
    public interface ITokenService
    {
        Task<IServiceResult<string>> CreateTokenForService();
        Task<IServiceResult<string>> CreateTokenForUser(AppUser user);
    }
}
