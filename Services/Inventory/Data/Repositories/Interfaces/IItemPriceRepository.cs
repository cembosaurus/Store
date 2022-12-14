using Business.Data.Repositories.Interfaces;
using Business.Inventory.DTOs.ItemPrice;
using Services.Inventory.Models;

namespace Services.Inventory.Data.Repositories.Interfaces
{
    public interface IItemPriceRepository: IBaseRepository
    {
        Task<IEnumerable<ItemPrice>> GetItemPrices(IEnumerable<int> itemIds = default);
        Task<ItemPrice> GetItemPriceById(int id);
        Task<IEnumerable<ItemPrice>> GetItemPricesByIds(IEnumerable<int> ids);
        Task<bool> ItemExistsById(int id);
    }
}
