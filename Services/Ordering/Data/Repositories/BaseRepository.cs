using Business.Data.Repositories.Interfaces;

namespace Ordering.Data.Repositories
{
    public class BaseRepository : IBaseRepository
    {
        private readonly OrderingContext _context;

        public BaseRepository(OrderingContext context)
        {
            _context = context;
        }

        public int SaveChanges()
        {
            return _context.SaveChanges();
        }
    }
}
