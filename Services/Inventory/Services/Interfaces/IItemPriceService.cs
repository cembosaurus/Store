using Business.Inventory.DTOs.ItemPrice;
using Business.Libraries.ServiceResult.Interfaces;

namespace Inventory.Services.Interfaces
{
    public interface IItemPriceService
    {
        Task<IServiceResult<ItemPriceReadDTO>> UpdateItemPrice(int itemId, ItemPriceUpdateDTO itemPriceEditDTO);
        Task<IServiceResult<IEnumerable<ItemPriceReadDTO>>> GetItemPrices(IEnumerable<int> itemIds = default);
        Task<IServiceResult<ItemPriceReadDTO>> GetItemPriceById(int id);
        Task<IServiceResult<IEnumerable<ItemPriceReadDTO>>> GetItemPricesByIds(IEnumerable<int> itemIds);
    }
}
