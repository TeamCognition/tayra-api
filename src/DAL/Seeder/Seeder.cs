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
        public static void SeedAll(IShardMapProvider shardMapProvider, IConfigurationRoot config)
        {
            using (var catalogDbContext = new CatalogDbContext(ConnectionStringUtilities.GetCatalogDbConnStr(config)))
            {
                var tenantKeys = catalogDbContext.Tenants.Where(x => x.Key != "demo.tayra.io").Select(x => x.Key).ToArray();
                Seed(shardMapProvider, tenantKeys);
            }
        }

        public static void Seed(IShardMapProvider shardMapProvider, params string[] tenantKeys)
        {
            foreach (var tKey in tenantKeys)
            {
                using (var organizationDb = new OrganizationDbContext(null, new ShardTenantProvider(tKey), shardMapProvider))
                {
                    if (tKey == DemoKey)
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
            ItemSeeds.AddShopItemSeeds(organizationDb);
        }

        public static void SeedTasksFromTxt(IShardMapProvider shardMapProvider, string tenantKey)
        {
            using (var organizationDb = new OrganizationDbContext(null, new ShardTenantProvider(tenantKey), shardMapProvider))
            {
                var tasks = File.ReadAllLines(@"./Inputs/tasks.txt");

                List<Task> results = new List<Task>();

                if (!tasks.Any())
                {
                    Console.WriteLine("nisi dobro kopirao");
                    return;
                }

                foreach (var t in tasks)
                //var t = tasks[0];
                {
                    var props = t.Split('\t');
                    for (int i = 0; i < props.Length - 1; i++)
                    {
                        if (props[i] == "NULL")
                            props[i] = null;
                    };
                    Console.WriteLine(t[0]);
                    var x = new Task
                    {
                        ExternalId = props[1],
                        IntegrationType = Enum.Parse<IntegrationType>(props[2]),
                        Summary = props[3],
                        Status = Enum.Parse<TaskStatuses>(props[4]),
                        Type = Enum.Parse<TaskTypes>(props[5]),
                        TimeSpentInMinutes = Parse<int?>(props[6]),
                        TimeOriginalEstimatInMinutes = Parse<int?>(props[7]),
                        StoryPoints = Parse<int?>(props[8]),
                        Complexity = Parse<int>(props[9]),
                        Priority = Enum.Parse<TaskPriorities>(props[10]),
                        BugSeverity = Parse<int?>(props[11]),
                        BugPopulationAffect = 0,
                        IsProductionBugCausing = false,
                        IsProductionBugFixing = false,
                        EffortScore = Parse<int?>(props[15]),
                        Labels = props[16],
                        LastModifiedDateId = Parse<int>(props[17]),
                        ReporterProfileId = Parse<int>(props[18]),
                        AssigneeProfileId = Parse<int>(props[19]),
                        TeamId = Parse<int?>(props[20]),
                        SegmentId = Parse<int?>(props[21]),
                        SegmentAreaId = Parse<int?>(props[22]),
                        TaskCategoryId = Parse<int?>(props[23]),
                        Created = DateTime.Parse(props[24]),
                        LastModified = Parse<DateTime?>(props[25])
                    };
                    x.SegmentId = 55;
                    x.TeamId = 204;

                    if (x.AssigneeProfileId == 3)
                        x.AssigneeProfileId = 30;

                    if (x.AssigneeProfileId == 4)
                        x.AssigneeProfileId = 31;

                    if (x.AssigneeProfileId == 7)
                        x.AssigneeProfileId = 32;

                    if (x.AssigneeProfileId == 9)
                        x.AssigneeProfileId = 33;

                    if (x.AssigneeProfileId == 10)
                        x.AssigneeProfileId = 34;

                    organizationDb.Add(x);
                }
                organizationDb.SaveChanges();
            }
        }
        public static T Parse<T>(object value)
        {
            try { return (T)System.ComponentModel.TypeDescriptor.GetConverter(typeof(T)).ConvertFrom(value.ToString()); }
            catch { return default(T); }
        }

    }
}