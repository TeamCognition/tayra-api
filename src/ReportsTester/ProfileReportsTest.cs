using System;
using System.Collections.Generic;
using System.Linq;
using Firdaws.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using Tayra.Common;
using Tayra.Models.Organizations;
using Tayra.SyncServices.Common;
using Tayra.SyncServices.Tayra;
using Xunit;

namespace ReportsTester
{
    public class ProfileReportsTest
    {
        private static DateTime startDate = DateTime.UtcNow;
        private List<DateTime> workDays = new List<DateTime>
        {
            startDate,
            startDate.AddDays(1),
            startDate.AddDays(2),
            startDate.AddDays(3),
            startDate.AddDays(1).AddWeeks(1),
            startDate.AddDays(2).AddWeeks(1),
            startDate.AddDays(3).AddWeeks(1),
            startDate.AddDays(1).AddWeeks(2),
            startDate.AddDays(2).AddWeeks(2),
            startDate.AddDays(3).AddWeeks(2),
            startDate.AddDays(1).AddWeeks(3),
            startDate.AddDays(2).AddWeeks(3),
            startDate.AddDays(3).AddWeeks(3),
            startDate.AddDays(1).AddWeeks(4),
            startDate.AddDays(2).AddWeeks(4),
            startDate.AddDays(3).AddWeeks(4),
            startDate.AddDays(1).AddWeeks(5),
            startDate.AddDays(2).AddWeeks(5),
            startDate.AddDays(3).AddWeeks(5),
        };
        
        [Fact]
        public void Test1()
        {
            var options = new DbContextOptionsBuilder<OrganizationDbContext>()
                .UseInMemoryDatabase(databaseName: "report_testing")
                .Options;

            // Run the test against one instance of the context
            using (var context = new OrganizationDbContext(null, null, null))
            {
                Seed(context);
                context.SaveChanges();

                var logService = new LogService(new NullLogger<ProfileReportsTest>());
                var date = workDays.First();
                do
                {
                    GenerateProfileReportsLoader.GenerateProfileReportsDaily(context, date, logService);
                    GenerateProfileReportsLoader.GenerateProfileReportsWeekly(context, date, logService);

                    GenerateProjectReportsLoader.GenerateProjectReportsDaily(context, date, logService);
                    GenerateProjectReportsLoader.GenerateProjectReportsWeekly(context, date, logService);

                    GenerateTeamReportsLoader.GenerateTeamReportsDaily(context, date, logService);
                    GenerateTeamReportsLoader.GenerateTeamReportsWeekly(context, date, logService);

                    date = date.AddDays(1);
                } while (date <= workDays.Last());
            }

            // Use a separate instance of the context to verify correct data was saved to database
            using (var context = new OrganizationDbContext(null, null, null))
            {
                var a = context.ProfileReportsDaily.ToList();

                var s = a.Where(x => x.ProfileId == 1).Sum(x => x.ComplexityChange);
                Assert.Equal(s, a.OrderByDescending(x => x.DateId).First(x => x.ProfileId == 1).ComplexityTotal);

                var wl = context.ProfileReportsWeekly.Where(x => x.ProfileId == 1).ToList();
                foreach (var r in wl)
                {
                    Console.WriteLine("*Date: " + r.DateId);
                    Console.WriteLine("Heat " + r.Heat);
                    Console.WriteLine("Speed " + r.SpeedAverage);
                    Console.WriteLine("OImpact " + r.OImpactAverage);
                    Console.WriteLine("DImpact " + r.DImpactAverage);
                    Console.WriteLine(string.Empty);
                }

            }
        }

        private void Seed(OrganizationDbContext dbContext)
        {
            SeedTokens(dbContext);
            
            var profiles = SeedProfiles(dbContext);
            
            SeedTasks(dbContext, profiles);
            SeedOneUps(dbContext, profiles);

            var teams = SeedTeams(dbContext, profiles);
            SeedProjects(dbContext, profiles, teams);
        }

        private void SeedTokens(OrganizationDbContext dbContext)
        {
            var tokens = new List<Token>
            {
                new Token
                {
                    Id = (int)TokenType.CompanyToken,
                    Type = TokenType.CompanyToken
                },
                new Token
                {
                    Id = (int)TokenType.OneUp,
                    Type = TokenType.OneUp
                }
            };

            dbContext.AddRange(tokens);
        }

