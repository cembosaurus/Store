using Business.Libraries.ServiceResult.Interfaces;

namespace Scheduler.HttpServices.Interfaces
{
    public interface IHttpInventoryService
    {
        Task<IServiceResult<bool>> ExistsCatalogueItemById(int itemId);
    }
}
