using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Tayra.Common;
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
                var tenantKeys = catalogDbContext.TenantInfo.Where(x => x.Identifier != DemoKey).Select(x => x.ConnectionString).ToArray();
                Seed(tenantKeys);
            }
        }

        public static void Seed(params string[] tenantConnectionStrings)
        {
            foreach (var connStr in tenantConnectionStrings)
            {
                var tenantInfo = TenantModel.WithConnectionStringOnly(connStr);
                using (var organizationDb = new OrganizationDbContext(tenantInfo, null))
                {
                    if (connStr == DemoKey)
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