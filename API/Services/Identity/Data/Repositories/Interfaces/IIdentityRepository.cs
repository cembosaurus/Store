using Business.Data.Repositories.Interfaces;
using Services.Identity.Models;


namespace Services.Identity.Data.Repositories.Interfaces
{
    public interface IIdentityRepository : IBaseRepository
    {
        Task<AppUser> Register(AppUser user, string password);
        Task<AppUser> Login(string username, string password);
        Task<bool> UserExist(string username);
    }
}
