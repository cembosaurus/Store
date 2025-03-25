using Business.Data.Repositories;
using Metrics.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Metrics.Data.Repositories
{
    public class MetricsRepository : BaseRepository<MetricsContext>, IMetricsRepository
    {


        public MetricsRepository(MetricsContext context) : base(context)
        {
        }



        public override int SaveChanges()
        {
            return _context.SaveChanges();
        }




        //public async Task<EntityState> AddItem(Item item)
        //{
        //    var result = await _context.Items.AddAsync(item);

        //    return result.State;
        //}
    }
}
