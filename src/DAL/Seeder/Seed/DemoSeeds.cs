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
                                Value = "DONE"
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
            TokensService Tokens = new TokensService(organizationDb);
            LogsService Logs = new LogsService(organizationDb);
            ProfilesService Profiles = new ProfilesService(Tokens, Logs, null, organizationDb);
            TasksService Tasks = new TasksService(organizationDb);
            AssistantService Assistant = new AssistantService(organizationDb);
            foreach (var t in demoData.Tasks)
            {
                t.LastModifiedDateId = DateHelper2.ToDateId(DateTime.UtcNow.AddDays(rnd.Next(-30, -2)));
                t.Priority = TaskPriorities.Medium;
                t.Status = TaskStatuses.Done;
                t.Type = TaskTypes.Task;

                TaskConverterJira taskConverter = new TaskConverterJira(
                    organizationDb,
                    Profiles,
                    new WebhookEvent {
                        JiraIssue = new JiraIssue {
                            Id = "DemoIssue-" + t.Id,
                            Fields = new JiraIssue.IssueFields {
                                Status = new JiraStatus {
                                    Category = new JiraStatusCategory {
                                        Id = Connectors.Atlassian.IssueStatusCategories.Done,
                                        Name = "Done"
                                    },
                                    Id = "DONE"
                                },
                                Project = new JiraProject {
                                    Id = "DemoProject-" + t.SegmentId
                                },
                                StoryPointsCF = t.StoryPoints,
                                Summary = t.Summary,
                                TimeOriginalEstimate = t.TimeOriginalEstimatInMinutes * 60,
                                Assignee = new JiraUser {
                                    AccountId = "External-" + t.AssigneeProfileId
                                },
                                Labels = new string[] {"label-1", "label-2"},
                                StatusCategoryChangeDate = DateHelper2.ParseDate(t.LastModifiedDateId),
                                Priority = new JiraPriority {
                                    Id = "3"
                                },
                                IssueType = new JiraIssueType {
                                    Id = "123"
                                }
                            },
                            Self = "https://demo.jira.com/this-and-that"
                        }
                    },
                    TaskConverterJiraMode.TEST
                );
                TaskHelpers.DoStandardStuff(taskConverter, Tasks, Tokens, Logs, Assistant);
            }
            // organizationDb.Tasks.AddRange(demoData.Tasks);
            organizationDb.SaveChanges();

            Console.WriteLine("Seeding TokenTransactions ...");
            foreach (var singleTokenTransaction in demoData.Transactions)
            {
                Tokens.CreateTransaction(
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