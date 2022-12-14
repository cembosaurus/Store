using Business.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Services.Inventory.Models;

namespace Services.Inventory.Data.Repositories.Interfaces
{
    public interface IItemRepository: IBaseRepository
    {
        Task<IEnumerable<Item>> GetItems(IEnumerable<int> itemIds = null);
        Task<Item> GetItemById(int id);
        Task<Item> GetItemByName(string name);
        Task<EntityEntry> AddItem(Item item);
        Task<EntityEntry> DeleteItem(Item item);
        Task<bool> ExistsById(int id);
        Task<bool> ExistsByName(string name);
    }
}
