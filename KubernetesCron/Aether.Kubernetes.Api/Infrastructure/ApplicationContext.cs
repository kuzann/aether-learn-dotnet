using Aether.Kubernetes.Api.Domains;
using Microsoft.EntityFrameworkCore;
using System;

namespace Aether.Kubernetes.Api.Infrastructure
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext()
        {
        }

        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            AppContext.SetSwitch("Npgsql.DisableDateTimeInfinityConversions", true);
        }


        public DbSet<VendorEntity> Vendors { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<VendorEntity>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("gen_random_uuid()"); // or uuid_generate_v4()
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            });
        }
    }
}
