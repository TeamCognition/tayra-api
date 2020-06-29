using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using Cog.Core;
using Microsoft.EntityFrameworkCore;
using MoreLinq;
using Newtonsoft.Json;
using Tayra.Common;
using Tayra.Connectors.Atlassian;
using Tayra.Connectors.Atlassian.Jira;
using Tayra.Connectors.Common;
using Tayra.Models.Organizations;
using Tayra.Services;
using Tayra.Services.TaskConverters;

namespace Tayra.Models.Seeder.DemoSeeds
{
    public class ProfileAssignmentDemo
    {
        public int TeamId { get; set; }
        public int MembersCount { get; set; }

    }
    public class DemoSeedData
    {
        public Profile[] Profiles { get; set; }
        public Segment[] Segments { get; set; }
        public Team[] Teams { get; set; }
        public ProfileAssignmentDemo[] ProfileAssignmentDemos { get; set; }
        public Task[] Tasks { get; set; }
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
                        //context.Database.ExecuteSqlInterpolated($"DELETE FROM {tableName}");
                        context.Database.ExecuteSqlCommand($"DELETE FROM {tableName}", tableName);
                        finishedTables.Add(tableName);
                    } catch(SqlException e)
                    {
                        Console.WriteLine("Error on unseed: " + e.Message);
                        wasError = true;
                    }
                }
            }
        }
        public static void AddOrganization(OrganizationDbContext organizationDb)
        {
            Console.WriteLine("Seeding demo account");
            organizationDb.Add(new Organization
            {
                Name = "Demo organization",
                Address = "Street demo 123",
                Id = TenantUtilities.GenerateShardingKey(Seeder.DemoKey)
            });
            organizationDb.SaveChanges();
        }

        public static void SeedDemo(OrganizationDbContext organizationDb)
        {
            var demoData = JsonConvert.DeserializeObject<DemoSeedData>(File.ReadAllText("input/demo.json"));
            var demoTaskNames = File.ReadAllLines("input/demo-task-names.txt");

            UnseedDemo(organizationDb);

            //Essentials
            Seeder.SeedNoSave(organizationDb);
            AddOrganization(organizationDb);
            organizationDb.SaveChanges();


            Console.WriteLine("Seeding Profiles ...");
            organizationDb.Profiles.AddRange(demoData.Profiles);
            //organizationDb.Database.ExecuteSqlRaw(@"SET IDENTITY_INSERT [dbo].[Profiles] ON");
            organizationDb.Database.ExecuteSqlCommand(@"SET IDENTITY_INSERT [dbo].[Profiles] ON");
            organizationDb.SaveChanges();
            //organizationDb.Database.ExecuteSqlRaw(@"SET IDENTITY_INSERT [dbo].[Profiles] OFF");
            organizationDb.Database.ExecuteSqlCommand(@"SET IDENTITY_INSERT [dbo].[Profiles] OFF");
            demoData.Profiles = demoData.Profiles.Where(x => x.Role == ProfileRoles.Member).ToArray();

            Console.WriteLine("Seeding Segments ...");
            organizationDb.Segments.AddRange(demoData.Segments);
            foreach (var profile in demoData.Profiles)
            {
                foreach (var segment in demoData.Segments)
                {
                    organizationDb.Integrations.Add(new Integration
                    {
                        SegmentId = segment.Id,
                        ProfileId = profile.Id,
                        Type = IntegrationType.ATJ,
                        Fields = new List<IntegrationField>() {
                            new IntegrationField {
                                Key = ATConstants.ATJ_REWARD_STATUS_FOR_PROJECT_ + "DemoProject-" + segment.Id,
                                Value = "REWARDING_ID"
                            },
                            new IntegrationField {
                                Key = Constants.PROFILE_EXTERNAL_ID,
                                Value = "ExternalIdFor" + segment.Id
                            }
                        }
                    });
                    organizationDb.ProfileExternalIds.Add(new ProfileExternalId
                    {
                        ExternalId = "External-" + profile.Id,
                        IntegrationType = IntegrationType.ATJ,
                        ProfileId = profile.Id,
                        SegmentId = segment.Id
                    });
                }
            }

            foreach (var segment in demoData.Segments)
            {
                segment.IsReportingUnlocked = true;

                organizationDb.Integrations.Add(new Integration
                {
                    SegmentId = segment.Id,
                    ProfileId = null,
                    Type = IntegrationType.ATJ,
                    Fields = new List<IntegrationField>() {
                            new IntegrationField {
                                Key = ATConstants.ATJ_REWARD_STATUS_FOR_PROJECT_ + "DemoProject-" + segment.Id,
                                Value = "REWARDING_ID"
                            }
                        }
                });
            }

            //organizationDb.Database.ExecuteSqlRaw(@"SET IDENTITY_INSERT [dbo].[Segments] ON");
            organizationDb.Database.ExecuteSqlCommand(@"SET IDENTITY_INSERT [dbo].[Segments] ON");
            organizationDb.SaveChanges();
            //organizationDb.Database.ExecuteSqlRaw(@"SET IDENTITY_INSERT [dbo].[Segments] OFF");
            organizationDb.Database.ExecuteSqlCommand(@"SET IDENTITY_INSERT [dbo].[Segments] OFF");

            Console.WriteLine("Seeding Teams ...");
            organizationDb.Teams.AddRange(demoData.Teams);

            //organizationDb.Database.ExecuteSqlRaw(@"SET IDENTITY_INSERT [dbo].[Teams] ON");
            organizationDb.Database.ExecuteSqlCommand(@"SET IDENTITY_INSERT [dbo].[Teams] ON");
            organizationDb.SaveChanges();
            //organizationDb.Database.ExecuteSqlRaw(@"SET IDENTITY_INSERT [dbo].[Teams] OFF");
            organizationDb.Database.ExecuteSqlCommand(@"SET IDENTITY_INSERT [dbo].[Teams] OFF");

            Console.WriteLine("Seeding ProfileAssignments ...");
            demoData.ProfileAssignmentDemos = demoData.ProfileAssignmentDemos.DistinctBy(x => new { x.TeamId }).ToArray();
            if (demoData.ProfileAssignmentDemos.Sum(x => x.MembersCount) > demoData.Profiles.Length)
            {
                throw new Exception("Not enough members for Profile Assignments");
            }
            var profileAssignmentsData = new List<ProfileAssignment>();
            {
                int toSkip = 0;
                for (int i = 0; i < demoData.ProfileAssignmentDemos.Length; i++)
                {
                    var pad = demoData.ProfileAssignmentDemos[i];
                    var segmentId = demoData.Teams.First(x => x.Id == pad.TeamId).SegmentId;
                    profileAssignmentsData.AddRange(demoData.Profiles.Skip(toSkip).Take(pad.MembersCount).Select(x => new ProfileAssignment
                    {
                        TeamId = pad.TeamId,
                        SegmentId = segmentId,
                        ProfileId = x.Id
                    }));
                    toSkip += pad.MembersCount;
                }
            }

            var unassignedProfiles = demoData.Profiles.Where(x => !profileAssignmentsData.Select(a => a.ProfileId).Contains(x.Id)).Select(x => new ProfileAssignment
            {
                ProfileId = x.Id,
                SegmentId = demoData.Segments[0].Id,
                TeamId = demoData.Teams[0].Id,
            });

            organizationDb.ProfileAssignments.AddRange(profileAssignmentsData.Concat(unassignedProfiles));
            organizationDb.SaveChanges();


            Console.WriteLine("Seeding Tasks ...");
            Random rnd = new Random();
            ITokensService TokensService = new DemoTokensService(organizationDb);
            ILogsService LogsService = new DemoLogsService(organizationDb);
            IProfilesService ProfilesService = new ProfilesService(TokensService, LogsService, null, organizationDb);
            ITasksService TasksService = new TasksService(organizationDb);
            IAssistantService AdvisorService = new AssistantService(organizationDb);
            IInventoriesService InventoryService = new InventoryService(LogsService, TokensService, organizationDb);
            IShopItemsService ShopItemsService = new ShopItemsService(LogsService, TokensService, organizationDb);
            IItemsService ItemService = new ItemsService(organizationDb);
            IPraiseService PraiseService = new PraiseService(TokensService, LogsService, organizationDb);

            int taskCounter = -1;
            foreach (var t in demoData.Tasks.Concat(demoData.Tasks.Take(40)))
            {
                taskCounter++;
                t.LastModifiedDateId = DateHelper2.ToDateId(GetRandomDateTimeInPast());
                t.Priority = TaskPriorities.Medium;
                t.Status = TaskStatuses.Done;
                t.Type = TaskTypes.Task;

                TaskConverterJira taskConverter = new TaskConverterJira(
                    organizationDb,
                    ProfilesService,
                    new WebhookEvent
                    {
                        JiraIssue = new JiraIssue
                        {
                            Id = "DemoIssue-" + t.Id,
                            Key = "TSK-" + rnd.Next(101, 9998),
                            Fields = new JiraIssue.IssueFields
                            {
                                Status = new JiraStatus
                                {
                                    Category = new JiraStatusCategory
                                    {
                                        Id = IssueStatusCategories.Done
                                    },
                                    Id = "REWARDING_ID",
                                    Name = "Done"
                                },
                                Project = new JiraProject
                                {
                                    Id = "DemoProject-" + t.SegmentId
                                },
                                StoryPointsCF = t.StoryPoints,
                                Summary = demoTaskNames[taskCounter % demoTaskNames.Length],
                                TimeOriginalEstimate = t.TimeOriginalEstimatInMinutes * 60,
                                Timespent = t.TimeSpentInMinutes * 60,
                                Assignee = new JiraUser
                                {
                                    AccountId = "External-" + demoData.Profiles[rnd.Next(demoData.Profiles.Length)].Id // lenght-1??
                                },
                                Labels = new string[] { "label-1", "label-2" },
                                StatusCategoryChangeDate = DateHelper2.ParseDate(t.LastModifiedDateId),
                                Priority = new JiraPriority
                                {
                                    Id = "3"
                                },
                                IssueType = new JiraIssueType
                                {
                                    Id = "10003"
                                }
                            },
                            Self = "https://jira.com/filler_because_we_need_characters"
                        }
                    },
                    TaskConverterMode.TEST
                );
                TaskHelpers.DoStandardStuff(taskConverter, TasksService, TokensService, LogsService, AdvisorService);
            }
            organizationDb.SaveChanges();

            Console.WriteLine("Seeding TokenTransactions ...");
            foreach (var p in demoData.Profiles)
            {
                TokensService.CreateTransaction(
                    TokenType.CompanyToken,
                    p.Id,
                    rnd.Next(1500, 5500),
                    TransactionReason.Manual,
                    null,
                    new DateTime()
                );
            }
            organizationDb.SaveChanges();

            Console.WriteLine("Seeding Praises ...");
            foreach (var p in demoData.Profiles)
            {
                foreach (var pToPraise in demoData.Profiles.RandomSubset(rnd.Next(10)))
                {
                    if (p.Id == pToPraise.Id)
                        continue;

                    PraiseService.PraiseProfile(p.Id, new PraiseProfileDTO
                    {
                        ProfileId = pToPraise.Id,
                        DemoDate = GetRandomDateTimeInPast()
                    });
                }
            }

            Console.WriteLine("Seeding Inventory items ...");
            var adminProfile = organizationDb.Profiles.FirstOrDefault(x => x.Role == ProfileRoles.Admin);
            var allItems = organizationDb.Items.ToArray();
            foreach (var p in demoData.Profiles)
            {
                var toGive = rnd.Next(1, 10);
                foreach (var item in allItems.RandomSubset(toGive))
                {
                    InventoryService.Give(adminProfile.Id, new InventoryGiveDTO { ItemId = item.Id, ReceiverUsername = p.Username, ClaimRequired = false });
                }
            }

            organizationDb.SaveChanges();
            foreach (var p in demoData.Profiles)
            {
                var invItems = organizationDb.ProfileInventoryItems.Where(x => x.ProfileId == p.Id).ToList();
                var receiverId = rnd.Next(p.Id, demoData.Profiles.Max(x => x.Id));
                if (invItems.Count() == 0 || p.Id == receiverId)
                    continue;

                var toGift = rnd.Next(0, invItems.Count() - 1);
                for (int i = 0; i < toGift; i++)
                {
                    InventoryService.Gift(p.Id, new InventoryItemGiftDTO { InventoryItemId = invItems[i].Id, ReceiverId = receiverId, ClaimRequired = false, DemoDate = GetRandomDateTimeInPast() });
                }
                invItems.RemoveRange(0, toGift);
                var ownedTitle = invItems.FirstOrDefault(x => x.ProfileId == p.Id && x.ItemType == ItemTypes.TayraTitle);
                var ownedBadge = invItems.FirstOrDefault(x => x.ProfileId == p.Id && x.ItemType == ItemTypes.TayraBadge);

                if (ownedTitle != null)
                    ownedTitle.IsActive = true;

                if (ownedBadge != null)
                    ownedBadge.IsActive = true;
            }

            {
                Console.WriteLine("Seeding Shop data ...");
                var shopItems = organizationDb.ShopItems.Where(x => x.Item.ShopQuantityRemaining == null).ToArray();
                foreach (var p in demoData.Profiles)
                {
                    foreach (var toBuy in shopItems.RandomSubset(rnd.Next(3)))
                    {
                        ShopItemsService.PurchaseShopItem(p.Id, new ShopItemPurchaseDTO
                        {
                            ItemId = toBuy.ItemId,
                            DemoDate = GetRandomDateTimeInPast()
                        });
                    }
                }

                var customItems = new List<Item>();

                customItems.Add(ItemService.CreateItem(new ItemCreateDTO
                {
                    Name = "Company T-Shirt",
                    Image = "https://tayra.blob.core.windows.net/demo-images/item-tshirt.jpg",
                    Type = ItemTypes.PhysicalGood,
                    Price = 200,
                    Description = "A T-shirt is a style of fabric shirt named after the T shape of its body and sleeves. Traditionally it has short sleeves and a round neckline, known as a crew neck, which lacks a collar. T-shirts are generally made of a stretchy, light and inexpensive fabric and are easy to clean.",
                    IsActivable = false,
                    IsGiftable = true,
                    IsDisenchantable = false,
                    PlaceInShop = true,
                    GiveawayQuantityRemaining = 10,
                    ShopQuantityRemaining = 5,
                    Rarity = ItemRarities.Common
                }));
                customItems.Add(ItemService.CreateItem(new ItemCreateDTO
                {
                    Name = "Family Trip",
                    Image = "https://tayra.blob.core.windows.net/demo-images/item-trip.jpg",
                    Type = ItemTypes.Digital,
                    Price = 1500,
                    Description = "Travel is the movement of people between distant geographical locations.",
                    IsActivable = true,
                    IsGiftable = false,
                    IsDisenchantable = false,
                    PlaceInShop = true,
                    GiveawayQuantityRemaining = 2,
                    ShopQuantityRemaining = 2,
                    Rarity = ItemRarities.Legendary
                }));
                customItems.Add(ItemService.CreateItem(new ItemCreateDTO
                {
                    Name = "Xbox One X",
                    Image = "https://tayra.blob.core.windows.net/demo-images/item-xbox.jpg",
                    Type = ItemTypes.PhysicalGood,
                    Price = 999,
                    Description = "The Xbox One is an eighth-generation home video game console developed by Microsoft.",
                    IsActivable = true,
                    IsGiftable = false,
                    IsDisenchantable = false,
                    PlaceInShop = true,
                    GiveawayQuantityRemaining = 4,
                    ShopQuantityRemaining = 4,
                    Rarity = ItemRarities.Legendary
                }));

                organizationDb.SaveChanges();
                foreach (var ci in customItems)
                {
                    ci.Created = GetRandomDateTimeInPast();
                    ci.CreatedBy = 1;
                    ShopItemsService.PurchaseShopItem(demoData.Profiles[0].Id, new ShopItemPurchaseDTO { ItemId = ci.Id, DemoDate = GetRandomDateTimeInPast() });
                }
            }

            Console.WriteLine("Seeding Assistent Action Points ...");
            for (int i = 0; i < 3; i++)
            {
                var index = rnd.Next(1, 5);
                organizationDb.Add(new ActionPoint
                {
                    Type = ActionPointTypes.ProfilesLowImpactFor2Weeks,
                    DateId = DateHelper2.ToDateId(GetRandomDateTimeInPast(14)),
                    ProfileId = demoData.Profiles[i + index].Id,
                    SegmentId = demoData.Segments[rnd.Next(0, 1)].Id,
                });
            }

            for (int i = 0; i < 2; i++)
            {
                var index = rnd.Next(1, 3);
                organizationDb.Add(new ActionPoint
                {
                    Type = ActionPointTypes.ProfilesNoCompletedTasksIn1Week,
                    DateId = DateHelper2.ToDateId(GetRandomDateTimeInPast(14)),
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
                    DateId = DateHelper2.ToDateId(GetRandomDateTimeInPast(14)),
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

            DateTime GetRandomDateTimeInPast(int maxDaysInPast = 30) =>
                DateTime.UtcNow.Date.Subtract(new TimeSpan(rnd.Next(0, maxDaysInPast - 1), rnd.Next(0, 23), rnd.Next(0, 59), rnd.Next(0, 59)));

        }
    }
}