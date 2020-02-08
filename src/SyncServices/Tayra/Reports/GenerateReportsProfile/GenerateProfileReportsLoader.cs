using System;
using System.Collections.Generic;
using System.Linq;
using Firdaws.Core;
using Microsoft.EntityFrameworkCore;
using MoreLinq;
using Tayra.Common;
using Tayra.Models.Catalog;
using Tayra.Models.Organizations;
using Tayra.SyncServices.Common;

namespace Tayra.SyncServices.Tayra
{
    public class GenerateProfileReportsLoader : BaseLoader
    {
        #region Private Variables

        private readonly IShardMapProvider _shardMapProvider;

        #endregion

        #region Constructor

        public GenerateProfileReportsLoader(IShardMapProvider shardMapProvider, LogService logService, CatalogDbContext catalogDb) : base(logService, catalogDb)
        {
            _shardMapProvider = shardMapProvider;
        }

        #endregion

        #region Public Methods

        public override void Execute(DateTime date, params Tenant[] tenants)
        {
            foreach (var tenant in tenants)
            {
                LogService.SetOrganizationId(tenant.Name);
                using (var organizationDb = new OrganizationDbContext(null, new ShardTenantProvider(tenant.Name), _shardMapProvider))
                {
                    GenerateProfileReportsDaily(organizationDb, date, LogService);
                }
            }
        }


        #endregion

        #region Private Methods