        private List<Profile> SeedProfiles(OrganizationDbContext dbContext)
        {            
            var profiles = new List<Profile>
            {
                new Profile
                {
                    Id = 1,
                    FirstName = "Haris",
                    LastName = "Botic",
                    Nickname = "bota"
                },
                new Profile
                {
                    Id = 2,
                    FirstName = "Ejub",
                    LastName = "Hadzic",
                    Nickname = "eji"
                }
            };

            dbContext.AddRange(profiles);
            return profiles;
        }

        private void SeedTasks(OrganizationDbContext dbContext, List<Profile> profiles)
        {
            var p1 = profiles.First().Id;
            var p2 = profiles.Last().Id;

            var tasks = new List<Task>();
            foreach (var date in workDays)
            {
                var dateId = DateHelper2.ToDateId(date);
                var c = 2;
                if(dateId >= 20191125)
                    c = 1;
                tasks.AddRange(new List<Task>
                {
                    new Task
                    {
                        LastModifiedDateId = dateId,
                        AssigneeProfileId = p1,
                        EffortScore = 10.2f,
                        Complexity = c,
                        IsProductionBugFixing = false,
                        IsProductionBugCausing = false,
                        BugSeverity = 2, 
                        BugPopulationAffect = 1,
                        Status = TaskStatuses.Done
                    },
                    new Task
                    {
                        LastModifiedDateId = dateId,
                        AssigneeProfileId = p1,
                        EffortScore = 10.2f,
                        Complexity = c,
                        IsProductionBugFixing = false,
                        IsProductionBugCausing = false,
                        BugSeverity = 2, 
                        BugPopulationAffect = 1,
                        Status = TaskStatuses.Done
                    },
                    new Task
                    {
                        LastModifiedDateId = dateId,
                        AssigneeProfileId = p2,
                        EffortScore = 10.2f,
                        Complexity = c,
                        IsProductionBugFixing = false,
                        IsProductionBugCausing = false,
                        BugSeverity = 2, 
                        BugPopulationAffect = 1,
                        Status = TaskStatuses.Done
                    },
                });
            };

            dbContext.AddRange(tasks);
        }


        private void SeedOneUps(OrganizationDbContext dbContext, List<Profile> profiles)
        {
            var p1 = profiles.First().Id;
            var p2 = profiles.Last().Id;

            var oneUps = new List<ProfileOneUp>();
            foreach (var date in workDays)
            {
                var dateId = DateHelper2.ToDateId(date);
                oneUps.AddRange(new List<ProfileOneUp>
                {
                    new ProfileOneUp
                    {
                        UppedProfileId = p1,
                        CreatedBy = p2,
                        DateId = dateId
                    },
                    new ProfileOneUp
                    {
                        UppedProfileId = p2,
                        CreatedBy = p1,
                        DateId = dateId
                    },
                });
            }

            dbContext.AddRange(oneUps);
        }

        private List<Team> SeedTeams(OrganizationDbContext dbContext, List<Profile> profiles)
        {
            var teams = new List<Team>
            {
                new Team
                {
                    Id = 1
                }
            };

            var teamMembers = new List<TeamMember>();
            foreach (var p in profiles)
            {
                teamMembers.Add(new TeamMember{TeamId = 1, ProfileId = p.Id});
            }

            dbContext.AddRange(teams);
            dbContext.AddRange(teamMembers);

            return teams;
        }

        private void SeedProjects(OrganizationDbContext dbContext, List<Profile> profiles, List<Team> teams)
        {
            var projects = new List<Project>
            {
                new Project
                {
                    Id = 1
                }
            };

            var projectMembers = new List<ProjectMember>();
            foreach (var p in profiles)
            {
                projectMembers.Add(new ProjectMember{ProjectId = 1, ProfileId = p.Id});
            }

            var projectTeams = new List<ProjectTeam>();
            foreach (var t in teams)
            {
                projectTeams.Add(new ProjectTeam{ProjectId = 1, TeamId = t.Id});
            }

            dbContext.AddRange(projects);
            dbContext.AddRange(projectMembers);
            dbContext.AddRange(projectTeams);
        }
    }
}
