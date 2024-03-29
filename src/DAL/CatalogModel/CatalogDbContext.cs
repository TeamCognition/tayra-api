﻿using Finbuckle.MultiTenant.Stores;
using Microsoft.EntityFrameworkCore;

namespace Tayra.Models.Catalog
{
    public class CatalogDbContext : EFCoreStoreDbContext<Tenant>
    {
        #region Constructor

        public CatalogDbContext(DbContextOptions<CatalogDbContext> options) : base(options)
        {
        }
        
        public CatalogDbContext(string connectionString): base(new DbContextOptions<CatalogDbContext>())
        {
            ConnectionString = connectionString;
        }
        #endregion

        public string ConnectionString { get; }
        
        #region Datasets

        public DbSet<Identity> Identities { get; set; }
        public DbSet<IdentityEmail> IdentityEmails { get; set; }
        public DbSet<TenantIdentity> TenantIdentities { get; set; }
        public DbSet<TenantIntegration> TenantIntegrations { get; set; }

        public DbSet<LandingPageContact> LandingPageContacts { get; set; }
        public DbSet<LandingPageTry> LandingPageTry { get; set; }

        #endregion

        #region Protected Methods

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder
                    .UseSqlServer(ConnectionString);
            }

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IdentityEmail>(entity =>
            {
                entity.HasKey(x => new { x.IdentityId, x.Email });
                entity.HasIndex(x => new { x.Email, x.IsPrimary }).IsUnique();
            });

            modelBuilder.Entity<Tenant>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.HasIndex(e => e.Identifier);

                entity.Property(e => e.Id).HasMaxLength(128);

                entity.Property(e => e.ServicePlan)
                    .IsRequired()
                    .HasColumnType("char(10)")
                    .HasDefaultValueSql("'standard'");
            });

            modelBuilder.Entity<TenantIdentity>(entity =>
            {
                entity.HasKey(x => new { x.TenantId, x.IdentityId });
            });

            modelBuilder.Entity<TenantIntegration>(entity =>
            {
                entity.HasKey(x => new { x.TenantId, x.Type, x.SegmentId });
                entity.HasIndex(x => new { x.InstallationId });
            });

            Seed(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }

        #endregion

        #region Private Methods

        private void Seed(ModelBuilder modelBuilder)
        {

        }

        #endregion
    }
}
