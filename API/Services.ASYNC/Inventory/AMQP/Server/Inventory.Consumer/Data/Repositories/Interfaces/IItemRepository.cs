using Business.Data.Repositories.Interfaces;
using Inventory.Consumer.Models;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Consumer.Data.Repositories.Interfaces
{
    public interface IItemRepository : IBaseRepository
    {
        Task<EntityState> AddItem(Item item);
        Task<EntityState> DeleteItem(Item item);
        Task<bool> ExistsById(int id);
        Task<bool> ExistsByName(string name);
        Task<Item> GetItemById(int id);
        Task<Item> GetItemByName(string name);
        Task<IEnumerable<Item>> GetItems(IEnumerable<int> itemIds = null);
    }
}
