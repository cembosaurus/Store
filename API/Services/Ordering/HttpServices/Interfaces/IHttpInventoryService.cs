using Business.Inventory.DTOs.CatalogueItem;
using Business.Inventory.DTOs.Item;
using Business.Inventory.DTOs.ItemPrice;
using Business.Libraries.ServiceResult.Interfaces;

namespace Ordering.HttpServices.Interfaces
{
    public interface IHttpInventoryService
    {
        Task<IServiceResult<int>> AddAmountToStock(int itemId, int amount);
        Task<IServiceResult<CatalogueItemReadDTO>> GetCatalogueItemById(int itemId);
        Task<IServiceResult<IEnumerable<CatalogueItemReadDTO>>> GetCatalogueItems(IEnumerable<int> itemIds = null);
        Task<IServiceResult<int>> GetInstockCount(int itemId);
        Task<IServiceResult<ItemReadDTO>> GetItemById(int itemId);
        Task<IServiceResult<ItemPriceReadDTO>> GetItemPriceById(int itemId);
        Task<IServiceResult<IEnumerable<ItemPriceReadDTO>>> GetItemPrices(IEnumerable<int> itemIds = null);
        Task<IServiceResult<IEnumerable<ItemReadDTO>>> GetItems(IEnumerable<int> itemIds = null);
        Task<IServiceResult<int>> RemoveAmountFromStock(int itemId, int amount);
    }
}
