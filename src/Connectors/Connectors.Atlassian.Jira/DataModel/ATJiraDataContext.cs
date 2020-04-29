using System;
using Microsoft.EntityFrameworkCore;

namespace Tayra.Connectors.Atlassian.Jira
{
    public class ATJiraDataContext : DbContext
    {
        #region Constructor

        public ATJiraDataContext(DbContextOptions<ATJiraDataContext> options) : base(options)
        {
        }

        public ATJiraDataContext(string connectionString) : base(new DbContextOptions<ATJiraDataContext>())
        {
            ConnectionString = connectionString;
        }

        #endregion

        #region Properties

        public string ConnectionString { get; }

        #endregion

        #region Db Sets



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
            base.OnModelCreating(modelBuilder);
            Database.SetCommandTimeout(TimeSpan.FromHours(3));
        }

        #endregion
    }
}
