using Business.Data.Repositories;
using Inventory.Models;
using Microsoft.EntityFrameworkCore;
using Services.Inventory.Data.Repositories.Interfaces;

namespace Services.Inventory.Data
{
    public class ItemRepository : BaseRepository<InventoryContext>, IItemRepository
    {


        public ItemRepository(InventoryContext context): base(context)
        {
        }




        public override int SaveChanges()
        {
            return _context.SaveChanges();
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


        public async Task<EntityState> AddItem(Item item)
        {
            var result = await _context.Items.AddAsync(item);

            return result.State;
        }

        public async Task<EntityState> DeleteItem(Item item)
        {
            return _context.Items.Remove(item).State;
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
