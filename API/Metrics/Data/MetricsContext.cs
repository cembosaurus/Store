using Microsoft.EntityFrameworkCore;

namespace Metrics.Data
{
    public class MetricsContext : DbContext
    {

        private IConfiguration _conf;
        private readonly bool _isProdEnv;


        public MetricsContext(DbContextOptions<MetricsContext> opt, IConfiguration conf, IWebHostEnvironment env) : base(opt)
        {
            _conf = conf;
            _isProdEnv = env.IsProduction();
        }





        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_conf.GetSection($"Config.Local:ConnectionStrings:MetricsConnStr:{(_isProdEnv ? "Prod" : "Dev")}").Value, opt => opt.EnableRetryOnFailure());
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {



            base.OnModelCreating(modelBuilder);
        }
    }
}



