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
using Tayra.Connectors.Atlassian;
using Tayra.Connectors.Atlassian.Jira;
using Tayra.Models.Organizations;
using Tayra.Services;
using Tayra.Services.TaskConverters;

namespace Tayra.Models.Seeder.DemoSeeds
{
    public class DemoSeedData
    {
        public Profile[] Profiles { get; set; }
        public Segment[] Segments { get; set; }
        public Team[] Teams { get; set; }
        public ProfileAssignment[] ProfileAssignments { get; set; }
        public Task[] Tasks { get; set; }
        public TokenTransaction[] Transactions { get; set; }
        public ProfileOneUp[] Praises { get; set; }
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
            var finishedTables = new List<string>();
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
        public static void AddOrganizationAndAdminAccount(OrganizationDbContext organizationDb)
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
                FirstName = "Admin",
                LastName = "Demo",
                Username = "admin_demo",
                Role = ProfileRoles.Admin,
                IdentityId = 20
            });
        }
        public static void SeedDemo(OrganizationDbContext organizationDb)
        {
            var demoData = JsonConvert.DeserializeObject<DemoSeedData>(File.ReadAllText("input/demo.json"));

            UnseedDemo(organizationDb);

            //Essentials
            Seeder.SeedNoSave(organizationDb);
            AddOrganizationAndAdminAccount(organizationDb);
            organizationDb.SaveChanges();


            Console.WriteLine("Seeding Profiles ...");
            organizationDb.Profiles.AddRange(demoData.Profiles);
            organizationDb.Database.ExecuteSqlCommand(@"SET IDENTITY_INSERT [dbo].[Profiles] ON");
            organizationDb.SaveChanges();
            organizationDb.Database.ExecuteSqlCommand(@"SET IDENTITY_INSERT [dbo].[Profiles] OFF");

            Console.WriteLine("Seeding Segments ...");
            organizationDb.Segments.AddRange(demoData.Segments);
            foreach (var profile in demoData.Profiles)
            {
                foreach (var segment in demoData.Segments)
                {
                    segment.IsReportingUnlocked = true;
                    organizationDb.Integrations.Add(new Integration {
                        SegmentId = segment.Id,
                        ProfileId = profile.Id,
                        Type = IntegrationType.ATJ,
                        Fields = new List<IntegrationField>() {
                            new IntegrationField {
                                Key = ATConstants.ATJ_REWARD_STATUS_FOR_PROJECT_ + "DemoProject-" + segment.Id,
                                Value = "REWARDING_ID"
                            }
                        }
                    });
                    organizationDb.ProfileExternalIds.Add(new ProfileExternalId {
                        ExternalId = "External-" + profile.Id,
                        IntegrationType = IntegrationType.ATJ,
                        ProfileId = profile.Id,
                        SegmentId = segment.Id
                    });
                }
            }

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
            ITokensService TokensService = new TokensService(organizationDb);
            ILogsService LogsService = new DemoLogsService(organizationDb);
            IProfilesService ProfilesService = new ProfilesService(TokensService, LogsService, null, organizationDb);
            ITasksService TasksService = new TasksService(organizationDb);
            IAssistantService AdvisorService = new AssistantService(organizationDb);
            IInventoriesService InventoryService = new InventoryService(LogsService, TokensService, organizationDb);


            foreach (var t in demoData.Tasks)
            {
                t.LastModifiedDateId = DateHelper2.ToDateId(DateTime.UtcNow.AddDays(rnd.Next(-30, -2)));
                t.Priority = TaskPriorities.Medium;
                t.Status = TaskStatuses.Done;
                t.Type = TaskTypes.Task;

                TaskConverterJira taskConverter = new TaskConverterJira(
                    organizationDb,
                    ProfilesService,
                    new WebhookEvent {
                        JiraIssue = new JiraIssue {
                            Id = "DemoIssue-" + t.Id,
                            Fields = new JiraIssue.IssueFields {
                                Status = new JiraStatus
                                {
                                    Category = new JiraStatusCategory {
                                        Id = IssueStatusCategories.Done
                                    },
                                    Id = "REWARDING_ID",
                                    Name = "Done"
                                },
                                Project = new JiraProject{
                                    Id = "DemoProject-" + t.SegmentId
                                },
                                StoryPointsCF = t.StoryPoints,
                                Summary = t.Summary,
                                TimeOriginalEstimate = t.TimeOriginalEstimatInMinutes * 60,
                                Timespent = t.TimeSpentInMinutes,
                                Assignee = new JiraUser {
                                    AccountId = "External-" + t.AssigneeProfileId
                                },
                                Labels = new string[] {"label-1", "label-2"},
                                StatusCategoryChangeDate = DateHelper2.ParseDate(t.LastModifiedDateId),
                                Priority = new JiraPriority {
                                    Id = "3"
                                },
                                IssueType = new JiraIssueType {
                                    Id = "10003"
                                }
                            },
                            Self = "https://jira.com/filler_because_we_need_characters"
                        }
                    },
                    TaskConverterJiraMode.TEST
                );
                TaskHelpers.DoStandardStuff(taskConverter, TasksService, TokensService, LogsService, AdvisorService);
            }
            // organizationDb.Tasks.AddRange(demoData.Tasks);
            organizationDb.SaveChanges();

            Console.WriteLine("Seeding TokenTransactions ...");
            foreach (var singleTokenTransaction in demoData.Transactions)
            {
                TokensService.CreateTransaction(
                    TokenType.CompanyToken,
                    singleTokenTransaction.ProfileId,
                    singleTokenTransaction.Value,
                    TransactionReason.Manual,
                    null
                );
            }
            organizationDb.SaveChanges();

            Console.WriteLine("Seeding Praises ...");
            var c = -90;
            foreach (var singlePraise in demoData.Praises)
            {
                if (singlePraise.CreatedBy == singlePraise.UppedProfileId)
                    continue;

                ProfilesService.OneUpProfile(singlePraise.CreatedBy, new ProfileOneUpDTO
                {
                    ProfileId = singlePraise.UppedProfileId,
                    DemoDate = DateTime.UtcNow.AddDays(c % -30)
                });
                c++;
            }

            Console.WriteLine("Seeding Inventory items ...");
            var adminProfile = organizationDb.Profiles.FirstOrDefault(x => x.Role == ProfileRoles.Admin);
            var allItems = organizationDb.Items.ToArray();
            foreach (var p in demoData.Profiles)
            {
                var index = rnd.Next(1, allItems.Length - 11);
                var toGive = rnd.Next(1, 10);
                var toGift = rnd.Next(0, toGive);
                for (int i = 0; i < toGive; i++)
                {
                    InventoryService.Give(adminProfile.Id, new InventoryGiveDTO { ItemId = allItems[index + i].Id, ReceiverUsername = p.Username, ClaimRequired = false });
                }
            }
            organizationDb.SaveChanges();
            foreach(var p in demoData.Profiles)
            {
                var invItems = organizationDb.ProfileInventoryItems.Where(x => x.ProfileId == p.Id).ToList();
                var receiverId = rnd.Next(p.Id, demoData.Profiles.Max(x => x.Id));
                if (invItems.Count() == 0 || p.Id == receiverId)
                    continue;

                var toGift = rnd.Next(0, invItems.Count()-1);
                for (int i = 0; i < toGift; i++)
                {
                    InventoryService.Gift(p.Id, new InventoryItemGiftDTO { InventoryItemId = invItems[i].Id, ReceiverId = receiverId });
                }
                invItems.RemoveRange(0, toGift);
                var ownedTitle = invItems.FirstOrDefault(x => x.ProfileId == p.Id && x.ItemType == ItemTypes.TayraTitle);
                var ownedBadge = invItems.FirstOrDefault(x => x.ProfileId == p.Id && x.ItemType == ItemTypes.TayraBadge);

                if (ownedTitle != null)
                    ownedTitle.IsActive = true;

                if (ownedBadge != null)
                    ownedBadge.IsActive = true;
            }

            Console.WriteLine("Seeding Assistent Action Points ...");
            for(int i = 0; i < 3; i ++)
            {
                var index = rnd.Next(1, 5);
                organizationDb.Add(new ActionPoint
                {
                    Type = ActionPointTypes.ProfilesLowImpactFor2Weeks,
                    DateId = DateHelper2.ToDateId(DateTime.UtcNow.AddDays(rnd.Next(-14, -1))),
                    ProfileId = demoData.Profiles[i+index].Id,
                    SegmentId = demoData.Segments[rnd.Next(0,1)].Id,
                });
            }

            for (int i = 0; i < 2; i++)
            {
                var index = rnd.Next(1, 3);
                organizationDb.Add(new ActionPoint
                {
                    Type = ActionPointTypes.ProfilesNoCompletedTasksIn1Week,
                    DateId = DateHelper2.ToDateId(DateTime.UtcNow.AddDays(rnd.Next(-14, -1))),
                    ProfileId = demoData.Profiles[i + index].Id,
                    SegmentId = demoData.Segments[rnd.Next(0, 1)].Id,
                });
            }

            for (int i = 0; i < 4; i++)
            {
                var index = rnd.Next(6, 9);
                organizationDb.Add(new ActionPoint
                {
                    Type = ActionPointTypes.ProfilesHighSpeedFor2Weeks,
                    DateId = DateHelper2.ToDateId(DateTime.UtcNow.AddDays(rnd.Next(-14, -1))),
                    ProfileId = demoData.Profiles[i + index].Id,
                    SegmentId = demoData.Segments[rnd.Next(0, 1)].Id,
                });
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