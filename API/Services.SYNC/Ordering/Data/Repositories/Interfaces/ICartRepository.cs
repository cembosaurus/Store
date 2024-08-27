using Business.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Services.Ordering.Models;

namespace Ordering.Data.Repositories.Interfaces
{
    public interface ICartRepository : IBaseRepository
    {
        Task<EntityEntry> CreateActiveCart(ActiveCart activeCart);
        Task<EntityEntry> CreateCart(Cart cart);
        Task<EntityEntry> DeleteActiveCart(ActiveCart activeCart);
        Task<EntityEntry> DeleteCart(Cart cart);
        Task<bool> ExistsByCartId(Guid id);
        Task<bool> ExistsByUserId(int id);
        Task<ActiveCart> GetActiveCart(Guid cartId);
        Task<IEnumerable<ActiveCart>> GetActiveCarts();
        Task<IEnumerable<Cart>> GetCards();
        Task<Cart> GetCartByCartId(Guid cartId);
        Task<Cart> GetCartByUserId(int userId);
        Task<Guid> GetCartIdByUserId(int userId);
        Task<IEnumerable<CartItem>> GetCartItems(Guid cartId);
        Task<Cart> GetCartWithOrderByUserId(int userId);
        Task<int> GetUserIdByCartId(Guid ucartId);
    }
}
