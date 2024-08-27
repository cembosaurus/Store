using Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Services.Identity.Models;

namespace Services.Identity.Data
{
    public class IdentityContext : IdentityDbContext<AppUser, AppRole, int, IdentityUserClaim<int>, AppUserRole, IdentityUserLogin<int>, IdentityRoleClaim<int>, IdentityUserToken<int>>
    {
        private IConfiguration _conf;
        private readonly bool _isProdEnv;

        public IdentityContext(DbContextOptions<IdentityContext> opt, IConfiguration conf, IWebHostEnvironment env) : base(opt)
        {
            _conf = conf;
            _isProdEnv = env.IsProduction();
        }


        public DbSet<Address> Address { get; set; }
        public DbSet<UserAddress> UserAddress { get; set; }
        public DbSet<CurrentUsersAddress> CurrentUsersAddress { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_conf.GetSection($"Config.Local:ConnectionStrings:IdentityConnStr:{(_isProdEnv ? "Prod" : "Dev")}").Value, opt => opt.EnableRetryOnFailure());
        }


        protected override void OnModelCreating(ModelBuilder builder)
        {

            base.OnModelCreating(builder);


            builder.Entity<AppUser>()
                .HasMany(u => u.UserRoles)
                .WithOne(ur => ur.AppUser)
                .HasForeignKey(ur => ur.UserId)
                .IsRequired();


            builder.Entity<AppRole>()
                .HasMany(r => r.UserRoles)
                .WithOne(ur => ur.AppRole)
                .HasForeignKey(ur => ur.RoleId)
                .IsRequired();


            builder.Entity<Address>()
                .HasKey(a => a.AddressId);


            builder.Entity<UserAddress>()
                .HasKey(ua => new { ua.UserId, ua.AddressId });

            builder.Entity<UserAddress>()
                .HasOne(ua => ua.AppUser)
                .WithMany(u => u.UserAddresses)
                .HasForeignKey(ua => ua.UserId)
                .IsRequired();

            builder.Entity<UserAddress>()
                .HasOne(ua => ua.Address)
                .WithMany(u => u.UserAddresses)
                .HasForeignKey(ua => ua.AddressId)
                .IsRequired();


            builder.Entity<CurrentUsersAddress>()
                .HasKey(cua => cua.UserId);

            builder.Entity<CurrentUsersAddress>()
                .HasOne(cua => cua.AppUser)
                .WithOne(u => u.CurrentUsersAddress)
                .HasForeignKey<CurrentUsersAddress>(cua => cua.UserId);

            builder.Entity<CurrentUsersAddress>()
                .HasOne(cua => cua.Address)
                .WithMany(a => a.CurrentUsersAddresses)
                .HasForeignKey(cua => cua.AddressId);
        }
    }
}
