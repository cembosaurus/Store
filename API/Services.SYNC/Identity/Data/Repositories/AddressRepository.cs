using Business.Data.Repositories;
using Business.Identity.Models;
using Identity.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Services.Identity.Data;
using Services.Identity.Data.Repositories;
using Services.Identity.Models;

namespace Identity.Data.Repositories
{
    public class AddressRepository : BaseRepository<IdentityContext>, IAddressRepository
    {


        private readonly IdentityContext _context;


        public AddressRepository(IdentityContext context) : base(context)
        {
            _context = context;
        }



        public override int SaveChanges()
        {
            return _context.SaveChanges();
        }




        public async Task<IEnumerable<Address>> GetAllAddresses()
        {
            return await _context.Address.ToListAsync();
        }


        public async Task<Address> GetAddressById(int id)
        {
            return await _context.Address.FirstOrDefaultAsync(a => a.AddressId == id);
        }


        public async Task<IEnumerable<Address>> GetAddressesByIds(IEnumerable<int> ids)
        {
            return await _context.Address.Where(a => ids.Contains(a.AddressId)).ToListAsync();
        }



        public async Task<IEnumerable<Address>> GetAddressesByUserId(int userId)
        {
            return await _context.Address.Where(a => a.UserAddresses.Select(ua => ua.UserId).Contains(userId)).ToListAsync();
        }


        public async Task<EntityState> CreateAddress(Address address)
        {
            var result = await _context.Address.AddAsync(address);

            return result.State;
        }


        public async Task<IEnumerable<Address>> SearchAddress(SearchAddressModel searchModel)
        {
            // To Do: ..............................................................

            return null;
        }


        public async Task<EntityState> DeleteAddress(Address address)
        {
            return _context.Address.Remove(address).State;
        }

        public async Task<bool> ExistsById(int id)
        {
            return await _context.Address.AnyAsync(a => a.AddressId == id);
        }


    }
}
