using Business.Http.Interfaces;
using Business.Inventory.DTOs.Item;

namespace Business.Inventory.Http.Clients.Interfaces
{
    public interface IHttpItemClient
    {
        Task<HttpResponseMessage> AddItem(ItemCreateDTO itemDTO);
        Task<HttpResponseMessage> DeleteItem(int itemId);
        Task<HttpResponseMessage> GetItemById(int itemId);
        Task<HttpResponseMessage> GetItems(IEnumerable<int> itemIds = null);
        Task<HttpResponseMessage> UpdateItem(int itemId, ItemUpdateDTO itemDTO);
    }
}
