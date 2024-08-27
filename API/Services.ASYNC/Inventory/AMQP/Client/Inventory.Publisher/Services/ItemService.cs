using Business.Inventory.DTOs.Item;
using Business.Libraries.ServiceResult.Interfaces;
using Inventory.Publisher.AMQPServices.Interfaces;
using Inventory.Publisher.Services.Interfaces;

namespace Inventory.Publisher.Services
{
    public class ItemService : IItemService
    {
        private readonly IAMQPItemService _amqpItemService;


        public ItemService(IAMQPItemService amqpItemService)
        {
            _amqpItemService = amqpItemService;
        }





        public async Task<IServiceResult<IEnumerable<ItemReadDTO>>> GetItems(IEnumerable<int> itemIds = default)
        {
            return await _amqpItemService.GetItems(itemIds);
        }



        public async Task<IServiceResult<ItemReadDTO>> GetItemById(int id)
        {
            return await _amqpItemService.GetItemById(id);
        }



        public async Task<IServiceResult<ItemReadDTO>> AddItem(ItemCreateDTO item)
        {
            return await _amqpItemService.AddItem(item);
        }



        public async Task<IServiceResult<ItemReadDTO>> DeleteItem(int id)
        {
            return await _amqpItemService.DeleteItem(id);
        }



        public async Task<IServiceResult<ItemReadDTO>> UpdateItem(int id, ItemUpdateDTO item)
        {
            return await _amqpItemService.UpdateItem(id, item);
        }


    }
}
