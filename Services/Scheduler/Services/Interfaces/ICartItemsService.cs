using Business.Libraries.ServiceResult.Interfaces;
using Business.Scheduler.DTOs;

namespace Scheduler.Services.Interfaces
{
    public interface ICartItemsService
    {
        Task<IServiceResult<CartItemsLockReadDTO>> CartItemsLock(CartItemsLockCreateDTO cartItemsLockDTO);
        Task<IServiceResult<CartItemsLockReadDTO>> CartItemsUnLock(CartItemsLockDeleteDTO cartItemsToUnLockDTO);
        Task<IServiceResult<IEnumerable<CartItemsLockReadDTO>>> GetAllLocks();
        Task<IServiceResult<IEnumerable<CartItemsLockReadDTO>>> RemoveExpiredItemsFromCart(IEnumerable<CartItemsLockDeleteDTO> cartItemsToRemoveDTO);
    }
}
