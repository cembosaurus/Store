using Business.Libraries.ServiceResult.Interfaces;
using Business.Ordering.DTOs;
using Business.Scheduler.DTOs;

namespace Business.Ordering.Http.Interfaces
{
    public interface IHttpCartClient
    {
        Task<HttpResponseMessage> CreateCart(int userId);
        Task<HttpResponseMessage> ExistsCartByCartId(Guid cartId);
        Task<HttpResponseMessage> RemoveExpiredItemsFromCart(IEnumerable<CartItemsLockDeleteDTO> cartItemLocks);
    }
}
