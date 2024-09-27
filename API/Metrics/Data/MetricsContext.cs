using Metrics.Models;
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


        public DbSet<Service> Services { get; set; }
        public DbSet<Request> Requests { get; set; }
        public DbSet<Response> Responses { get; set; }




        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_conf.GetSection($"Config.Local:ConnectionStrings:MetricsConnStr:{(_isProdEnv ? "Prod" : "Dev")}").Value ?? "", opt => opt.EnableRetryOnFailure());
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Service>()
                .HasKey(i => i.Id);



            modelBuilder.Entity<Request>()
                .HasKey(r => new { r.Id, r.Index });

            modelBuilder.Entity<Request>()
                .HasOne(r => r.Service)
                .WithMany(s => s.Requests)
                .HasPrincipalKey(s => s.Id)
                .IsRequired();



            modelBuilder.Entity<Response>()
                .HasKey(r => new { r.RequestId, r.Index });
            // ???????? revisit the structure. Response should be redesigned
            modelBuilder.Entity<Response>()
                .HasOne(res => res.Request)
                .WithOne(req => req.Response)
                .HasPrincipalKey<Request>(req => req.Id)
                .IsRequired();



            base.OnModelCreating(modelBuilder);
        }
    }
}



