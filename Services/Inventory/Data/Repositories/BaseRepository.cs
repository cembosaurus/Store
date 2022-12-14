using Business.Data.Repositories.Interfaces;

namespace Services.Inventory.Data
{
    public class BaseRepository: IBaseRepository
    {
        private readonly InventoryContext _context;

        public BaseRepository(InventoryContext context)
        {
            _context = context;
        }

        public int SaveChanges()
        {
            return _context.SaveChanges();
        }
    }
}
