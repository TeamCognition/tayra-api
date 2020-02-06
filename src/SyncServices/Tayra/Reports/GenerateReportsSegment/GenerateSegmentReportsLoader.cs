using System;
using System.Collections.Generic;
using System.Linq;
using Firdaws.Core;
using Microsoft.EntityFrameworkCore;
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

            var segments = (from s in organizationDb.Segments
                            let teamIds = s.Teams.Select(x => x.Id).ToList()
                            select new
                            {
                                SegmentId = s.Id,
                                MemberIds = organizationDb.TeamMembers.Where(x => teamIds.Contains(x.TeamId) && x.Profile.Role == ProfileRoles.Member).Select(x => x.ProfileId).ToList(),
                                NonMemberIds = organizationDb.TeamMembers.Where(x => teamIds.Contains(x.TeamId) && x.Profile.Role != ProfileRoles.Member).Select(x => x.ProfileId).ToList()
                            }).ToList();

            foreach (var s in segments)
            {
                var mr = profileReportsDaily.Where(x => s.MemberIds.Contains(x.ProfileId)).ToList();
                var nmr = profileReportsDaily.Where(x => s.NonMemberIds.Contains(x.ProfileId)).ToList();

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

            var segments = (from s in organizationDb.Segments
                            let teamIds = s.Teams.Select(x => x.Id).ToList()
                            select new
                            {
                                SegmentId = s.Id,
                                MemberIds = organizationDb.TeamMembers.Where(x => teamIds.Contains(x.TeamId) && x.Profile.Role == ProfileRoles.Member).Select(x => x.ProfileId).ToList(),
                                NonMemberIds = organizationDb.TeamMembers.Where(x => teamIds.Contains(x.TeamId) && x.Profile.Role != ProfileRoles.Member).Select(x => x.ProfileId).ToList()
                            }).ToList();

            foreach (var s in segments)
            {
                var dmr = profileReportsDaily.Where(x => s.MemberIds.Contains(x.ProfileId)).ToList();
                var dnmr = profileReportsDaily.Where(x => s.NonMemberIds.Contains(x.ProfileId)).ToList();

                var wmr = profileReportsWeekly.Where(x => s.MemberIds.Contains(x.ProfileId)).ToList();
                var wnmr = profileReportsWeekly.Where(x => s.NonMemberIds.Contains(x.ProfileId)).ToList();

                if (dmr.Count() == 0)
                    continue;

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

                    CompanyTokensChange = dmr.Sum(x => x.CompanyTokensChange),
                    CompanyTokensAverage = dmr.Average(x => x.CompanyTokensChange),

                    EffortScoreChange = dmr.Sum(x => x.EffortScoreChange),
                    EffortScoreAverage = dmr.Average(x => x.EffortScoreChange),

                    OneUpsGivenChange = dmr.Sum(x => x.OneUpsGivenChange),
                    OneUpsGivenAverage = (float)dmr.Average(x => x.OneUpsGivenChange),

                    OneUpsReceivedChange = dmr.Sum(x => x.OneUpsReceivedChange),
                    OneUpsReceivedAverage = (float)dmr.Average(x => x.OneUpsReceivedChange),

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