        public static List<ProfileReportDaily> GenerateProfileReportsDaily(OrganizationDbContext organizationDb, DateTime fromDay, LogService logService)
        {
            var reportsToInsert = new List<ProfileReportDaily>();

            var dateId = DateHelper2.ToDateId(fromDay);
            var taskStats = (from t in organizationDb.Tasks
                             where t.Status == TaskStatuses.Done
                             group t by t.AssigneeProfileId into total
                             let change = total.Where(x => x.LastModifiedDateId == dateId)
                             select new
                             {
                                 ProfileId = total.Key,

                                 EffortScore = change.Sum(x => x.EffortScore),
                                 EffortScoreTotal = total.Sum(x => x.EffortScore),

                                 Complexity = change.Sum(x => x.Complexity),
                                 ComplexityTotal = total.Sum(x => x.Complexity),

                                 Completed = change.Count(),
                                 CompletedTotal = total.Count(),

                                 ProductionBugsFixed = change.Count(x => x.IsProductionBugFixing),
                                 ProductionBugsFixedTotal = total.Count(x => x.IsProductionBugFixing),

                                 ProductionBugsCaused = change.Count(x => x.IsProductionBugCausing),
                                 ProductionBugsCausedTotal = total.Count(x => x.IsProductionBugCausing),

                                 Errors = change.Where(x => x.IsProductionBugCausing).Sum(x => x.BugSeverity * x.BugPopulationAffect),
                                 ErrorsTotal = total.Where(x => x.IsProductionBugCausing).Sum(x => x.BugSeverity * x.BugPopulationAffect),

                                 Saves = change.Count(x => x.IsProductionBugFixing && x.BugSeverity > 3),
                                 SavesTotal = total.Count(x => x.IsProductionBugFixing && x.BugSeverity > 3),

                                 Turnovers = 0,
                                 TurnoversTotal = 0,

                                 Tackles = 0,
                                 TacklesTotal = 0,

                                 MinutesSpent = change.Sum(x => x.TimeSpentInMinutes),
                                 MinutesSpentTotal = total.Sum(x => x.TimeSpentInMinutes)
                             }).ToList();

            var companyTokenId = organizationDb.Tokens.Where(x => x.Type == TokenType.CompanyToken).Select(x => x.Id).FirstOrDefault();
            if (companyTokenId == 0)
                throw new Exception("COMPANY TOKEN NOT FOUND");

            var tokens = (from tt in organizationDb.TokenTransactions
                          group tt by tt.ProfileId into total
                          let change = total.Where(x => x.Created.Date == fromDay.Date)
                          select new
                          {
                              ProfileId = total.Key,
                              CompanyTokens = change.Where(x => x.TokenId == companyTokenId).Sum(x => x.Value),
                              CompanyTokensTotal = total.Where(x => x.TokenId == companyTokenId).Sum(x => x.Value)
                          }).ToList();

            var oneUpsGiven = (from u in organizationDb.ProfileOneUps
                               group u by u.CreatedBy into total
                               let change = total.Where(x => x.DateId == dateId)
                               select new
                               {
                                   ProfileId = total.Key,

                                   Count = change.Count(),
                                   CountTotal = total.Count()
                               })
                                .ToList();

            var oneUpsReceived = (from u in organizationDb.ProfileOneUps
                                  group u by u.UppedProfileId into total
                                  let change = total.Where(x => x.DateId == dateId)
                                  select new
                                  {
                                      ProfileId = total.Key,

                                      Count = change.Count(),
                                      CountTotal = total.Count()
                                  })
                                    .ToList();

            var profiles = organizationDb.Profiles.Select(x => new { x.Id, x.Role }).ToList();
            foreach (var p in profiles)
            {
                var ts = taskStats.FirstOrDefault(x => x.ProfileId == p.Id);
                var t = tokens.FirstOrDefault(x => x.ProfileId == p.Id);
                var upsG = oneUpsGiven.FirstOrDefault(x => x.ProfileId == p.Id);
                var upsR = oneUpsReceived.FirstOrDefault(x => x.ProfileId == p.Id);
                var iterationCount = 1;

                reportsToInsert.Add(new ProfileReportDaily
                {
                    ProfileId = p.Id,
                    ProfileRole = p.Role,
                    DateId = dateId,
                    IterationCount = iterationCount,
                    TaskCategoryId = 1,

                    ComplexityChange = ts?.Complexity ?? 0,
                    ComplexityTotal = ts?.ComplexityTotal ?? 0,

                    CompanyTokensChange = (float)(t?.CompanyTokens ?? 0),
                    CompanyTokensTotal = (float)(t?.CompanyTokensTotal ?? 0),

                    EffortScoreChange = ts?.EffortScore ?? 0f,
                    EffortScoreTotal = ts?.EffortScoreTotal ?? 0f,

                    OneUpsGivenChange = upsG?.Count ?? 0,
                    OneUpsGivenTotal = upsG?.CountTotal ?? 0,

                    OneUpsReceivedChange = upsR?.Count ?? 0,
                    OneUpsReceivedTotal = upsR?.CountTotal ?? 0,

                    AssistsChange = (upsR?.Count) ?? 0,
                    AssistsTotal = (upsR?.CountTotal) ?? 0,

                    TasksCompletedChange = ts?.Completed ?? 0,
                    TasksCompletedTotal = ts?.CompletedTotal ?? 0,

                    TurnoverChange = ts?.Turnovers ?? 0,
                    TurnoverTotal = ts?.TurnoversTotal ?? 0,

                    ErrorChange = ts?.Errors ?? 0,
                    ErrorTotal = ts?.ErrorsTotal ?? 0,

                    ContributionChange = (ts?.Complexity - ts?.Turnovers - ts?.Errors) ?? 0,
                    ContributionTotal = (ts?.ComplexityTotal - ts?.TurnoversTotal - ts?.ErrorsTotal) ?? 0,

                    SavesChange = ts?.Saves ?? 0,
                    SavesTotal = ts?.SavesTotal ?? 0,

                    TacklesChange = ts?.Tackles ?? 0,
                    TacklesTotal = ts?.TacklesTotal ?? 0,
                    
                    TasksCompletionTimeChange = ts?.MinutesSpent ?? 0,
                    TasksCompletionTimeTotal = ts?.MinutesSpentTotal ?? 0,
                });
            }

            var existing = organizationDb.ProfileReportsDaily.Count(x => x.DateId == dateId);
            if (existing > 0)
            {
                logService.Log<ProfileReportDaily>($"deleting {existing} records from database");
                organizationDb.Database.ExecuteSqlCommand($"delete from ProfileReportsDaily where {nameof(ProfileReportDaily.DateId)} = {dateId}", dateId); //this extra parameter is a workaround in ef 2.2
                organizationDb.SaveChanges();
            }

            organizationDb.ProfileReportsDaily.AddRange(reportsToInsert);
            organizationDb.SaveChanges();

            logService.Log<GenerateProfileReportsLoader>($"{reportsToInsert.Count} new profile reports saved to database.");
            return reportsToInsert;
        }

