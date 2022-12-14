using Microsoft.EntityFrameworkCore;
using Scheduler.Models;

namespace Scheduler.Data
{
    public class SchedulerDBContext : DbContext
    {


        public SchedulerDBContext(DbContextOptions<SchedulerDBContext> opt) : base(opt)
        {

        }


        public DbSet<CartItemLock> CartItemLocks { get; set; }



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
