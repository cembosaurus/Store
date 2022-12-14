using Microsoft.EntityFrameworkCore;
using Services.Identity.Data.Repositories.Interfaces;
using Services.Identity.Models;
using System.Security.Cryptography;
using System.Text;

namespace Services.Identity.Data.Repositories
{
    public class IdentityRepository : BaseRepository, IIdentityRepository
    {
        private readonly IdentityContext _context;

        public IdentityRepository(IdentityContext context) : base(context)
        {
            _context = context;
        }

        public async Task<AppUser> Login(string username, string password)
        {
            var userInDb = await _context.Users.FirstOrDefaultAsync(u => u.UserName == username);

            if (userInDb == null)
                return null;


            return userInDb;
        }




        public async Task<AppUser> Register(AppUser user, string password)
        {
            await _context.Users.AddAsync(user);

            return user;
        }



        public async Task<bool> UserExist(string username)
        {
            if (await _context.Users.AnyAsync(u => u.UserName == username))
                return true;

            return false;
        }


    }
}
