using Business.Libraries.ServiceResult.Interfaces;
using Business.Ordering.DTOs;
using Business.Scheduler.DTOs;

namespace Business.Inventory.Http.Services.Interfaces
{
    public interface IHttpCartService
    {
        Task<IServiceResult<CartReadDTO>> CreateCart(int userId);
        Task<IServiceResult<CartReadDTO>> DeleteCart(int id);
        Task<IServiceResult<bool>> ExistsCartByCartId(Guid cartId);
        Task<IServiceResult<IEnumerable<CartReadDTO>>> GetCards();
        Task<IServiceResult<CartReadDTO>> GetCartByUserId(int userId);
        Task<IServiceResult<IEnumerable<CartItemsLockReadDTO>>> RemoveExpiredItemsFromCart(IEnumerable<CartItemsLockDeleteDTO> cartItemLocks);
        Task<IServiceResult<CartReadDTO>> UpdateCart(int userId, CartUpdateDTO cartUpdateDTO);
    }
}
