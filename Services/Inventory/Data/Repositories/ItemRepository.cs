using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Services.Inventory.Data.Repositories.Interfaces;
using Services.Inventory.Models;

namespace Services.Inventory.Data
{
    public class ItemRepository : BaseRepository, IItemRepository
    {

        private readonly InventoryContext _context;


        public ItemRepository(InventoryContext context): base(context)
        {
            _context = context;
        }



        public async Task<IEnumerable<Item>> GetItems(IEnumerable<int> itemIds = default)
        {
            if (itemIds != null && itemIds.Any())
                return await _context.Items
                    .Where(i => i.Archived == false && itemIds.Contains(i.Id))
                    .ToListAsync();

            return await _context.Items.ToListAsync();
        }       
        
        
        public async Task<Item> GetItemById(int id)
        {
            return await _context.Items.Where(i => i.Archived == false).FirstOrDefaultAsync(i => i.Id == id);
        }


        public async Task<Item> GetItemByName(string name)
        {
            return await _context.Items.Where(i => i.Archived == false).FirstOrDefaultAsync(i => i.Name == name);
        }


        public async Task<EntityEntry> AddItem(Item item)
        {
            return await _context.Items.AddAsync(item);
        }

        public async Task<EntityEntry> DeleteItem(Item item)
        {
            return _context.Items.Remove(item);
        }


        public async Task<bool> ExistsById(int id)
        { 
            return await _context.Items.Where(i => i.Archived == false).AnyAsync(i => i.Id == id);
        }


        public async Task<bool> ExistsByName(string name)
        {
            return await _context.Items.Where(i => i.Archived == false).AnyAsync(i => i.Name == name);
        }


    }
}
