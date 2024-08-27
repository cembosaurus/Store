using Business.Data.Repositories.Interfaces;
using Inventory.Models;
using Microsoft.EntityFrameworkCore;

namespace Services.Inventory.Data.Repositories.Interfaces
{
    public interface IItemRepository: IBaseRepository
    {
        Task<IEnumerable<Item>> GetItems(IEnumerable<int> itemIds = null);
        Task<Item> GetItemById(int id);
        Task<Item> GetItemByName(string name);
        Task<EntityState> AddItem(Item item);
        Task<EntityState> DeleteItem(Item item);
        Task<bool> ExistsById(int id);
        Task<bool> ExistsByName(string name);
    }
}
