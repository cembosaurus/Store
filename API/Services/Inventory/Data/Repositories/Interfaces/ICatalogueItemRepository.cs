using Business.Data.Repositories.Interfaces;
using Inventory.Models;
using Microsoft.EntityFrameworkCore;

namespace Services.Inventory.Data.Repositories.Interfaces
{
    public interface ICatalogueItemRepository: IBaseRepository
    {
        Task AddAccessoriesToCatalogueItem(int id, IEnumerable<AccessoryItem> accessories);
        Task AddSimilarProductsToCatalogueItem(int id, IEnumerable<SimilarProductItem> similarProducts);
        Task<EntityState> CreateCatalogueItem(CatalogueItem catalogueItem);
        Task<bool> ExistsById(int id);
        Task<bool> ExistsByName(string name);
        Task<IEnumerable<CatalogueItem>> GetAccessoriesForCatalogueItem(CatalogueItem catalogueItem);
        Task<CatalogueItem> GetCatalogueItemById(int id);
        Task<IEnumerable<CatalogueItem>> GetCatalogueItems(IEnumerable<int> itemIds = null);
        Task<CatalogueItem> GetCatalogueItemWithExtrasById(int id);
        Task<int> GetInstockCount(int id);
        Task<Item> GetItemById(int id);
        Task<Item> GetItemByName(string name);
        Task<IEnumerable<CatalogueItem>> GetSimilarProductsForCatalogueItem(CatalogueItem catalogueItem);
        Task RemoveAccessoriesFromCatalogueItem(int id, IEnumerable<AccessoryItem> accessories);
        Task<EntityState> RemoveCatalogueItem(CatalogueItem catalogueItem);
        Task RemoveSimilarProductsFromCatalogueItem(int id, IEnumerable<SimilarProductItem> similarProducts);
    }
}
