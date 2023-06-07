using Business.Inventory.DTOs.CatalogueItem;
using Business.Libraries.ServiceResult.Interfaces;

namespace API_Gateway.Services.Inventory.Interfaces
{
    public interface ICatalogueItemService
    {
        Task<IServiceResult<ExtrasReadDTO>> AddExtrasToCatalogueItem(int id, ExtrasAddDTO extrasAddDTO);
        Task<IServiceResult<int>> AddAmountToStock(int itemId, int amount);
        Task<IServiceResult<CatalogueItemReadDTO>> CreateCatalogueItem(int itemId, CatalogueItemCreateDTO catalogueItemCreateDTO);
        Task<IServiceResult<bool>> ExistsCatalogueItemById(int id);
        Task<IServiceResult<CatalogueItemReadDTO>> GetCatalogueItemById(int id);
        Task<IServiceResult<IEnumerable<CatalogueItemReadDTO>>> GetCatalogueItems(IEnumerable<int> itemIds = null);
        Task<IServiceResult<CatalogueItemReadDTOWithExtras>> GetCatalogueItemWithExtrasById(int id);
        Task<IServiceResult<int>> GetInstockCount(int id);
        Task<IServiceResult<CatalogueItemReadDTO>> RemoveCatalogueItem(int id);
        Task<IServiceResult<ExtrasReadDTO>> RemoveExtrasFromCatalogueItem(int id, ExtrasRemoveDTO extrasRemoveDTO);
        Task<IServiceResult<int>> RemoveFromStockAmount(int itemId, int amount);
        Task<IServiceResult<CatalogueItemReadDTO>> UpdateCatalogueItem(int itemId, CatalogueItemUpdateDTO catalogueItemUpdateDTO);
    }
}
