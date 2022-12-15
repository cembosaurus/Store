using Microsoft.EntityFrameworkCore;
using Services.Inventory.Data.Repositories.Interfaces;
using Services.Inventory.Models;

namespace Services.Inventory.Data
{
    public class ItemPriceRepository : BaseRepository, IItemPriceRepository
    {
        private readonly InventoryContext _context;

        public ItemPriceRepository(InventoryContext context): base(context)
        {
            _context = context;
        }



        public async Task<IEnumerable<ItemPrice>> GetItemPrices(IEnumerable<int> itemIds)
        {
            if (itemIds != null && itemIds.Any())
                return await _context.ItemPrices
                    .Where(i => itemIds.Contains(i.ItemId))
                    .ToListAsync();

            return await _context.ItemPrices.ToListAsync();
        }


        public async Task<ItemPrice> GetItemPriceById(int id)
        {
            return await _context.ItemPrices.FirstOrDefaultAsync(i => i.ItemId == id);
        }



        public async Task<bool> ItemExistsById(int id)
        {
            return await _context.Items.AnyAsync(i => i.Id == id);
        }

    }
}
