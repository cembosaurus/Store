using Business.Libraries.ServiceResult.Interfaces;
using Business.Scheduler.DTOs;

namespace Business.Scheduler.Http.Services.Interfaces
{
    public interface IHttpSchedulerService
    {
        Task<IServiceResult<CartItemsLockReadDTO>> CartItemsLock(CartItemsLockCreateDTO cartItemsToLock);
        Task<IServiceResult<CartItemsLockReadDTO>> CartItemsUnLock(CartItemsLockDeleteDTO cartItemsToUnLock);
    }
}
