using AutoMapper;
using Business.Data.Repositories;
using Inventory.Models;
using Microsoft.EntityFrameworkCore;
using Services.Inventory.Data.Repositories.Interfaces;



namespace Services.Inventory.Data
{

    public class CatalogueItemRepository : BaseRepository<InventoryContext>, ICatalogueItemRepository
    {

        public CatalogueItemRepository(InventoryContext context, IMapper mapper) : base(context)
        {
        }




        public override int SaveChanges()
        {
            return _context.SaveChanges();
        }




        public async Task<Item> GetItemById(int id)
        {
            return await _context.Items.FirstOrDefaultAsync(i => i.Id == id);
        }


        public async Task<Item> GetItemByName(string name)
        {
            return await _context.Items.FirstOrDefaultAsync(i => i.Name == name);
        }


        public async Task<IEnumerable<CatalogueItem>> GetCatalogueItems(IEnumerable<int> itemIds = default)
        {
            if (itemIds != null && itemIds.Any())
                return await _context.CatalogueItems
                    .Where(ci => itemIds.Contains(ci.ItemId))
                    .Include(ci => ci.Item)
                    .Include(ci => ci.ItemPrice)
                    .ToListAsync();

            return await _context.CatalogueItems
                .Include(ci => ci.Item)
                .Include(ci => ci.ItemPrice)
                .ToListAsync();
        }


        public async Task<CatalogueItem> GetCatalogueItemById(int id)
        {
            return await _context.CatalogueItems.Where(x => x.ItemId == id)
                .Include(ci => ci.Item)
                .Include(ci => ci.ItemPrice)
                .FirstOrDefaultAsync();
        }


        public async Task<CatalogueItem> GetCatalogueItemWithExtrasById(int id)
        {
            return await _context.CatalogueItems.Where(x => x.ItemId == id)
                .Include(ci => ci.Item)
                .Include(ci => ci.SimilarProducts)
                .Include(ci => ci.Accessories)
                .Include(ci => ci.ItemPrice)
                .SingleOrDefaultAsync();
        }


        public async Task<IEnumerable<CatalogueItem>> GetAccessoriesForCatalogueItem(CatalogueItem catalogueItem)
        {
            var ids = catalogueItem.Accessories.Select(a => a.AccessoryItemId).ToList();

            return await _context.CatalogueItems.Where(ci => ids.Any(x => x == ci.ItemId))
                .Include(ci => ci.Item)
                .Include(ci => ci.ItemPrice)
                .ToListAsync();
        }


        public async Task<IEnumerable<CatalogueItem>> GetSimilarProductsForCatalogueItem(CatalogueItem catalogueItem)
        {
            var ids = catalogueItem.SimilarProducts.Select(a => a.SimilarProductItemId).ToList();

            return await _context.CatalogueItems.Where(ci => ids.Any(x => x == ci.ItemId))
                .Include(ci => ci.Item)
                .Include(ci => ci.ItemPrice)
                .ToListAsync();
        }



        public async Task<EntityState> CreateCatalogueItem(CatalogueItem catalogueItem)
        {
            var resultState = await _context.CatalogueItems.AddAsync(catalogueItem);

            return resultState.State;
        }



        public async Task<EntityState> RemoveCatalogueItem(CatalogueItem catalogueItem)
        {
            //var catalogueItemToRemove = await _context.CatalogueItems.Where(x => x.ItemId == catalogueItem.ItemId)
            //    .Include(ci => ci.Item)
            //    .Include(ci => ci.ItemPrice)
            //    .FirstOrDefaultAsync();

            //if (catalogueItemToRemove == null)
            //    return false;

            //var result = _context.CatalogueItems.Remove(catalogueItemToRemove);

            return _context.CatalogueItems.Remove(catalogueItem).State; // result.State == EntityState.Deleted ? true: false;
        }



        public async Task AddAccessoriesToCatalogueItem(int id, IEnumerable<AccessoryItem> accessories)
        {
            await _context.Accessories.AddRangeAsync(accessories);
        }



        public async Task AddSimilarProductsToCatalogueItem(int id, IEnumerable<SimilarProductItem> similarProducts)
        {
            await _context.SimilarProducts.AddRangeAsync(similarProducts);
        }



        public async Task RemoveAccessoriesFromCatalogueItem(int id, IEnumerable<AccessoryItem> accessories)
        {
            _context.Accessories.RemoveRange(accessories);
        }



        public async Task RemoveSimilarProductsFromCatalogueItem(int id, IEnumerable<SimilarProductItem> similarProducts)
        {
            _context.SimilarProducts.RemoveRange(similarProducts);
        }



        public async Task<bool> ExistsById(int id)
        {
            return await _context.CatalogueItems.AnyAsync(ci => ci.ItemId == id);
        }



        public async Task<bool> ExistsByName(string name)
        {
            return await _context.CatalogueItems.AnyAsync(ci => ci.Item.Name == name);
        }




        public async Task<int> GetInstockCount(int id)
        {
            return await _context.CatalogueItems.Where(ci => ci.ItemId == id).Select(s => s.Instock).FirstOrDefaultAsync();
        }

    }
}
