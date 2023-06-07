using Business.Inventory.DTOs.Item;
using Business.Libraries.ServiceResult.Interfaces;
using Business.Scheduler.DTOs;

namespace Ordering.HttpServices.Interfaces
{
    public interface IHttpSchedulerService
    {
        Task<IServiceResult<CartItemsLockReadDTO>> CartItemsLock(CartItemsLockCreateDTO cartItemsToLock);
        Task<IServiceResult<CartItemsLockReadDTO>> CartItemsUnLock(CartItemsLockDeleteDTO cartItemsToUnLock);
    }
}
