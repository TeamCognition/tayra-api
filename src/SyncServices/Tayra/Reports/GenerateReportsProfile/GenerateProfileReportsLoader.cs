using System;
using System.Collections.Generic;
using System.Linq;
using Cog.Core;
using Microsoft.EntityFrameworkCore;
using MoreLinq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Tayra.Common;
using Tayra.Models.Catalog;
using Tayra.Models.Organizations;
using Tayra.Services;
using Tayra.SyncServices.Common;
using Tayra.SyncServices.Metrics;

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

        public override void Execute(DateTime date, JObject requestBody, params Tenant[] tenants)
        {
            foreach (var tenant in tenants)
            {
                LogService.SetOrganizationId(tenant.Key);
                using (var organizationDb = new OrganizationDbContext(null, new ShardTenantProvider(tenant.Key), _shardMapProvider))
                {
                    GenerateProfileReportsDaily(organizationDb, date, LogService);
                }
            }
        }


        #endregion

        #region Private Methods

        public static List<ProfileReportDaily> GenerateProfileReportsDaily(OrganizationDbContext organizationDb, DateTime fromDay, LogService logService, params int[] segmentIds)
        {
            var reportsToInsert = new List<ProfileReportDaily>();

            if(segmentIds.Length > 0)
            {
                if(!organizationDb.Segments.Any(x => segmentIds.Contains(x.Id)))
                {
                    throw new ApplicationException("there is an ID in segmentIds that doesn't exists");
                }
            }
            else
            {
                segmentIds = organizationDb.Segments.Where(x => x.IsReportingUnlocked).Select(x => x.Id).ToArray();
            }

            var companyTokenId = organizationDb.Tokens.Where(x => x.Type == TokenType.CompanyToken).Select(x => x.Id).FirstOrDefault();
            if (companyTokenId == 0)
                throw new ApplicationException("COMPANY TOKEN NOT FOUND");

            var dateId = DateHelper2.ToDateId(fromDay);
            foreach (var segmentId in segmentIds)
            {
                var profiles = organizationDb.ProfileAssignments.Where(x => x.SegmentId == segmentId).Select(x => new { Id = x.ProfileId, x.Profile.Role }).DistinctBy(x => x.Id).ToArray();
                var profileIds = profiles.Select(x => x.Id);

                var taskStats = (from t in organizationDb.Tasks
                                 where t.SegmentId == segmentId
                                 where t.Status == TaskStatuses.Done
                                 group t by t.AssigneeProfileId into total
                                 let change = total.Where(x => x.LastModifiedDateId == dateId)
                                 select new
                                 {
                                     ProfileId = total.Key,

                                     EffortScore = new EffortMetric(change.Sum(x => x.EffortScore) ?? 0f),
                                     EffortScoreTotal = total.Sum(x => x.EffortScore),

                                     Complexity = change.Sum(x => x.Complexity),
                                     ComplexityTotal = total.Sum(x => x.Complexity),

                                     Completed = change.Count(),
                                     CompletedNames = change.Select(x => x.Summary).ToArray(),
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

                                     MinutesSpent = change.Sum(x => x.TimeSpentInMinutes ?? x.AutoTimeSpentInMinutes), //TODO: those should could not completed tasks as well
                                     MinutesSpentTotal = total.Sum(x => x.TimeSpentInMinutes ?? x.AutoTimeSpentInMinutes) //TODO: those should could not completed tasks as well
                                 }).ToList();

                var tokens = (from tt in organizationDb.TokenTransactions
                              where profileIds.Contains(tt.ProfileId)
                              where tt.TokenId == companyTokenId
                              group tt by tt.ProfileId into total
                              let change = total.Where(x => x.DateId == dateId)
                              select new
                              {
                                  ProfileId = total.Key,
                                  CompanyTokensEarned = change.Where(x => x.Value > 0).Sum(x => x.Value),
                                  CompanyTokensEarnedTotal = total.Where(x => x.Value > 0).Sum(x => x.Value),
                                  CompanyTokensSpent = Math.Abs(change.Where(x => x.Value < 0).Sum(x => x.Value)),
                                  CompanyTokensSpentTotal = Math.Abs(total.Where(x => x.Value < 0).Sum(x => x.Value))
                              }).ToList();

                var praisesGiven = (from u in organizationDb.ProfilePraises
                                   where profileIds.Contains(u.PraiserProfileId)
                                   group u by u.PraiserProfileId into total
                                   let change = total.Where(x => x.DateId == dateId)
                                   select new
                                   {
                                       ProfileId = total.Key,

                                       Usernames = change.Select(x => x.Profile.Username).ToArray(),
                                       Count = change.Count(),
                                       CountTotal = total.Count()
                                   }).ToList();

                var praisesReceived = (from u in organizationDb.ProfilePraises
                                      where profileIds.Contains(u.ProfileId)
                                      group u by u.ProfileId into total
                                      let change = total.Where(x => x.DateId == dateId)
                                      select new
                                      {
                                          ProfileId = total.Key,

                                          Usernames = organizationDb.Profiles.Where(x => change.Select(c => c.CreatedBy).Contains(x.Id)).Select(x => x.Username).ToArray(),
                                          Count = change.Count(),
                                          CountTotal = total.Count()
                                      }).ToList();

                var itemsCreated = (from i in organizationDb.Items
                                    where profileIds.Contains(i.CreatedBy)
                                    group i by i.CreatedBy into total
                                    let change = total.Where(x => x.CreatedDateId == dateId)
                                    select new
                                    {
                                        ProfileId = total.Key,
                                        Count = change.Count(),
                                        CountTotal = total.Count()
                                    }).ToList();


                var itemsDissed = (from i in organizationDb.ItemDisenchants
                                   where profileIds.Contains(i.ProfileId)
                                   group i by i.ProfileId into total
                                   let change = total.Where(x => x.DateId == dateId)
                                   select new
                                   {
                                       ProfileId = total.Key,

                                       Names = change.Select(x => x.Item.Name).ToArray(),
                                       Count = change.Count(),
                                       CountTotal = total.Count()
                                   }).ToList();

                var giftsSent = (from u in organizationDb.ItemGifts
                                 where profileIds.Contains(u.SenderId)
                                 group u by u.SenderId into total
                                 let change = total.Where(x => x.DateId == dateId)
                                 select new
                                 {
                                     ProfileId = total.Key,
                                     Gifts = change.Select(x => x.Receiver.Username + " - " + x.Item.Name).ToArray(),
                                     Count = change.Count(),
                                     CountTotal = total.Count()
                                 }).ToList();

                var giftsReceived = (from u in organizationDb.ItemGifts
                                     where profileIds.Contains(u.ReceiverId)
                                     group u by u.ReceiverId into total
                                     let change = total.Where(x => x.DateId == dateId)
                                     select new
                                     {
                                         ProfileId = total.Key,
                                         Gifts = change.Select(x => x.Sender.Username + " - " + x.Item.Name).ToArray()
                                     }).ToList();

                var shopPurchases = (from sp in organizationDb.ShopPurchases
                                     where profileIds.Contains(sp.ProfileId)
                                     where sp.Status == ShopPurchaseStatuses.Fulfilled
                                     group sp by sp.ProfileId into total
                                     let change = total.Where(x => x.LastModifiedDateId == dateId)
                                     select new
                                     {
                                         ProfileId = total.Key,
                                         Something = total.Select(x => x.Created).ToArray(),
                                         Names = change.Select(x => x.Item.Name).ToArray(),
                                         Count = change.Count(),
                                         CountTotal = total.Count()
                                     }).ToList();

                var inventory = (from pinv in organizationDb.ProfileInventoryItems
                                 where profileIds.Contains(pinv.ProfileId)
                                 group pinv by pinv.ProfileId into total
                                 select new
                                 {
                                     ProfileId = total.Key,

                                     TotalCount = total.Count(),
                                     TotalValue = total.Sum(x => x.Item.Price)
                                 }).ToList();

                var quests = (from qc in organizationDb.QuestCompletions
                    where profileIds.Contains(qc.ProfileId)
                    group qc by qc.ProfileId
                    into total
                    let change = total.Where(x => x.Created.Date == fromDay.Date)
                    select new
                    {
                        ProfileId = total.Key,

                        Count = change.Count(),
                        CountTotal = total.Count()
                    }).ToList();
                
                var gitCommitsToday = (from gc in organizationDb.GitCommits
                    where profileIds.Contains(gc.AuthorProfileId.Value)
                    where gc.Created.Date == fromDay.Date
                    select new
                    {
                        ProfileId = gc.AuthorProfileId,
                        Message = gc.Message,
                        ExternalUrl = gc.ExternalUrl
                    }).ToList();

                foreach (var p in profiles)
                {
                    var ts = taskStats.FirstOrDefault(x => x.ProfileId == p.Id);
                    var t = tokens.FirstOrDefault(x => x.ProfileId == p.Id);
                    var praisesG = praisesGiven.FirstOrDefault(x => x.ProfileId == p.Id);
                    var praisesR = praisesReceived.FirstOrDefault(x => x.ProfileId == p.Id);
                    var sp = shopPurchases.FirstOrDefault(x => x.ProfileId == p.Id);
                    var iCreated = itemsCreated.FirstOrDefault(x => x.ProfileId == p.Id);
                    var iDissed = itemsDissed.FirstOrDefault(x => x.ProfileId == p.Id);
                    var iGiftS = giftsSent.FirstOrDefault(x => x.ProfileId == p.Id);
                    var iGiftR = giftsReceived.FirstOrDefault(x => x.ProfileId == p.Id);
                    var inv = inventory.FirstOrDefault(x => x.ProfileId == p.Id);
                    var q = quests.FirstOrDefault(x => x.ProfileId == p.Id);
                    var gcommits = gitCommitsToday.Where(x => x.ProfileId == p.Id).ToArray();
                    var iterationCount = 1;

                    var activityChart = new ProfileActivityChartDTO
                    {
                        DateId = dateId,
                        AssistsData = new ProfileActivityChartDTO.AssistsDTO
                        {
                            Endorsed = praisesG?.Usernames ?? new string[0],
                            EndorsedBy = praisesR?.Usernames ?? new string[0]
                        },
                        DeliveryData = new ProfileActivityChartDTO.DeliveryDTO
                        {
                            TaskName = ts?.CompletedNames ?? new string[0],
                            TokensGained = ts?.EffortScore ?? 0d
                        },
                        ItemActivityData = new ProfileActivityChartDTO.ItemActivityDTO
                        {
                            GiftsSent = iGiftS?.Gifts ?? new string[0],
                            GiftsReceived = iGiftR?.Gifts ?? new string[0],
                            Bought = sp?.Names ?? new string[0],
                            Disenchanted = iDissed?.Names ?? new string[0]
                        },
                        GitCommitData = gcommits.Select(x => 
                            new ProfileActivityChartDTO.GitCommitDTO
                            {
                                Message = x.Message,
                                ExternalUrl = x.ExternalUrl
                            }).ToArray()
                    };

                    reportsToInsert.Add(new ProfileReportDaily
                    {
                        ProfileId = p.Id,
                        ProfileRole = p.Role,
                        DateId = dateId,
                        IterationCount = iterationCount,
                        SegmentId = segmentId,
                        TaskCategoryId = 1,

                        ComplexityChange = ts?.Complexity ?? 0,
                        ComplexityTotal = ts?.ComplexityTotal ?? 0,

                        CompanyTokensEarnedChange = (float)(t?.CompanyTokensEarned ?? 0),
                        CompanyTokensEarnedTotal = (float)(t?.CompanyTokensEarnedTotal ?? 0),

                        CompanyTokensSpentChange = (float)(t?.CompanyTokensSpent ?? 0),
                        CompanyTokensSpentTotal = (float)(t?.CompanyTokensSpentTotal ?? 0),

                        EffortScoreChange = ts?.EffortScore ?? 0f,
                        EffortScoreTotal = ts?.EffortScoreTotal ?? 0f,

                        PraisesGivenChange = praisesG?.Count ?? 0,
                        PraisesGivenTotal = praisesG?.CountTotal ?? 0,

                        PraisesReceivedChange = praisesR?.Count ?? 0,
                        PraisesReceivedTotal = praisesR?.CountTotal ?? 0,

                        AssistsChange = (praisesR?.Count) ?? 0,
                        AssistsTotal = (praisesR?.CountTotal) ?? 0,

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

                        InventoryCountTotal = inv?.TotalCount ?? 0,
                        InventoryValueTotal = inv?.TotalValue ?? 0,

                        ItemsBoughtChange = (sp?.Count) ?? 0,
                        ItemsBoughtTotal = (sp?.CountTotal) ?? 0,

                        ItemsCreatedChange = (iCreated?.Count) ?? 0,
                        ItemsCreatedTotal = (iCreated?.CountTotal) ?? 0,

                        ItemsDisenchantedChange = (iDissed?.Count) ?? 0,
                        ItemsDisenchantedTotal = (iDissed?.CountTotal) ?? 0,

                        ItemsGiftedChange = (iGiftS?.Count) ?? 0,
                        ItemsGiftedTotal = (iGiftS?.CountTotal) ?? 0,

                        QuestsCompletedChange = (q?.Count) ?? 0,
                        QuestsCompletedTotal = (q?.CountTotal) ?? 0,
                        
                        ActivityChartJson = JsonConvert.SerializeObject(activityChart)
                    });
                }

                var existing = organizationDb.ProfileReportsDaily.Count(x => x.DateId == dateId && x.SegmentId == segmentId);
                if (existing > 0)
                {
                    logService.Log<ProfileReportDaily>($"deleting {existing} records from database");
                    //organizationDb.Database.ExecuteSqlInterpolated($"delete from ProfileReportsDaily where {nameof(ProfileReportDaily.DateId)} = {dateId} AND {nameof(ProfileReportDaily.SegmentId)} = {segmentId}");
                    organizationDb.Database.ExecuteSqlCommand($"delete from ProfileReportsDaily where {nameof(ProfileReportDaily.DateId)} = {dateId} AND {nameof(ProfileReportDaily.SegmentId)} = {segmentId}", dateId); //this extra parameter is a workaround in ef 2.2
                    organizationDb.SaveChanges();
                }
            }
            organizationDb.ProfileReportsDaily.AddRange(reportsToInsert);
            
            organizationDb.SaveChanges();

            logService.Log<GenerateProfileReportsLoader>($"{reportsToInsert.Count} new profile reports saved to database.");
            return reportsToInsert;
        }

        public static List<ProfileReportWeekly> GenerateProfileReportsWeekly(OrganizationDbContext organizationDb, DateTime fromDay, LogService logService, params int[] segmentIds)
        {
            if (!CommonHelper.IsMonday(fromDay))
                return null;
 
            var reportsToInsert = new List<ProfileReportWeekly>();

            var iterationDays = 7;
            var dateId = DateHelper2.ToDateId(fromDay);
            var dateId1ago = DateHelper2.ToDateId(fromDay.AddDays(-iterationDays));
            var dateId2ago = DateHelper2.ToDateId(fromDay.AddDays(-iterationDays * 2));
            //var dateId3ago = DateHelper2.ToDateId(fromDay.AddDays(-iterationDays * 3));
            var dateId4ago = DateHelper2.ToDateId(fromDay.AddDays(-iterationDays * 4));

            if (segmentIds.Length > 0)
            {
                if (!organizationDb.Segments.Any(x => segmentIds.Contains(x.Id)))
                {
                    throw new ApplicationException("there is an ID in segmentIds that doesn't exists");
                }
            }
            else
            {
                segmentIds = organizationDb.Segments.Where(x => x.IsReportingUnlocked).Select(x => x.Id).ToArray();
            }

            foreach (var segmentId in segmentIds)
            {   
                reportsToInsert = (from d in organizationDb.ProfileReportsDaily
                                   where d.DateId <= dateId
                                   where d.SegmentId == segmentId
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
                                       SegmentId = segmentId,
                                       TaskCategoryId = 1,

                                       ComplexityChange = last1.Sum(x => x.ComplexityChange),
                                       ComplexityTotalAverage = (float)dg.Sum(c => c.ComplexityChange) / ic,

                                       CompanyTokensEarnedChange = last1.Sum(x => x.CompanyTokensEarnedChange),
                                       CompanyTokensEarnedTotalAverage = dg.Sum(c => c.CompanyTokensEarnedChange) / ic,

                                       CompanyTokensSpentChange = last1.Sum(x => x.CompanyTokensSpentChange),
                                       CompanyTokensSpentTotalAverage = dg.Sum(c => c.CompanyTokensSpentChange) / ic,

                                       EffortScoreChange = last1.Sum(x => x.EffortScoreChange),
                                       EffortScoreTotalAverage = dg.Sum(c => c.EffortScoreChange) / ic,

                                       PraisesGivenChange = last1.Sum(x => x.PraisesGivenChange),
                                       PraisesGivenTotalAverage = (float)dg.Sum(c => c.PraisesGivenChange) / ic,

                                       PraisesReceivedChange = last1.Sum(x => x.PraisesReceivedChange),
                                       PraisesReceivedTotalAverage = (float)dg.Sum(c => c.PraisesReceivedChange) / ic,

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

                                       InventoryCountTotal = last1.OrderByDescending(x => x.DateId).Select(x => x.InventoryCountTotal).FirstOrDefault(),
                                       InventoryValueTotal = last1.OrderByDescending(x => x.DateId).Select(x => x.InventoryValueTotal).FirstOrDefault(),

                                       ItemsBoughtChange = last1.Sum(x => x.ItemsBoughtChange),
                                       ItemsCreatedChange = last1.Sum(x => x.ItemsCreatedChange),
                                       ItemsDisenchantedChange = last1.Sum(x => x.ItemsDisenchantedChange),
                                       ItemsGiftedChange = last1.Sum(x => x.ItemsGiftedChange),

                                       OImpactAverage = (float)last4.Sum(x => x.ComplexityChange + x.TasksCompletedChange + x.AssistsChange) / icMax4,
                                       OImpactTotalAverage = (float)dg.Sum(x => x.ComplexityChange + x.TasksCompletedChange + x.AssistsChange) / ic,

                                       DImpactAverage = (float)last4.Sum(x => x.SavesChange + x.TacklesChange) / icMax4,
                                       DImpactTotalAverage = (float)dg.Sum(x => x.SavesChange + x.TacklesChange) / ic,

                                       PowerAverage = last4.Sum(x => x.ComplexityChange) / (float)last4.Sum(x => x.TasksCompletedChange),
                                       PowerTotalAverage = dg.Sum(x => x.ComplexityChange) / (float)dg.Sum(x => x.TasksCompletedChange),

                                       SpeedAverage = (float)last4.Sum(x => x.TasksCompletedChange) / icMax4,
                                       SpeedTotalAverage = (float)dg.Sum(x => x.TasksCompletedChange) / ic,

                                       HeatIndex = icMax4
                                   }).ToList();

                var lastReport = (from r in organizationDb.ProfileReportsWeekly
                                  where r.SegmentId == segmentId
                                  where r.DateId == dateId1ago
                                  select new
                                  {
                                      r.ProfileId,
                                      r.Heat,
                                      r.ComplexityChange
                                  })
                                  .DistinctBy(x => x.ProfileId).ToList(); //SegmentArea is ignored cuz we are only taking 'Totals'


                foreach (var r in reportsToInsert)
                {
                    r.PowerAverage = double.IsNaN(r.PowerAverage) || double.IsInfinity(r.PowerAverage) ? 0 : r.PowerAverage;
                    r.PowerTotalAverage = double.IsNaN(r.PowerTotalAverage) || double.IsInfinity(r.PowerTotalAverage) ? 0 : r.PowerTotalAverage;
                    //r.HeatIndex = double.IsNaN(r.HeatIndex) || double.IsInfinity(r.HeatIndex) ? 0 : r.HeatIndex;

                    var lastWeekC = lastReport.FirstOrDefault(x => x.ProfileId == r.ProfileId)?.ComplexityChange;
                    var lastWeekHeat = lastReport.FirstOrDefault(x => x.ProfileId == r.ProfileId)?.Heat;

                    //you have done some work AND (no previous reports exist OR a week before no work has been done)
                    if (r.ComplexityChange != 0 && (!lastWeekC.HasValue || (lastWeekC.HasValue && lastWeekC.Value == 0)))
                    {
                        r.HeatIndex = -1;
                        r.Heat = 10f;
                    }
                    //no work been done in a week
                    else if(r.ComplexityChange == 0)
                    {
                        r.HeatIndex = 0;
                        r.Heat = 0;
                    }
                    //heat formula
                    else
                    {
                        r.HeatIndex = (float)Math.Sqrt(r.ComplexityChange / (float)lastWeekC);
                        r.Heat = lastWeekHeat.Value * r.HeatIndex;
                    }
                }

                var existing = organizationDb.ProfileReportsWeekly.Where(x => x.SegmentId == segmentId).Count(x => x.DateId == dateId);
                if (existing > 0)
                {
                    logService.Log<ProfileReportWeekly>($"deleting {existing} records from database");
                    //organizationDb.Database.ExecuteSqlInterpolated($"delete from ProfileReportsWeekly where {nameof(ProfileReportWeekly.DateId)} = {dateId} AND {nameof(ProfileReportWeekly.SegmentId)} = {segmentId}");
                    organizationDb.Database.ExecuteSqlCommand($"delete from ProfileReportsWeekly where {nameof(ProfileReportWeekly.DateId)} = {dateId} AND {nameof(ProfileReportWeekly.SegmentId)} = {segmentId}", dateId); //this extra parameter is a workaround in ef 2.2
                    organizationDb.SaveChanges();
                }

            }
            organizationDb.ProfileReportsWeekly.AddRange(reportsToInsert);
            organizationDb.SaveChanges();

            logService.Log<GenerateProfileReportsLoader>($"{reportsToInsert.Count} new profile reports saved to database.");
            return reportsToInsert;
        }

        #endregion
    }
}
