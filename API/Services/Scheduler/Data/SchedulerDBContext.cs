using Microsoft.EntityFrameworkCore;
using Scheduler.Models;

namespace Scheduler.Data
{
    public class SchedulerDBContext : DbContext
    {

        private IConfiguration _conf;
        private readonly bool _isProdEnv;


        public SchedulerDBContext(DbContextOptions<SchedulerDBContext> opt, IConfiguration conf, IWebHostEnvironment env) : base(opt)
        {
            _conf = conf;
            _isProdEnv = env.IsProduction();
        }


        public DbSet<CartItemLock> CartItemLocks { get; set; }



        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_conf.GetSection($"Config.Local:ConnectionStrings:SchedulerConnStr:{(_isProdEnv ? "Prod" : "Dev")}").Value, opt => opt.EnableRetryOnFailure());
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<CartItemLock>()
                .HasKey(cil => new { cil.ItemId, cil.CartId});

            modelBuilder.Entity<CartItemLock>()
                .Property(cil => cil.Locked)
                .IsRequired();

            modelBuilder.Entity<CartItemLock>()
                .Property(cil => cil.LockedForDays)
                .IsRequired();


        }


    }
}
