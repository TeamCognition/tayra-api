using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using Firdaws.Core;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Tayra.Common;
using Tayra.Models.Organizations;
using Tayra.Services;

namespace Tayra.Models.Seeder.DemoSeeds
{
    public class DemoSeedData
    {
        public Profile[] Profiles {get; set;}
        public Segment[] Segments {get; set;}
        public Team[] Teams {get; set;}
        public ProfileAssignment[] ProfileAssignments {get; set;}
        public Task[] Tasks {get; set;}
        public TokenTransaction[] Transactions {get; set;}
    }
    
    public static class DemoSeeds
    {
        public static void UnseedDemo(OrganizationDbContext context)
        {
            List<string> tables = new List<string>();
            using (var command = context.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = "SELECT name, schema_id FROM sys.tables ORDER BY name";
                context.Database.OpenConnection();
                using (var result = command.ExecuteReader())
                {
                    foreach (IDataRecord row in result)
                    {
                        string TableName = row.GetString(0);
                        if (!TableName.StartsWith("_") && row.GetInt32(1) == 1)
                        {
                            tables.Add(TableName);
                        }
                    }
                }
            }
            bool wasError = true;
            var finishedTables = new List<String>();
            while (wasError)
            {
                wasError = false;
                foreach (string tableName in tables)
                {
                    if (finishedTables.Contains(tableName))
                    {
                        continue;
                    }
                    Console.WriteLine("Deleting " + tableName);
                    try
                    {
                        context.Database.ExecuteSqlCommand("DELETE FROM " + tableName);
                        finishedTables.Add(tableName);
                    } catch(SqlException)
                    {
                        wasError = true;
                    }
                }
            }
        }
        public static void AddDemoAccount(OrganizationDbContext organizationDb)
        {
            Console.WriteLine("Seeding demo account");
            organizationDb.Add(new Organization()
            {
                Name = "Demo organization",
                Address = "Some street 123",
                Id = TenantUtilities.GenerateShardingKey(Seeder.DemoKey)
            });
            organizationDb.SaveChanges();
            organizationDb.Profiles.Add(new Profile()
            {
                FirstName = "Demo",
                LastName = "Account",
                Username = "demoaccount",
                Role = ProfileRoles.Admin,
                IdentityId = 20
            });
        }
        public static void SeedDemo(OrganizationDbContext organizationDb)
        {
            var demoData = JsonConvert.DeserializeObject<DemoSeedData>(File.ReadAllText("input/demo.json"));

            UnseedDemo(organizationDb);

            Seeder.SeedNoSave(organizationDb);
            AddDemoAccount(organizationDb);
            organizationDb.SaveChanges();


            Console.WriteLine("Seeding Profiles ...");
            organizationDb.Profiles.AddRange(demoData.Profiles);

            organizationDb.Database.ExecuteSqlCommand(@"SET IDENTITY_INSERT [dbo].[Profiles] ON");
            organizationDb.SaveChanges();
            organizationDb.Database.ExecuteSqlCommand(@"SET IDENTITY_INSERT [dbo].[Profiles] OFF");

            Console.WriteLine("Seeding Segments ...");
            foreach (var segment in demoData.Segments)
            {
                segment.IsReportingUnlocked = true;
            }
            organizationDb.Segments.AddRange(demoData.Segments);

            organizationDb.Database.ExecuteSqlCommand(@"SET IDENTITY_INSERT [dbo].[Segments] ON");
            organizationDb.SaveChanges();
            organizationDb.Database.ExecuteSqlCommand(@"SET IDENTITY_INSERT [dbo].[Segments] OFF");

            Console.WriteLine("Seeding Teams ...");
            organizationDb.Teams.AddRange(demoData.Teams);

            organizationDb.Database.ExecuteSqlCommand(@"SET IDENTITY_INSERT [dbo].[Teams] ON");
            organizationDb.SaveChanges();
            organizationDb.Database.ExecuteSqlCommand(@"SET IDENTITY_INSERT [dbo].[Teams] OFF");

            Console.WriteLine("Seeding ProfileAssignments ...");
            organizationDb.ProfileAssignments.AddRange(demoData.ProfileAssignments);

            Console.WriteLine("Seeding Tasks ...");
            Random rnd = new Random();
            foreach (var t in demoData.Tasks)
            {
                t.LastModifiedDateId = DateHelper2.ToDateId(DateTime.UtcNow.AddDays(rnd.Next(-30, -2)));
                t.Priority = TaskPriorities.Medium;
                t.Status = TaskStatuses.Done;
                t.Type = TaskTypes.Task;
            }
            organizationDb.Tasks.AddRange(demoData.Tasks);

            Console.WriteLine("Seeding TokenTransactions ...");
            TokensService Service = new TokensService(organizationDb);
            foreach (var singleTokenTransaction in demoData.Transactions)
            {
                Service.CreateTransaction(
                    TokenType.OneUp,
                    singleTokenTransaction.ProfileId,
                    singleTokenTransaction.Value,
                    TransactionReason.OneUpProfile,
                    null
                );
            }
            organizationDb.SaveChanges();

            Console.WriteLine("Unlocking reporting");
            foreach (var segment in organizationDb.Segments.ToArray())
            {
                new ReportsService(organizationDb).UnlockReporting(Seeder.DemoKey, segment.Id);
            }
            organizationDb.SaveChanges();
        }
    }
}