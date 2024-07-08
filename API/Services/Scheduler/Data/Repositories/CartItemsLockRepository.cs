using Business.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Scheduler.Data.Repositories.Interfaces;
using Scheduler.Models;



namespace Scheduler.Data.Repositories
{
    public class CartItemLockRepository : BaseRepository<SchedulerDBContext>, ICartItemLockRepository
    {

        private readonly SchedulerDBContext _context;


        public CartItemLockRepository(SchedulerDBContext context) : base(context)
        {
            _context = context;
        }



        public override int SaveChanges()
        {
            return _context.SaveChanges();
        }




        public async Task<IEnumerable<CartItemLock>> GetAllCartItemLocks()
        {
            return await _context.CartItemLocks.ToListAsync();
        }



        public async Task<CartItemLock?> GetCartItemLock(Guid cartId, int itemId)
        {
            return await _context.CartItemLocks.FirstOrDefaultAsync(cil => cil.CartId == cartId && cil.ItemId == itemId);
        }



        public async Task<IEnumerable<CartItemLock>?> GetCartItemLocksExpired()
        {
            return await _context.CartItemLocks.Where(cil => 
                    cil.Locked.AddDays(cil.LockedForDays) <= DateTime.Now)
                .ToListAsync();
        }


        public async Task<bool> ExistCartItemLock(Guid cartId, int itemId)
        {
            return await _context.CartItemLocks.AnyAsync(cil => cil.CartId == cartId && cil.ItemId == itemId);        
        }


        public async Task<EntityEntry> CreateCartItemLock(CartItemLock cartItemLock)
        {
            return await _context.CartItemLocks.AddAsync(cartItemLock);
        }


        public async Task CreateCartItemsLock(IEnumerable<CartItemLock> cartItemsLock)
        {
            await _context.CartItemLocks.AddRangeAsync(cartItemsLock);
        }


        public async Task<EntityEntry> DeleteCartItemLock(CartItemLock cartItemLock)
        {
            return _context.CartItemLocks.Remove(cartItemLock);
        }


        public async Task DeleteCartItemsLocks(IEnumerable<CartItemLock> cartItemsLocks)
        {
            _context.CartItemLocks.RemoveRange(cartItemsLocks);
        }
    }
}
