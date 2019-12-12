using System.Linq;
using Microsoft.Extensions.Configuration;
using Tayra.DAL;
using Tayra.Models.Catalog;
using Tayra.Models.Organizations;

namespace Tayra.Models.Seeder
{
    public static class Seeder
    {
        public static void SeedAll(IShardMapProvider shardMapProvider, IConfigurationRoot config)
        {
            using (var catalogDbContext = new CatalogDbContext(ConnectionStringUtilities.GetCatalogDbConnStr(config)))
            {
                var tenantKeys = catalogDbContext.Tenants.Select(x => x.Name).ToArray();
                Seed(shardMapProvider, tenantKeys);
            }
        }

        public static void Seed(IShardMapProvider shardMapProvider, params string[] tenantKeys)
        {
            foreach (var tKey in tenantKeys)
            {
                using (var organizationDb = new OrganizationDbContext(null, new ShardTenantProvider(tKey), shardMapProvider))
                {
                    EssentialSeeds.AddEssentialSeeds(organizationDb);
                    ItemSeeds.AddShopItemSeeds(organizationDb);

                    organizationDb.SaveChanges();
                }
            }
        }
    }
}
