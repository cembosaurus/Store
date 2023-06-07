using API_Gateway.HttpServices.Inventory.Interfaces;
using API_Gateway.Services.Inventory.Interfaces;
using Business.Inventory.DTOs.Item;
using Business.Libraries.ServiceResult.Interfaces;

namespace API_Gateway.Services.Inventory
{
    public class ItemService : IItemService
    {
        private readonly IHttpItemService _httpItemService;

        public ItemService(IHttpItemService httpItemService)
        {
            _httpItemService = httpItemService;
        }





        public async Task<IServiceResult<IEnumerable<ItemReadDTO>>> GetItems(IEnumerable<int> itemIds = default)
        {
            return await _httpItemService.GetItems(itemIds);
        }



        public async Task<IServiceResult<ItemReadDTO>> GetItemById(int id)
        {
            return await _httpItemService.GetItemById(id);
        }



        public async Task<IServiceResult<ItemReadDTO>> AddItem(ItemCreateDTO item)
        {
            return await _httpItemService.AddItem(item);
        }



        public async Task<IServiceResult<ItemReadDTO>> DeleteItem(int id)
        {
            return await _httpItemService.DeleteItem(id);
        }



        public async Task<IServiceResult<ItemReadDTO>> UpdateItem(int id, ItemUpdateDTO item)
        {
            return await _httpItemService.UpdateItem(id, item);
        }
    }
}
