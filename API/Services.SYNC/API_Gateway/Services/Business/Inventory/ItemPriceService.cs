using API_Gateway.Services.Business.Inventory.Interfaces;
using Business.Inventory.DTOs.ItemPrice;
using Business.Inventory.Http.Services.Interfaces;
using Business.Libraries.ServiceResult.Interfaces;



namespace API_Gateway.Services.Business.Inventory
{
    public class ItemPriceService : IItemPriceService
    {

        private readonly IHttpItemPriceService _httpItemPriceService;

        public ItemPriceService(IHttpItemPriceService httpItemPriceService)
        {
            _httpItemPriceService = httpItemPriceService;
        }




        public async Task<IServiceResult<ItemPriceReadDTO>> GetItemPriceById(int id)
        {
            return await _httpItemPriceService.GetItemPriceById(id);
        }



        public async Task<IServiceResult<IEnumerable<ItemPriceReadDTO>>> GetItemPrices(IEnumerable<int> itemIds = default)
        {
            return await _httpItemPriceService.GetItemPrices(itemIds);
        }



        public async Task<IServiceResult<ItemPriceReadDTO>> UpdateItemPrice(int itemId, ItemPriceUpdateDTO itemPriceEditDTO)
        {
            return await _httpItemPriceService.UpdateItemPrice(itemId, itemPriceEditDTO);
        }
    }
}
