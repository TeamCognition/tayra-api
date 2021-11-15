using System.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Tayra.Models.Catalog;

namespace Tayra.Models.Organizations
{
    public class OrganizationContextFactory : IDesignTimeDbContextFactory<OrganizationDbContext>
    {
        public OrganizationDbContext CreateDbContext(string[] args)
        {
            return new OrganizationDbContext(TenantModel.GetDummy(), null);
        }
    }
}