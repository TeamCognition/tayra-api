using System;
using Tayra.Models.Catalog;

namespace Tayra.Models.Organizations
{
    public static class OrganizationsService
    {
        public static void InsertOrganization(string connectionString, TenantModel tenant)
        {
            using (var db = new OrganizationDbContext(TenantModel.WithConnectionStringOnly(connectionString), null))
            {
                db.LocalTenants.Add(new LocalTenant
                {
                    TenantId = Guid.Parse(tenant.Id),
                    Identifier = tenant.Identifier,
                    DisplayName = tenant.Name
                });
                db.SaveChanges();
            }
        }
    }
}
