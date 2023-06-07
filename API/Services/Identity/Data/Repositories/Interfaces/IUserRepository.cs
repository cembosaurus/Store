using Business.Data.Repositories.Interfaces;
using Services.Identity.Models;

namespace Identity.Data.Repositories.Interfaces
{
    public interface IUserRepository: IBaseRepository
    {
        Task<IEnumerable<AppUser>> GetAllUsers();
        Task<AppUser> GetUserById(int id);
        Task<AppUser> GetUserByName(string name);
    }
}
