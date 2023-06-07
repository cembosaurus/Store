using Business.Libraries.ServiceResult.Interfaces;
using Business.Ordering.DTOs;
using Business.Scheduler.DTOs;

namespace Business.Ordering.Http.Interfaces
{
    public interface IHttpCartClient
    {
        Task<HttpResponseMessage> CreateCart(int userId);
        Task<HttpResponseMessage> DeleteCart(int id);
        Task<HttpResponseMessage> ExistsCartByCartId(Guid cartId);
        Task<HttpResponseMessage> GetCards();
        Task<HttpResponseMessage> GetCartByUserId(int userId);
        Task<HttpResponseMessage> RemoveExpiredItemsFromCart(IEnumerable<CartItemsLockDeleteDTO> cartItemLocks);
        Task<HttpResponseMessage> UpdateCart(int userId, CartUpdateDTO cartUpdateDTO);
    }
}
