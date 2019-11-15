using System;
using Microsoft.EntityFrameworkCore;

namespace Tayra.Models.Catalog
{
    public class CatalogDbContext : DbContext
    {
        #region Constructor

        public CatalogDbContext(DbContextOptions<CatalogDbContext> options) : base(options)
        {
        }

        public CatalogDbContext(string connectionString, DbContextOptions<CatalogDbContext> options) : base(options)
        {
            ConnectionString = connectionString;
        }

        public CatalogDbContext(string connectionString) : this(connectionString, new DbContextOptions<CatalogDbContext>())
        {
        }

        #endregion

        #region Properties

        public string ConnectionString { get; set; }

        #endregion

        #region Datasets

        public DbSet<Organization> Organizations { get; set; }
        public DbSet<LandingPageContact> LandingPageContacts { get; set; }

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
            modelBuilder.Entity<Organization>().HasIndex(e => e.Key).IsUnique();

            Seed(modelBuilder);

            base.OnModelCreating(modelBuilder);

            Database.SetCommandTimeout(TimeSpan.FromMinutes(30));
        }

        #endregion

        #region Private Methods

        private void Seed(ModelBuilder modelBuilder)
        {

        }

        #endregion
    }
}
