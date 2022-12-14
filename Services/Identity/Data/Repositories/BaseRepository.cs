using Business.Data.Repositories.Interfaces;

namespace Services.Identity.Data.Repositories
{
    public class BaseRepository: IBaseRepository
    {
        private readonly IdentityContext _context;

        public BaseRepository(IdentityContext context)
        {
            _context = context;
        }

        public int SaveChanges()
        {
            return _context.SaveChanges();
        }
    }
}
