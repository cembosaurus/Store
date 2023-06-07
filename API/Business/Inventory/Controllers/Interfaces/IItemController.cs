using Business.Inventory.DTOs.Item;
using Microsoft.AspNetCore.Mvc;

namespace Business.Inventory.Controllers.Interfaces
{
    public interface IItemController
    {
        Task<ActionResult> AddItem(ItemCreateDTO item);
        Task<ActionResult> DeleteItemById(int id);
        Task<ActionResult> GetAllItems();
        Task<ActionResult> GetItemById(int id);
        Task<ActionResult> GetItems(IEnumerable<int> itemIds);
        Task<ActionResult> UpdateItem(int id, ItemUpdateDTO item);
    }
}
