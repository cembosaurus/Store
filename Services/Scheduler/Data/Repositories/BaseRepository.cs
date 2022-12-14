using Business.Data.Repositories.Interfaces;

namespace Scheduler.Data.Repositories
{
    public class BaseRepository : IBaseRepository
    {
        private readonly SchedulerDBContext _context;

        public BaseRepository(SchedulerDBContext context)
        {
            _context = context;
        }

        public int SaveChanges()
        {
            return _context.SaveChanges();
        }
    }
}
