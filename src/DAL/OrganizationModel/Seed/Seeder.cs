namespace Tayra.Models.Organizations
{
    public static class Seeder
    {
        public static void SeedAll(IShardMapProvider shardMapProvider, params string[] tenantKeys)
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
