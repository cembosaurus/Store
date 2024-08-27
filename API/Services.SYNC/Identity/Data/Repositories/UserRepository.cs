using Business.Data.Repositories;
using Identity.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Services.Identity.Data;
using Services.Identity.Models;

namespace Identity.Data.Repositories
{
    public class UserRepository : BaseRepository<IdentityContext>, IUserRepository
    {

        private readonly IdentityContext _context;

        public UserRepository(IdentityContext context) : base(context)
        {
            _context = context;
        }




        public override int SaveChanges()
        {
            return _context.SaveChanges();
        }




        public async Task<IEnumerable<AppUser>> GetAllUsers()
        {
            return await _context.Users.Include(u => u.CurrentUsersAddress).ToListAsync();
        }



        public async Task<AppUser> GetUserById(int id)
        {
            return await _context.Users.Include(u => u.CurrentUsersAddress).Include(u => u.UserAddresses).FirstOrDefaultAsync(u => u.Id == id);
        }



        public async Task<AppUser> GetUserByName(string name)
        {
            return await _context.Users.Include(u => u.CurrentUsersAddress).Include(u => u.UserAddresses).FirstOrDefaultAsync(u => u.UserName == name.ToLower());
        }

    }
}
