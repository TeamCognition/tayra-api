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

        #endregion

        #region Properties

        public string ConnectionString { get; set; }

        #endregion

        #region Datasets

        public DbSet<Tenant> Tenants { get; set; }
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
            modelBuilder.Entity<Tenant>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.HasIndex(e => e.Name);

                entity.Property(e => e.Id).HasMaxLength(128);

                entity.Property(e => e.ServicePlan)
                    .IsRequired()
                    .HasColumnType("char(10)")
                    .HasDefaultValueSql("'standard'");
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
