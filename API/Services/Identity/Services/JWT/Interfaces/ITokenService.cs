using Business.Libraries.ServiceResult.Interfaces;
using Services.Identity.Models;

namespace Identity.Services.JWT.Interfaces
{
    public interface ITokenService
    {
        Task<IServiceResult<string>> CreateToken_ForService();
        Task<IServiceResult<string>> CreateToken_ForUser(AppUser user);
    }
}
