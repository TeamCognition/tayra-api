using System;
using Microsoft.EntityFrameworkCore;

namespace Tayra.Models.Core
{
    public class CoreDbContext : DbContext
    {
        #region Constructor

        public CoreDbContext(DbContextOptions<CoreDbContext> options) : base(options)
        {
        }

        public CoreDbContext(string connectionString, DbContextOptions<CoreDbContext> options) : base(options)
        {
            ConnectionString = connectionString;
        }

        public CoreDbContext(string connectionString) : this(connectionString, new DbContextOptions<CoreDbContext>())
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
