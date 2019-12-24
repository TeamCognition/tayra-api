using System;
using System.Collections.Generic;
using System.Linq;
using Firdaws.Core;
using Microsoft.EntityFrameworkCore;
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

        public override void Execute(DateTime date, params Tenant[] tenants)
        {
            foreach (var tenant in tenants)
            {
                LogService.SetOrganizationId(tenant.Name);
                using (var organizationDb = new OrganizationDbContext(null, new ShardTenantProvider(tenant.Name), _shardMapProvider))
                {
                    GenerateSegmentReportsDaily(organizationDb, date, LogService);
                }
            }
        }

        public static void GenerateSegmentReportsDaily(OrganizationDbContext organizationDb, DateTime fromDay, LogService logService, List<ProfileReportDaily> profileReportsDaily = null)
        {
            var dateId = DateHelper2.ToDateId(fromDay);

            if (profileReportsDaily == null)
            {
                profileReportsDaily = organizationDb.ProfileReportsDaily.Where(x => x.DateId == dateId).ToList();
            }


            var reportsToInsert = new List<SegmentReportDaily>();

            var segments = (from t in organizationDb.Segments
                            where t.ArchivedAt == null
                            select new
                            {
                                TeamId = t.Id,
                                MemberIds = t.Members.Select(x => x.ProfileId).ToList()
                            }).ToList();

            foreach (var t in segments)
            {
                var mr = profileReportsDaily.Where(x => t.MemberIds.Contains(x.ProfileId)).ToList();

                reportsToInsert.Add(new SegmentReportDaily
                {
                    SegmentId = t.TeamId,
                    DateId = dateId,
                    IterationCount = 1,
                    TaskCategoryId = 1,
                    MembersCountTotal = mr.Count(),

                    ComplexityChange = mr.Sum(x => x.ComplexityChange),
                    ComplexityTotal = mr.Sum(x => x.ComplexityTotal),

                    CompanyTokensChange = mr.Sum(x => x.CompanyTokensChange),
                    CompanyTokensTotal = mr.Sum(x => x.CompanyTokensTotal),

                    EffortScoreChange = mr.Sum(x => x.EffortScoreChange),
                    EffortScoreTotal = mr.Sum(x => x.EffortScoreTotal),

                    OneUpsGivenChange = mr.Sum(x => x.OneUpsGivenChange),
                    OneUpsGivenTotal = mr.Sum(x => x.OneUpsGivenTotal),

                    OneUpsReceivedChange = mr.Sum(x => x.OneUpsReceivedChange),
                    OneUpsReceivedTotal = mr.Sum(x => x.OneUpsReceivedTotal),

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
                });
            }


            var existing = organizationDb.SegmentReportsDaily.Count(x => x.DateId == dateId);
            if (existing > 0)
            {
                logService.Log<SegmentReportDaily>($"deleting {existing} records from database");
                organizationDb.Database.ExecuteSqlCommand($"delete from SegmentReportsDaily where {nameof(SegmentReportDaily.DateId)} = {dateId}", dateId); //this extra parameter is a workaround in ef 2.2
                organizationDb.SaveChanges();
            }

            organizationDb.SegmentReportsDaily.AddRange(reportsToInsert);
            organizationDb.SaveChanges();

            logService.Log<GenerateSegmentReportsLoader>($"{reportsToInsert.Count} new reports saved to database.");
        }

        public static void GenerateSegmentReportsWeekly(OrganizationDbContext organizationDb, DateTime fromDay, LogService logService, List<ProfileReportDaily> profileReportsDaily = null, List<ProfileReportWeekly> profileReportsWeekly = null)
        {
            if (!CommonHelper.IsMonday(fromDay) || profileReportsWeekly == null)
                return;

            var dateId = DateHelper2.ToDateId(fromDay);

            if (profileReportsDaily == null)
            {
                profileReportsDaily = organizationDb.ProfileReportsDaily.Where(x => x.DateId == dateId).ToList();
            }

            if (profileReportsWeekly == null)
            {
                profileReportsWeekly = organizationDb.ProfileReportsWeekly.Where(x => x.DateId == dateId).ToList();
            }


            var reportsToInsert = new List<SegmentReportWeekly>();

            var segments = (from t in organizationDb.Segments
                            where t.ArchivedAt == null
                            select new
                            {
                                TeamId = t.Id,
                                MemberIds = t.Members.Select(x => x.ProfileId).ToList()
                            }).ToList();

            foreach (var t in segments)
            {
                var drs = profileReportsDaily.Where(x => t.MemberIds.Contains(x.ProfileId)).ToList();
                var drsCount = drs.Count();

                var wrs = profileReportsWeekly.Where(x => t.MemberIds.Contains(x.ProfileId)).ToList();
                var wrsCount = wrs.Count();

                if (drsCount == 0)
                    continue;

                reportsToInsert.Add(new SegmentReportWeekly
                {
                    SegmentId = t.TeamId,
                    DateId = dateId,
                    IterationCount = 1,
                    TaskCategoryId = 1,
                    MembersCountTotal = drsCount,

                    ComplexityChange = drs.Sum(x => x.ComplexityChange),
                    ComplexityAverage = (float)drs.Average(x => x.ComplexityChange),

                    CompanyTokensChange = drs.Sum(x => x.CompanyTokensChange),
                    CompanyTokensAverage = drs.Average(x => x.CompanyTokensChange),

                    EffortScoreChange = drs.Sum(x => x.EffortScoreChange),
                    EffortScoreAverage = drs.Average(x => x.EffortScoreChange),

                    OneUpsGivenChange = drs.Sum(x => x.OneUpsGivenChange),
                    OneUpsGivenAverage = (float)drs.Average(x => x.OneUpsGivenChange),

                    OneUpsReceivedChange = drs.Sum(x => x.OneUpsReceivedChange),
                    OneUpsReceivedAverage = (float)drs.Average(x => x.OneUpsReceivedChange),

                    AssistsChange = drs.Sum(x => x.AssistsChange),
                    AssistsAverage = (float)drs.Average(x => x.AssistsChange),

                    TasksCompletedChange = drs.Sum(x => x.TasksCompletedChange),
                    TasksCompletedAverage = (float)drs.Average(x => x.TasksCompletedChange),

                    TurnoverChange = drs.Sum(x => x.TurnoverChange),
                    TurnoverAverage = (float)drs.Average(x => x.TurnoverChange),

                    ErrorChange = drs.Sum(x => x.ErrorChange),
                    ErrorAverage = drs.Average(x => x.ErrorChange),

                    ContributionChange = drs.Sum(x => x.ContributionChange),
                    ContributionAverage = drs.Average(x => x.ContributionChange),

                    SavesChange = drs.Sum(x => x.SavesChange),
                    SavesAverage = (float)drs.Average(x => x.SavesChange),

                    TacklesChange = drs.Sum(x => x.TacklesChange),
                    TacklesAverage = (float)drs.Average(x => x.TacklesChange),

                    OImpactAverage = wrs.Average(x => x.OImpactAverage),
                    OImpactAverageTotal = wrs.Sum(x => x.OImpactAverage),

                    DImpactAverage = wrs.Average(x => x.DImpactAverage),
                    DImpactAverageTotal = wrs.Sum(x => x.OImpactAverage),

                    PowerAverage = wrs.Average(x => x.PowerAverage),
                    PowerAverageTotal = wrs.Sum(x => x.PowerAverage),

                    SpeedAverage = wrs.Average(x => x.SpeedAverage),
                    SpeedAverageTotal = wrs.Sum(x => x.SpeedAverage),

                    HeatAverageTotal = wrs.Average(x => x.Heat)
                });
            }


            var existing = organizationDb.SegmentReportsWeekly.Count(x => x.DateId == dateId);
            if (existing > 0)
            {
                logService.Log<SegmentReportWeekly>($"deleting {existing} records from database");
                organizationDb.Database.ExecuteSqlCommand($"delete from SegmentReportsWeekly where {nameof(SegmentReportWeekly.DateId)} = {dateId}", dateId); //this extra parameter is a workaround in ef 2.2
                organizationDb.SaveChanges();
            }

            organizationDb.SegmentReportsWeekly.AddRange(reportsToInsert);
            organizationDb.SaveChanges();

            logService.Log<GenerateSegmentReportsLoader>($"{reportsToInsert.Count} new reports saved to database.");
        }

        #endregion
    }
}