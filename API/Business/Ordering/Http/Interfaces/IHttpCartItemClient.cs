using Business.Libraries.ServiceResult.Interfaces;
using Business.Ordering.DTOs;
using Business.Scheduler.DTOs;

namespace Business.Ordering.Http.Interfaces
{
    public interface IHttpCartItemClient
    {
        Task<HttpResponseMessage> AddItemsToCart(int cartId, IEnumerable<CartItemUpdateDTO> itemsToAdd);
        Task<HttpResponseMessage> DeleteExpiredCartItems(IEnumerable<CartItemsLockDeleteDTO> cartItemLocks);
        Task<HttpResponseMessage> DeleteCartItems(int userId, IEnumerable<int> itemIds);
        Task<HttpResponseMessage> GetAllCardItems();
        Task<HttpResponseMessage> GetCartItems(int userId);
        Task<HttpResponseMessage> RemoveCartItems(int cartId, IEnumerable<CartItemUpdateDTO> itemsToRemove);
    }
}
