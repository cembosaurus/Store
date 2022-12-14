using Business.Data.Repositories.Interfaces;
using Business.Libraries.ServiceResult.Interfaces;
using Business.Ordering.DTOs;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Services.Ordering.Models;

namespace Ordering.Data.Repositories.Interfaces
{
    public interface ICartItemsRepository : IBaseRepository
    {
        Task<EntityEntry> DeleteCartItem(CartItem cartItem);
        Task DeleteCartItems(IEnumerable<CartItem> cartItems);
        Task<IEnumerable<CartItem>> GetAllCardItems();
        Task<IEnumerable<CartItem>> GetCartItemsByCartId(Guid cartId);
        Task<IEnumerable<CartItem>> GetCartItemsByUserId(int userId);
    }
}
