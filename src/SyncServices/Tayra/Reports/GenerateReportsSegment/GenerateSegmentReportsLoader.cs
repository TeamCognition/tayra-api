using System;
using System.Collections.Generic;
using System.Linq;
using Cog.Core;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using Tayra.Common;
using Tayra.Models.Catalog;
using Tayra.Models.Organizations;
using Tayra.SyncServices.Common;

namespace Tayra.SyncServices.Tayra
{
    public class GenerateSegmentReportsLoader : BaseLoader
    {
        private readonly IShardMapProvider _shardMapProvider;

        #region Constructor

        public GenerateSegmentReportsLoader(IShardMapProvider shardMapProvider, LogService logService, CatalogDbContext catalogDb) : base(logService, catalogDb)
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
                    GenerateSegmentReportsDaily(organizationDb, date, LogService);
                }
            }
        }

        public static void GenerateSegmentReportsDaily(OrganizationDbContext organizationDb, DateTime fromDay, LogService logService, List<ProfileReportDaily> profileReportsDaily = null, params int[] segmentIds)
        {
            var dateId = DateHelper2.ToDateId(fromDay);

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

            if (profileReportsDaily == null)
            {
                profileReportsDaily = organizationDb.ProfileReportsDaily.Where(x => x.DateId == dateId && segmentIds.Contains(x.SegmentId)).ToList();
            }


            var reportsToInsert = new List<SegmentReportDaily>();

            var segments = (from s in organizationDb.Segments
                            where segmentIds.Contains(s.Id)
                            select new
                            {
                                SegmentId = s.Id,
                                MemberIds = s.Members.Where(x => x.Profile.Role == ProfileRoles.Member).Select(x => x.ProfileId).ToList(),
                                NonMemberIds = s.Members.Where(x => x.Profile.Role != ProfileRoles.Member).Select(x => x.ProfileId).ToList()
                            }).ToList();

            foreach (var s in segments)
            {
                var mr = profileReportsDaily.Where(x => s.MemberIds.Contains(x.ProfileId) && x.SegmentId == s.SegmentId).ToList();
                var nmr = profileReportsDaily.Where(x => s.NonMemberIds.Contains(x.ProfileId) && x.SegmentId == s.SegmentId).ToList();

                reportsToInsert.Add(new SegmentReportDaily
                {
                    SegmentId = s.SegmentId,
                    DateId = dateId,
                    IterationCount = 1,
                    TaskCategoryId = 1,
                    MembersCountTotal = mr.Count(),
                    NonMembersCountTotal = nmr.Count(),

                    ComplexityChange = mr.Sum(x => x.ComplexityChange),
                    ComplexityTotal = mr.Sum(x => x.ComplexityTotal),

                    CompanyTokensEarnedChange = mr.Sum(x => x.CompanyTokensEarnedChange),
                    CompanyTokensEarnedTotal = mr.Sum(x => x.CompanyTokensEarnedTotal),

                    CompanyTokensSpentChange = mr.Sum(x => x.CompanyTokensSpentChange),
                    CompanyTokensSpentTotal = mr.Sum(x => x.CompanyTokensSpentTotal),

                    EffortScoreChange = mr.Sum(x => x.EffortScoreChange),
                    EffortScoreTotal = mr.Sum(x => x.EffortScoreTotal),

                    PraisesGivenChange = mr.Sum(x => x.PraisesGivenChange),
                    PraisesGivenTotal = mr.Sum(x => x.PraisesGivenTotal),

                    PraisesReceivedChange = mr.Sum(x => x.PraisesReceivedChange),
                    PraisesReceivedTotal = mr.Sum(x => x.PraisesReceivedTotal),

                    AssistsChange = mr.Sum(x => x.AssistsChange),
                    AssistsTotal = mr.Sum(x => x.AssistsTotal),

                    TasksCompletedChange = mr.Sum(x => x.TasksCompletedChange),
                    TasksCompletedTotal = mr.Sum(x => x.TasksCompletedTotal),

                    TurnoverChange = mr.Sum(x => x.TurnoverChange),
                    TurnoverTotal = mr.Sum(x => x.TurnoverTotal),

                    ErrorChange = mr.Sum(x => x.ErrorChange),
                    ErrorTotal = mr.Sum(x => x.ErrorTotal),

                    ContributionChange = mr.Sum(x => x.ContributionChange),
                    ContributionTotal = mr.Sum(x => x.ContributionTotal),

                    SavesChange = mr.Sum(x => x.SavesChange),
                    SavesTotal = mr.Sum(x => x.SavesTotal),

                    TacklesChange = mr.Sum(x => x.TacklesChange),
                    TacklesTotal = mr.Sum(x => x.TacklesTotal),

                    TasksCompletionTimeChange = mr.Sum(x => x.TasksCompletionTimeChange),
                    TasksCompletionTimeTotal = mr.Sum(x => x.TasksCompletionTimeTotal),

                    ItemsBoughtChange = mr.Sum(x => x.ItemsBoughtChange),
                    ItemsBoughtTotal = mr.Sum(x => x.ItemsBoughtTotal),

                    ItemsCreatedChange = nmr.Sum(x => x.ItemsCreatedChange),
                    ItemsDisenchantedChange = mr.Sum(x => x.ItemsDisenchantedChange),
                    ItemsGiftedChange = mr.Sum(x => x.ItemsGiftedChange)
                });
            
                var existing = organizationDb.SegmentReportsDaily.Count(x => x.DateId == dateId && x.SegmentId == s.SegmentId);
                if (existing > 0)
                {
                    logService.Log<SegmentReportDaily>($"deleting {existing} records from database");
                    organizationDb.Database.ExecuteSqlCommand($"delete from SegmentReportsDaily where {nameof(SegmentReportDaily.DateId)} = {dateId} AND  {nameof(SegmentReportDaily.SegmentId)} = {s.SegmentId}", dateId); //this extra parameter is a workaround in ef 2.2
                    //organizationDb.Database.ExecuteSqlInterpolated($"delete from SegmentReportsDaily where {nameof(SegmentReportDaily.DateId)} = {dateId} AND  {nameof(SegmentReportDaily.SegmentId)} = {s.SegmentId}");
                    organizationDb.SaveChanges();
                }
            }
            organizationDb.SegmentReportsDaily.AddRange(reportsToInsert);
            organizationDb.SaveChanges();

            logService.Log<GenerateSegmentReportsLoader>($"{reportsToInsert.Count} new reports saved to database.");
        }

        public static void GenerateSegmentReportsWeekly(OrganizationDbContext organizationDb, DateTime fromDay, LogService logService, List<ProfileReportDaily> profileReportsDaily = null, List<ProfileReportWeekly> profileReportsWeekly = null, params int[] segmentIds)
        {
            if (!CommonHelper.IsMonday(fromDay) || profileReportsWeekly == null)
                return;

            var dateId = DateHelper2.ToDateId(fromDay);

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

            if (profileReportsDaily == null)
            {
                profileReportsDaily = organizationDb.ProfileReportsDaily.Where(x => x.DateId == dateId && segmentIds.Contains(x.SegmentId)).ToList();
            }

            if (profileReportsWeekly == null)
            {
                profileReportsWeekly = organizationDb.ProfileReportsWeekly.Where(x => x.DateId == dateId && segmentIds.Contains(x.SegmentId)).ToList();
            }


            var reportsToInsert = new List<SegmentReportWeekly>();

            var segments = (from s in organizationDb.Segments
                            where segmentIds.Contains(s.Id)
                            let teamIds = s.Teams.Select(x => x.Id).ToList()
                            select new
                            {
                                SegmentId = s.Id,
                                MemberIds = s.Members.Where(x => x.Profile.Role == ProfileRoles.Member).Select(x => x.ProfileId).ToList(),
                                NonMemberIds = s.Members.Where(x => x.Profile.Role != ProfileRoles.Member).Select(x => x.ProfileId).ToList()
                            }).ToList();

            foreach (var s in segments)
            {
                var dmr = profileReportsDaily.Where(x => s.MemberIds.Contains(x.ProfileId) && x.SegmentId == s.SegmentId).ToList();
                var dnmr = profileReportsDaily.Where(x => s.NonMemberIds.Contains(x.ProfileId) && x.SegmentId == s.SegmentId).ToList();

                var wmr = profileReportsWeekly.Where(x => s.MemberIds.Contains(x.ProfileId) && x.SegmentId == s.SegmentId).ToList();
                var wnmr = profileReportsWeekly.Where(x => s.NonMemberIds.Contains(x.ProfileId) && x.SegmentId == s.SegmentId).ToList();

                if (dmr.Count() == 0)
                    continue;

                //active teams, active integrations, active quests are missing. reports/overview
                reportsToInsert.Add(new SegmentReportWeekly
                {
                    SegmentId = s.SegmentId,
                    DateId = dateId,
                    IterationCount = 1,
                    TaskCategoryId = 1,
                    MembersCountTotal = dmr.Count(),
                    NonMembersCountTotal = dnmr.Count(),

                    ComplexityChange = dmr.Sum(x => x.ComplexityChange),
                    ComplexityAverage = (float)dmr.Average(x => x.ComplexityChange),

                    CompanyTokensEarnedChange = dmr.Sum(x => x.CompanyTokensEarnedChange),
                    CompanyTokensEarnedAverage = dmr.Average(x => x.CompanyTokensEarnedChange),

                    CompanyTokensSpentChange = dmr.Sum(x => x.CompanyTokensSpentChange),
                    CompanyTokensSpentAverage = dmr.Average(x => x.CompanyTokensSpentChange),

                    EffortScoreChange = dmr.Sum(x => x.EffortScoreChange),
                    EffortScoreAverage = dmr.Average(x => x.EffortScoreChange),

                    PraisesGivenChange = dmr.Sum(x => x.PraisesGivenChange),
                    PraisesGivenAverage = (float)dmr.Average(x => x.PraisesGivenChange),

                    PraisesReceivedChange = dmr.Sum(x => x.PraisesReceivedChange),
                    PraisesReceivedAverage = (float)dmr.Average(x => x.PraisesReceivedChange),

                    AssistsChange = dmr.Sum(x => x.AssistsChange),
                    AssistsAverage = (float)dmr.Average(x => x.AssistsChange),

                    TasksCompletedChange = dmr.Sum(x => x.TasksCompletedChange),
                    TasksCompletedAverage = (float)dmr.Average(x => x.TasksCompletedChange),

                    TurnoverChange = dmr.Sum(x => x.TurnoverChange),
                    TurnoverAverage = (float)dmr.Average(x => x.TurnoverChange),

                    ErrorChange = dmr.Sum(x => x.ErrorChange),
                    ErrorAverage = dmr.Average(x => x.ErrorChange),

                    ContributionChange = dmr.Sum(x => x.ContributionChange),
                    ContributionAverage = dmr.Average(x => x.ContributionChange),

                    SavesChange = dmr.Sum(x => x.SavesChange),
                    SavesAverage = (float)dmr.Average(x => x.SavesChange),

                    TacklesChange = dmr.Sum(x => x.TacklesChange),
                    TacklesAverage = (float)dmr.Average(x => x.TacklesChange),

                    TasksCompletionTimeChange = dmr.Sum(x => x.TasksCompletionTimeChange),
                    TasksCompletionTimeAverage = (int)dmr.Average(x => x.TasksCompletionTimeChange),

                    ItemsBoughtChange = dmr.Sum(x => x.ItemsBoughtChange),
                    ItemsCreatedChange = dnmr.Sum(x => x.ItemsCreatedChange),
                    ItemsDisenchantedChange = dmr.Sum(x => x.ItemsDisenchantedChange),
                    ItemsGiftedChange = dmr.Sum(x => x.ItemsGiftedChange),

                    OImpactAverage = wmr.Average(x => x.OImpactAverage),
                    OImpactAverageTotal = wmr.Sum(x => x.OImpactAverage),

                    DImpactAverage = wmr.Average(x => x.DImpactAverage),
                    DImpactAverageTotal = wmr.Sum(x => x.OImpactAverage),

                    PowerAverage = wmr.Average(x => x.PowerAverage),
                    PowerAverageTotal = wmr.Sum(x => x.PowerAverage),

                    SpeedAverage = wmr.Average(x => x.SpeedAverage),
                    SpeedAverageTotal = wmr.Sum(x => x.SpeedAverage),

                    HeatAverageTotal = wmr.Average(x => x.Heat)
                });

                var existing = organizationDb.SegmentReportsWeekly.Where(x => x.SegmentId == s.SegmentId).Count(x => x.DateId == dateId);
                if (existing > 0)
                {
                    logService.Log<SegmentReportWeekly>($"deleting {existing} records from database");
                    organizationDb.Database.ExecuteSqlCommand($"delete from SegmentReportsWeekly where {nameof(SegmentReportWeekly.DateId)} = {dateId} AND {nameof(SegmentReportWeekly.SegmentId)} = {s.SegmentId}", dateId); //this extra parameter is a workaround in ef 2.2
                    //organizationDb.Database.ExecuteSqlInterpolated($"delete from SegmentReportsWeekly where {nameof(SegmentReportWeekly.DateId)} = {dateId} AND {nameof(SegmentReportWeekly.SegmentId)} = {s.SegmentId}");
                    organizationDb.SaveChanges();
                }
            }

            organizationDb.SegmentReportsWeekly.AddRange(reportsToInsert);
            organizationDb.SaveChanges();

            logService.Log<GenerateSegmentReportsLoader>($"{reportsToInsert.Count} new reports saved to database.");
        }

        #endregion
    }
}