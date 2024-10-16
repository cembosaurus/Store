using Metrics.Models;
using Microsoft.EntityFrameworkCore;

namespace Metrics.Data
{
    public class MetricsContext : DbContext
    {

        private readonly IConfiguration _conf;
        private readonly bool _isProdEnv;


        public MetricsContext(DbContextOptions<MetricsContext> opt, IConfiguration conf, IWebHostEnvironment env) : base(opt)
        {
            _conf = conf;
            _isProdEnv = env.IsProduction();
        }


        public DbSet<APIService> Services { get; set; }
        public DbSet<Models.HttpRequest> Requests { get; set; }
        public DbSet<Models.HttpResponse> Responses { get; set; }




        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_conf.GetSection($"Config.Local:ConnectionStrings:MetricsConnStr:{(_isProdEnv ? "Prod" : "Dev")}").Value ?? "", opt => opt.EnableRetryOnFailure());
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<APIService>()
                .HasKey(i => i.Id);



            modelBuilder.Entity<Models.HttpRequest>()
                .HasKey(r => new { r.TransactionId, r.Index });

            modelBuilder.Entity<Models.HttpRequest>()
                .HasOne(r => r.Service)
                .WithMany(s => s.Requests)
                .HasPrincipalKey(s => s.Id)
                .IsRequired();



            modelBuilder.Entity<Models.HttpResponse>()
                .HasKey(r => new { r.TransactionId, r.Index });
            // ???????? revisit the structure. Response should be redesigned
            modelBuilder.Entity<Models.HttpResponse>()
                .HasOne(res => res.Request)
                .WithOne(req => req.Response)
                .HasPrincipalKey<Models.HttpRequest>(req => req.TransactionId)
                .IsRequired();



            base.OnModelCreating(modelBuilder);
        }
    }
}



