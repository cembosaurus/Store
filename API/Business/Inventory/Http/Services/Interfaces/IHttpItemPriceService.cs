using Business.Inventory.DTOs.ItemPrice;
using Business.Libraries.ServiceResult.Interfaces;

namespace Business.Inventory.Http.Services.Interfaces
{
    public interface IHttpItemPriceService
    {
        Task<IServiceResult<ItemPriceReadDTO>> GetItemPriceById(int id);
        Task<IServiceResult<IEnumerable<ItemPriceReadDTO>>> GetItemPrices(IEnumerable<int> itemIds = null);
        Task<IServiceResult<ItemPriceReadDTO>> UpdateItemPrice(int itemId, ItemPriceUpdateDTO itemPriceEditDTO);
    }
}
