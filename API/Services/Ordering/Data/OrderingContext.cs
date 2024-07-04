using Microsoft.EntityFrameworkCore;
using Services.Ordering.Models;

namespace Ordering.Data
{
    public class OrderingContext : DbContext
    {

        private IConfiguration _conf;
        private readonly bool _isProdEnv;


        public OrderingContext(DbContextOptions<OrderingContext> opt, IConfiguration conf, IWebHostEnvironment env) : base(opt)
        {
            _conf = conf;
            _isProdEnv = env.IsProduction();
        }



        public DbSet<Cart> Carts { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<OrderDetails> OrderDetails { get; set; }
        public DbSet<ActiveCart> ActiveCarts { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_conf.GetSection($"Config.Local:ConnectionStrings:OrderingConnStr:{(_isProdEnv ? "Prod" : "Dev")}").Value, opt => opt.EnableRetryOnFailure());
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<Cart>()
                .HasKey(c => c.CartId);

            modelBuilder.Entity<Cart>()
                .Property(c => c.CartId)
                .ValueGeneratedNever();



            modelBuilder.Entity<ActiveCart>()
                .HasKey(ac => ac.CartId);

            modelBuilder.Entity<ActiveCart>()
                .HasIndex(ac => ac.UserId)
                .IsUnique();

            modelBuilder.Entity<ActiveCart>()
                .HasOne(ac => ac.Cart)
                .WithOne(c => c.ActiveCart)
                .HasForeignKey<ActiveCart>(ac => ac.CartId);

            

            modelBuilder.Entity<Order>()
                .HasKey(o => o.CartId);

            modelBuilder.Entity<Order>()
                .HasOne(o => o.Cart)
                .WithOne(c => c.Order)
                .HasForeignKey<Order>(o => o.CartId);

            

            modelBuilder.Entity<OrderDetails>()
                .HasKey(od => od.CartId);

            modelBuilder.Entity<OrderDetails>()
                .HasOne(od => od.Order)
                .WithOne(o => o.OrderDetails)
                .HasForeignKey<OrderDetails>(o => o.CartId);



            modelBuilder.Entity<CartItem>()
                .HasKey(ci => new { ci.CartId, ci.ItemId });





        }

    }
}
