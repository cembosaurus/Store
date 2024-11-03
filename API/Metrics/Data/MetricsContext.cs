using Metrics.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System.Diagnostics;
using System;

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
        public DbSet<Models.Request> Requests { get; set; }
        public DbSet<Models.Response> Responses { get; set; }




        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_conf.GetSection($"Config.Local:ConnectionStrings:MetricsConnStr:{(_isProdEnv ? "Prod" : "Dev")}").Value ?? "", opt => opt.EnableRetryOnFailure());
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<HttpTransaction>()
                .Property(t => t.Id)
                .ValueGeneratedOnAdd();
            modelBuilder.Entity<HttpTransaction>()
                .HasKey(t => t.Id);


            modelBuilder.Entity<APIService>()
                .HasKey(i => i.Id);


            modelBuilder.Entity<Request>()
                .HasKey(r => new {r.TransactionId, r.Index });
            modelBuilder.Entity<Request>()
                .HasOne(r => r.HttpTransaction)
                .WithMany(ht => ht.Requests)
                .HasPrincipalKey(ht => ht.Id)
                .IsRequired();
            modelBuilder.Entity<Request>()
                .HasOne(r => r.APIService)
                .WithMany(s => s.Requests)
                .HasForeignKey(r => r.ServiceId)
                .IsRequired();
            modelBuilder.Entity<Request>()
                .HasOne(r => r.Method)
                .WithMany(m => m.Requests)
                .HasForeignKey(r => r.HttpMethodName);
            modelBuilder.Entity<Request>()
                .Property(r => r.HttpMethodName)
                .HasConversion<string>();


            modelBuilder.Entity<Response>()
                .HasKey(r => new { r.TransactionId, r.Index });
            modelBuilder.Entity<Response>()
                .HasOne(r => r.HttpTransaction)
                .WithMany(ht => ht.Responses)
                .HasPrincipalKey(ht => ht.Id)
                .IsRequired();
            modelBuilder.Entity<Response>()
                .HasOne(r => r.APIService)
                .WithMany(s => s.Responses)
                .HasForeignKey(r => r.ServiceId)
                .IsRequired();
            modelBuilder.Entity<Response>()
                .HasOne(r => r.Method)
                .WithMany(m => m.Responses)
                .HasForeignKey(r => r.HttpMethodName);
            modelBuilder.Entity<Response>()
                .Property(r => r.HttpMethodName)
                .HasConversion<string>();


            modelBuilder.Entity<Method>()
                .HasKey(m => m.Name);
            modelBuilder.Entity<Method>(m => { 
                m.Property(p => p.Name).IsRequired();
                m.HasData(
                    new Method { Name = "CONNECT" },
                    new Method { Name = "DELETE"},
                    new Method { Name = "GET"},
                    new Method { Name = "HEAD"},
                    new Method { Name = "OPTIONS"},
                    new Method { Name = "PATCH"},
                    new Method { Name = "POST"},
                    new Method { Name = "PUT"},
                    new Method { Name = "TRACE"}
                    );
            });

           

            base.OnModelCreating(modelBuilder);
        }
    }
}



