using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
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
            List<string> Tables = new List<string>();
            using (var command = context.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = "SELECT name, schema_id FROM sys.tables ORDER BY name";
                context.Database.OpenConnection();
                using (var Result = command.ExecuteReader())
                {
                    foreach (IDataRecord Row in Result)
                    {
                        string TableName = Row.GetString(0);
                        if (!TableName.StartsWith("_") && Row.GetInt32(1) == 1)
                        {
                            Tables.Add(TableName);
                        }
                    }
                }
            }
            bool WasError = true;
            List<String> FinishedTables = new List<String>();
            while (WasError)
            {
                WasError = false;
                foreach (string TableName in Tables)
                {
                    if (FinishedTables.Contains(TableName))
                    {
                        continue;
                    }
                    Console.WriteLine("Deleting " + TableName);
                    try
                    {
                        context.Database.ExecuteSqlCommand("DELETE FROM " + TableName);
                        FinishedTables.Add(TableName);
                    } catch(SqlException)
                    {
                        WasError = true;
                    }
                }
            }
        }
        public static void AddDemoAccount(OrganizationDbContext organizationDb)
        {
            Console.WriteLine("Seeding demo account");
            var AddedOrganization = organizationDb.Organizations.Add(new Organization() {
                Name = "Demo organization",
                Address = "Some street 123",
                Id = TenantUtilities.GenerateShardingKey(Seeder.DemoKey)
            });
            organizationDb.SaveChanges();
            organizationDb.Profiles.Add(new Profile() {
                FirstName = "Demo",
                LastName = "Account",
                Username = "demoaccount",
                Role = ProfileRoles.Manager,
                IdentityId = 1
            });
        }
        public static void SeedDemo(OrganizationDbContext organizationDb)
        {
            UnseedDemo(organizationDb);
            Seeder.SeedNoSave(organizationDb);
            AddDemoAccount(organizationDb);
            organizationDb.SaveChanges();
            DemoSeedData DemoData = JsonConvert.DeserializeObject<DemoSeedData>(File.ReadAllText("input/demo.json"));
            Console.WriteLine("Seeding Profiles ...");
            
            foreach (Profile SingleProfile in DemoData.Profiles)
            {
                organizationDb.Profiles.Add(SingleProfile);
            }
            organizationDb.Database.ExecuteSqlCommand(@"SET IDENTITY_INSERT [dbo].[Profiles] ON");
            organizationDb.SaveChanges();
            organizationDb.Database.ExecuteSqlCommand(@"SET IDENTITY_INSERT [dbo].[Profiles] OFF");

            Console.WriteLine("Seeding Segments ...");
            foreach (Segment SingleSegment in DemoData.Segments)
            {
                organizationDb.Segments.Add(SingleSegment);
            }
            organizationDb.Database.ExecuteSqlCommand(@"SET IDENTITY_INSERT [dbo].[Segments] ON");
            organizationDb.SaveChanges();
            organizationDb.Database.ExecuteSqlCommand(@"SET IDENTITY_INSERT [dbo].[Segments] OFF");

            Console.WriteLine("Seeding Teams ...");
            foreach (Team SingleTeam in DemoData.Teams)
            {
                organizationDb.Teams.Add(SingleTeam);
            }
            organizationDb.Database.ExecuteSqlCommand(@"SET IDENTITY_INSERT [dbo].[Teams] ON");
            organizationDb.SaveChanges();
            organizationDb.Database.ExecuteSqlCommand(@"SET IDENTITY_INSERT [dbo].[Teams] OFF");

            Console.WriteLine("Seeding ProfileAssignments ...");
            foreach (ProfileAssignment SingleProfileAssignment in DemoData.ProfileAssignments)
            {
                organizationDb.ProfileAssignments.Add(SingleProfileAssignment);
            }
            Console.WriteLine("Seeding Tasks ...");
            foreach (Task SingleTask in DemoData.Tasks)
            {
                organizationDb.Tasks.Add(SingleTask);
            }
            Console.WriteLine("Seeding TokenTransactions ...");
            TokensService Service = new TokensService(organizationDb);
            foreach (TokenTransaction SingleTokenTransaction in DemoData.Transactions)
            {
                Service.CreateTransaction(
                    TokenType.OneUp,
                    SingleTokenTransaction.ProfileId,
                    SingleTokenTransaction.Value,
                    TransactionReason.OneUpProfile,
                    null
                );
            }
            organizationDb.SaveChanges();

            Console.WriteLine("Unlocking reporting");
            foreach (Segment SingleSegment in organizationDb.Segments.ToList())
            {
                new ReportsService(organizationDb).UnlockReporting(Seeder.DemoKey, SingleSegment.Id);
            }
            organizationDb.SaveChanges();
        }
    }
}