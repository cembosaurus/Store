using Business.Libraries.ServiceResult.Interfaces;
using Business.Ordering.DTOs;
using Business.Scheduler.DTOs;

namespace Ordering.Services.Interfaces
{
    public interface ICartItemService
    {
        Task<IServiceResult<IEnumerable<CartItemReadDTO>>> AddItemsToCart(int cartId, IEnumerable<CartItemUpdateDTO> itemsToAdd);
        Task<IServiceResult<IEnumerable<CartItemsLockReadDTO>>> DeleteExpiredItems(IEnumerable<CartItemsLockDeleteDTO> cartItemLocks);
        Task<IServiceResult<IEnumerable<CartItemReadDTO>>> DeleteItemsFromCart(int userId, IEnumerable<int> itemIds);
        Task<IServiceResult<IEnumerable<CartItemReadDTO>>> GetAllCardItems();
        Task<IServiceResult<IEnumerable<CartItemReadDTO>>> GetCartItems(int userId);
        Task<IServiceResult<IEnumerable<CartItemReadDTO>>> RemoveItemsFromCart(int cartId, IEnumerable<CartItemUpdateDTO> itemsToRemove);
    }
}
