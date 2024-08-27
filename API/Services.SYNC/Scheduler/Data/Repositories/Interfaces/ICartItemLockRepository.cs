using Business.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Scheduler.Models;

namespace Scheduler.Data.Repositories.Interfaces
{
    public interface ICartItemLockRepository : IBaseRepository
    {
        Task<EntityEntry> CreateCartItemLock(CartItemLock cartItemLock);
        Task CreateCartItemsLock(IEnumerable<CartItemLock> cartItemsLock);
        Task<EntityEntry> DeleteCartItemLock(CartItemLock cartItemLock);
        Task DeleteCartItemsLocks(IEnumerable<CartItemLock> cartItemsLocks);
        Task<bool> ExistCartItemLock(Guid cartId, int itemId);
        Task<IEnumerable<CartItemLock>> GetAllCartItemLocks();
        Task<CartItemLock?> GetCartItemLock(Guid cartId, int itemId);
        Task<IEnumerable<CartItemLock>?> GetCartItemLocksExpired();
    }
}
