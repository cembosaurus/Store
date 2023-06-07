using Business.Data.Repositories.Interfaces;

namespace Business.Data.Repositories
{
    public abstract class BaseRepository<C>: IBaseRepository
    {
        private readonly C _context;

        public BaseRepository(C context)
        {
            _context = context;
        }

        public abstract int SaveChanges();

    }
}
