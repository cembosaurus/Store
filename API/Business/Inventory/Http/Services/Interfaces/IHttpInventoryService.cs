using Business.Libraries.ServiceResult.Interfaces;

namespace Business.Inventory.Http.Services.Interfaces
{
    public interface IHttpInventoryService
    {
        Task<IServiceResult<bool>> ExistsCatalogueItemById(int itemId);
    }
}