        public static List<ProfileReportWeekly> GenerateProfileReportsWeekly(OrganizationDbContext organizationDb, DateTime fromDay, LogService logService)
        {
            if (!CommonHelper.IsMonday(fromDay))
                return null;


            var reportsToInsert = new List<ProfileReportWeekly>();

            var iterationDays = 7;
            var dateId = DateHelper2.ToDateId(fromDay);
            var dateId1ago = DateHelper2.ToDateId(fromDay.AddDays(-iterationDays));
            var dateId2ago = DateHelper2.ToDateId(fromDay.AddDays(-iterationDays * 2));
            var dateId4ago = DateHelper2.ToDateId(fromDay.AddDays(-iterationDays * 4));

            reportsToInsert = (from d in organizationDb.ProfileReportsDaily
                               where d.DateId <= dateId
                               group d by d.ProfileId into dg
                               let dgCount = dg.Count()
                               //let dgCountWorked = dg.Count()
                               let last1 = dg.Where(x => x.DateId > dateId1ago) //last 1 iteration
                               let last4 = dg.Where(x => x.DateId > dateId4ago)
                               let ic = (dgCount + iterationDays - 1) / iterationDays
                               let icMax4 = Math.Min(4, ic)
                               select new ProfileReportWeekly
                               {
                                   ProfileId = dg.Key,
                                   ProfileRole = dg.First().ProfileRole,
                                   DateId = dateId,
                                   IterationCount = ic,
                                   TaskCategoryId = 1,

                                   ComplexityChange = last1.Sum(x => x.ComplexityChange),
                                   ComplexityTotalAverage = (float)dg.Sum(c => c.ComplexityChange) / ic,

                                   CompanyTokensChange = last1.Sum(x => x.CompanyTokensChange),
                                   CompanyTokensTotalAverage = dg.Sum(c => c.CompanyTokensChange) / ic,

                                   EffortScoreChange = last1.Sum(x => x.EffortScoreChange),
                                   EffortScoreTotalAverage = dg.Sum(c => c.EffortScoreChange) / ic,

                                   OneUpsGivenChange = last1.Sum(x => x.OneUpsGivenChange),
                                   OneUpsGivenTotalAverage = (float)dg.Sum(c => c.OneUpsGivenChange) / ic,

                                   OneUpsReceivedChange = last1.Sum(x => x.OneUpsReceivedChange),
                                   OneUpsReceivedTotalAverage = (float)dg.Sum(c => c.OneUpsReceivedChange) / ic,

                                   AssistsChange = last1.Sum(x => x.AssistsChange),
                                   AssistsTotalAverage = (float)dg.Sum(c => c.AssistsChange) / ic,

                                   TasksCompletedChange = last1.Sum(x => x.TasksCompletedChange),
                                   TasksCompletedTotalAverage = (float)dg.Sum(c => c.TasksCompletedChange) / ic,

                                   TurnoverChange = last1.Sum(x => x.TurnoverChange),
                                   TurnoverTotalAverage = (float)dg.Sum(c => c.TurnoverChange) / ic,

                                   ErrorChange = last1.Sum(x => x.ErrorChange),
                                   ErrorTotalAverage = dg.Sum(c => c.ErrorChange) / ic,

                                   ContributionChange = last1.Sum(x => x.ContributionChange),
                                   ContributionTotalAverage = dg.Sum(c => c.ContributionChange) / ic,

                                   SavesChange = last1.Sum(x => x.SavesChange),
                                   SavesTotalAverage = (float)dg.Sum(c => c.SavesChange) / ic,

                                   TacklesChange = last1.Sum(x => x.TacklesChange),
                                   TacklesTotalAverage = (float)dg.Sum(c => c.TacklesChange) / ic,

                                   TasksCompletionTimeChange = last1.Sum(x => x.TasksCompletionTimeChange),
                                   TasksCompletionTimeAverage = dg.Sum(c => c.TasksCompletionTimeChange) / ic,

                                   OImpactAverage = (float)last4.Sum(x => x.ComplexityChange + x.TasksCompletedChange + x.AssistsChange) / icMax4,
                                   OImpactTotalAverage = (float)dg.Sum(x => x.ComplexityChange + x.TasksCompletedChange + x.AssistsChange) / ic,

                                   DImpactAverage = (float)last4.Sum(x => x.SavesChange + x.TacklesChange) / icMax4,
                                   DImpactTotalAverage = (float)dg.Sum(x => x.SavesChange + x.TacklesChange) / ic,

                                   PowerAverage = last4.Sum(x => x.ComplexityChange / (float)x.TasksCompletedChange) / icMax4,
                                   PowerTotalAverage = dg.Sum(x => x.ComplexityChange / (float)x.TasksCompletedChange) / ic,

                                   SpeedAverage = last4.Sum(x => x.ComplexityChange) / icMax4,
                                   SpeedTotalAverage = (float)dg.Sum(x => x.ComplexityChange) / ic,

                                   HeatIndex = last1.Sum(x => x.ComplexityChange) / (float)dg.Where(x => x.DateId > dateId2ago && x.DateId <= dateId1ago).Sum(x => x.ComplexityChange)
                               }).ToList();

            var lastReportsHeat = (from r in organizationDb.ProfileReportsWeekly
                                   where r.DateId == dateId1ago
                                   select new
                                   {
                                       r.ProfileId,
                                       r.Heat
                                   })
                                    .DistinctBy(x => x.ProfileId).ToList(); //SegmentArea is ignored cuz we are only taking 'Totals'


            foreach (var r in reportsToInsert)
            {
                r.PowerAverage = double.IsNaN(r.PowerAverage) || double.IsInfinity(r.PowerAverage) ? 0 : r.PowerAverage;
                r.PowerTotalAverage = double.IsNaN(r.PowerTotalAverage) || double.IsInfinity(r.PowerTotalAverage) ? 0 : r.PowerTotalAverage;
                r.HeatIndex = double.IsNaN(r.HeatIndex) || double.IsInfinity(r.HeatIndex) ? 0 : r.HeatIndex;

                var lastHeat = lastReportsHeat.FirstOrDefault(x => x.ProfileId == r.ProfileId)?.Heat;

                if (!lastHeat.HasValue)
                {
                    r.Heat = 22f;
                }
                else
                {
                    r.Heat = lastHeat.Value * r.HeatIndex;
                }
            }

            var existing = organizationDb.ProfileReportsWeekly.Count(x => x.DateId == dateId);
            if (existing > 0)
            {
                logService.Log<ProfileReportWeekly>($"deleting {existing} records from database");
                organizationDb.Database.ExecuteSqlCommand($"delete from ProfileReportsWeekly where {nameof(ProfileReportWeekly.DateId)} = {dateId}", dateId); //this extra parameter is a workaround in ef 2.2
                organizationDb.SaveChanges();
            }

            organizationDb.ProfileReportsWeekly.AddRange(reportsToInsert);
            organizationDb.SaveChanges();

            logService.Log<GenerateProfileReportsLoader>($"{reportsToInsert.Count} new profile reports saved to database.");
            return reportsToInsert;
        }

        #endregion
    }
}
