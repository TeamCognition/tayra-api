using System.Linq;
using Microsoft.Extensions.Configuration;
using Tayra.DAL;
using Tayra.Models.Catalog;
using Tayra.Models.Organizations;
namespace Tayra.Models.Seeder
{
    public static class Seeder
    {
        public static string DemoKey = "demo.tayra.io";
        public static void SeedAll(IConfigurationRoot config)
        {
            using (var catalogDbContext = new CatalogDbContext(ConnectionStringUtilities.GetCatalogDbConnStr(config)))
            {
                var tenantConnStrs = catalogDbContext.TenantInfo.Where(x => x.Identifier != DemoKey).Select(x => x.ConnectionString).ToArray();
                Seed(false, tenantConnStrs);
            }
        }

        public static void Seed(bool shouldDemoSeed = false, params string[] tenantConnectionStrings)
        {
            foreach (var connStr in tenantConnectionStrings)
            {
                var tenantInfo = TenantModel.WithConnectionStringOnly(connStr);
                using (var organizationDb = new OrganizationDbContext(tenantInfo, null))
                {
                    tenantInfo.Id = organizationDb.LocalTenants.FirstOrDefault()?.TenantId.ToString();
                    if (connStr == DemoKey || shouldDemoSeed)
                    {
                        DemoSeeds.DemoSeeds.SeedDemo(organizationDb);
                    }
                    else
                    {
                        SeedNoSave(organizationDb);
                        organizationDb.SaveChanges();
                    }
                }
            }
        }

        public static void SeedNoSave(OrganizationDbContext organizationDb)
        {
            EssentialSeeds.AddEssentialSeeds(organizationDb);
        }
    }
}