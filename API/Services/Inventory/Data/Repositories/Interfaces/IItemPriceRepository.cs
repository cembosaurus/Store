using Business.Data.Repositories.Interfaces;
using Inventory.Models;

namespace Services.Inventory.Data.Repositories.Interfaces
{
    public interface IItemPriceRepository: IBaseRepository
    {
        Task<ItemPrice> GetItemPriceById(int id);
        Task<IEnumerable<ItemPrice>> GetItemPrices(IEnumerable<int> itemIds);
        Task<bool> ItemExistsById(int id);
    }
}
