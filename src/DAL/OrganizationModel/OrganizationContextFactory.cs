using System.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Tayra.Models.Organizations
{
    public class OrganizationContextFactory : IDesignTimeDbContextFactory<OrganizationDbContext>
    {
        public OrganizationDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<OrganizationDbContext>();
            optionsBuilder.UseSqlServer(new SqlConnection());

            return new OrganizationDbContext(optionsBuilder.Options);
        }
    }

    public static class OrganizationContextFactoryForTests
    {
        public static OrganizationDbContext CreateDbContext(DbContextOptions<OrganizationDbContext> options)
        {
            return new OrganizationDbContext(options);
        }
    }
}