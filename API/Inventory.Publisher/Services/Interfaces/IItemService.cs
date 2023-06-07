using Business.Inventory.DTOs.Item;
using Business.Libraries.ServiceResult.Interfaces;

namespace Inventory.Publisher.Services.Interfaces
{
    public interface IItemService
    {
        Task<IServiceResult<ItemReadDTO>> AddItem(ItemCreateDTO item);
        Task<IServiceResult<ItemReadDTO>> DeleteItem(int id);
        Task<IServiceResult<ItemReadDTO>> GetItemById(int id);
        Task<IServiceResult<IEnumerable<ItemReadDTO>>> GetItems(IEnumerable<int> itemIds = null);
        Task<IServiceResult<ItemReadDTO>> UpdateItem(int id, ItemUpdateDTO item);
    }
}
