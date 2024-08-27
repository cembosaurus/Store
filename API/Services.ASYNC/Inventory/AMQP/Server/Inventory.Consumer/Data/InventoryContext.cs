using Inventory.Consumer.Models;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Consumer.Data
{
    public class InventoryContext : DbContext
    {

        public InventoryContext(DbContextOptions<InventoryContext> opt) : base(opt)
        {
        }

        public DbSet<Item> Items { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Item>()
                .HasKey(i => i.Id);


            base.OnModelCreating(modelBuilder);
        }
    }
}
