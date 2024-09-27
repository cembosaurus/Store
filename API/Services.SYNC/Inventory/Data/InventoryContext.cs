using Inventory.Models;
using Microsoft.EntityFrameworkCore;

namespace Services.Inventory.Data
{
    public class InventoryContext: DbContext
    {

        private readonly IConfiguration _conf;
        private readonly bool _isProdEnv;


        public InventoryContext(DbContextOptions<InventoryContext> opt, IConfiguration conf, IWebHostEnvironment env) : base(opt)
        {
            _conf = conf;
            _isProdEnv = env.IsProduction();
        }

        public DbSet<Item> Items { get; set; }
        public DbSet<CatalogueItem> CatalogueItems { get; set; }
        public DbSet<ItemPrice> ItemPrices { get; set; }
        public DbSet<AccessoryItem> Accessories { get; set; }
        public DbSet<SimilarProductItem> SimilarProducts { get; set; }



        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_conf.GetSection($"Config.Local:ConnectionStrings:InventoryConnStr:{(_isProdEnv ? "Prod" : "Dev")}").Value, opt => opt.EnableRetryOnFailure());
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Item>()
                .HasKey(i => i.Id);

            modelBuilder.Entity<Item>()
                .HasOne(i => i.CatalogueItem)
                .WithOne(ci => ci.Item);


            modelBuilder.Entity<CatalogueItem>()
                .HasKey(ci => ci.ItemId);            
            
            modelBuilder.Entity<CatalogueItem>()
                .HasOne(ci => ci.Item)
                .WithOne(i => i.CatalogueItem)
                .HasPrincipalKey<Item>(i => i.Id);



            modelBuilder.Entity<ItemPrice>()
                .HasKey(ip => ip.ItemId);

            modelBuilder.Entity<ItemPrice>()
                .HasOne(ip => ip.CatalogueItem)
                .WithOne(ci => ci.ItemPrice)
                .HasPrincipalKey<CatalogueItem>(ci => ci.ItemId)
                .IsRequired();



            modelBuilder.Entity<AccessoryItem>()
                .HasKey(a => new { a.ItemId, a.AccessoryItemId });

            modelBuilder.Entity<AccessoryItem>()
                .HasOne(a => a.CatalogueItem)
                .WithMany(ci => ci.Accessories)
                .HasPrincipalKey(ci => ci.ItemId);




            modelBuilder.Entity<SimilarProductItem>()
                .HasKey(sp => new { sp.ItemId, sp.SimilarProductItemId });

            modelBuilder.Entity<SimilarProductItem>()
                .HasOne(sp => sp.CatalogueItem)
                .WithMany(ci => ci.SimilarProducts)
                .HasPrincipalKey(ci => ci.ItemId);


            base.OnModelCreating(modelBuilder);
        }
    }
}
