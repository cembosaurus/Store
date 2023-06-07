using Business.Libraries.ServiceResult.Interfaces;
using Business.Scheduler.DTOs;
using Scheduler.Models;

namespace Scheduler.HttpServices.Interfaces
{
    public interface IHttpCartService
    {
        Task<IServiceResult<bool>> ExistsCartByCartId(Guid cartId);
        Task<IServiceResult<IEnumerable<CartItemsLockReadDTO>>> RemoveExpiredItemsFromCart(IEnumerable<CartItemsLockDeleteDTO> cartItemLocks);
    }
}
