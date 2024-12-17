using Business.Libraries.ServiceResult.Interfaces;
using Services.Identity.Models;

namespace Identity.JWT.Interfaces
{
    public interface IJWT_Provider
    {
        IServiceResult<string> CreateToken_ForService();
        Task<IServiceResult<string>> CreateToken_ForUser(AppUser user);
    }
}
