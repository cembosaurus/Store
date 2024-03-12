using Business.Inventory.DTOs.ItemPrice;
using Business.Libraries.ServiceResult.Interfaces;

namespace API_Gateway.Services.Business.Inventory.Interfaces
{
    public interface IItemPriceService
    {
        Task<IServiceResult<ItemPriceReadDTO>> GetItemPriceById(int id);
        Task<IServiceResult<IEnumerable<ItemPriceReadDTO>>> GetItemPrices(IEnumerable<int> itemIds = null);
        Task<IServiceResult<ItemPriceReadDTO>> UpdateItemPrice(int itemId, ItemPriceUpdateDTO itemPriceEditDTO);
    }
}
