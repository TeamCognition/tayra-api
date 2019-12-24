using System;
using System.Collections.Generic;
using System.Linq;
using Firdaws.Core;
using Tayra.Common;
using Tayra.Models.Catalog;
using Tayra.Models.Organizations;
using Tayra.SyncServices.Common;

namespace Tayra.SyncServices.Tayra
{
    public class MakeActionPointsLoader : BaseLoader
    {
        #region Private Variables

        private readonly IShardMapProvider _shardMapProvider;

        #endregion

        #region Constructor

        public MakeActionPointsLoader(IShardMapProvider shardMapProvider, LogService logService, CatalogDbContext catalogDb) : base(logService, catalogDb)
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
                    MakeActionPoints(organizationDb, date, LogService);
                }
            }
        }

        #endregion

        #region Private Methods

        public static void MakeActionPoints(OrganizationDbContext organizationDb, DateTime fromDay, LogService logService)
        {
            var reportsToInsert = new List<ActionPoint>();

            var iterationDays = 7;
            var dateId = DateHelper2.ToDateId(fromDay);
            var dateId1ago = DateHelper2.ToDateId(fromDay.AddDays(-iterationDays));
            var dateId2ago = DateHelper2.ToDateId(fromDay.AddDays(-iterationDays * 2));
            var dateId4ago = DateHelper2.ToDateId(fromDay.AddDays(-iterationDays * 4));

            var taskStats = (from t in organizationDb.Tasks
                             where t.Status == TaskStatuses.Done
                             group t by t.AssigneeProfileId into total
                             let last1 = total.Where(x => x.LastModifiedDateId > dateId1ago) //last 1 iteration
                             let last2 = total.Where(x => x.LastModifiedDateId > dateId2ago) //last 2 iteration
                             select new
                             {
                                 ProfileId = total.Key,
                                 CompletedInLast1 = last1.Count(),
                                 CompletedInLast2 = last2.Count(),
                             }).ToList();

            var reportsStats = (from prw in organizationDb.ProfileReportsWeekly
                                group prw by prw.ProfileId into total
                                let last1 = total.Where(x => x.DateId > dateId1ago) //last 1 iteration
                                let last2 = total.Where(x => x.DateId > dateId2ago && x.DateId <= dateId1ago) //last 2 iteration
                                select new
                                {
                                    ProfileId = total.Key,
                                    ImpactFor1Weeks = last1.Select(x => x.OImpactAverage).FirstOrDefault(),
                                    SpeedFor1Weeks = last1.Select(x => x.SpeedAverage).FirstOrDefault(),
                                    HeatFor1Weeks = last1.Select(x => x.Heat).FirstOrDefault(),

                                    ImpactFor2Weeks = last2.Select(x => x.OImpactAverage).FirstOrDefault(),
                                    SpeedFor2Weeks = last2.Select(x => x.SpeedAverage).FirstOrDefault(),
                                    HeatFor2Weeks = last2.Select(x => x.Heat).FirstOrDefault(),
                                }).ToList();


            var shopItemsAddedCount = (from si in organizationDb.ShopItems
                                       where si.CreatedBy > 0
                                       where si.Created > DateHelper2.ParseDate(dateId4ago)
                                       select si).Count();


            var challengesCreatedCount = (from c in organizationDb.Challenges
                                          where c.CreatedBy > 0
                                          where c.Created > DateHelper2.ParseDate(dateId4ago)
                                          select c).Count();


            var profiles = organizationDb.Profiles.Select(x => new { x.Id, x.Created }).ToList();
            foreach (var p in profiles)
            {
                if (p.Created <= DateHelper2.ParseDate(dateId1ago))
                {
                    MakeProfileAP(organizationDb,
                        ActionPointTypes.ProfilesNoCompletedTasksIn1Week,
                        profileId: taskStats.Any(x => x.ProfileId == p.Id && x.CompletedInLast1 == 0 && x.CompletedInLast2 != 0)
                                   || !taskStats.Any(x => x.ProfileId == p.Id) ? p.Id : 0);
                }

                if (!CommonHelper.IsMonday(fromDay))
                {
                    if (p.Created <= DateHelper2.ParseDate(dateId2ago))
                    {
                        MakeProfileAP(organizationDb,
                            ActionPointTypes.ProfilesNoCompletedTasksIn2Week,
                            profileId: taskStats.Any(x => x.ProfileId == p.Id && x.CompletedInLast2 == 0)
                                       || !taskStats.Any(x => x.ProfileId == p.Id) ? p.Id : 0);

                        MakeProfileAP(organizationDb,
                            ActionPointTypes.ProfilesLowImpactFor2Weeks,
                            profileId: reportsStats.Any(x => x.ProfileId == p.Id && x.ImpactFor1Weeks < 9 && x.ImpactFor2Weeks < 9)
                                       || !reportsStats.Any(x => x.ProfileId == p.Id) ? p.Id : 0);

                        MakeProfileAP(organizationDb,
                            ActionPointTypes.ProfilesLowImpactFor2Weeks,
                            profileId: reportsStats.Any(x => x.ProfileId == p.Id && x.SpeedFor1Weeks < 9 && x.SpeedFor2Weeks < 9)
                                       || !reportsStats.Any(x => x.ProfileId == p.Id) ? p.Id : 0);

                        MakeProfileAP(organizationDb,
                            ActionPointTypes.ProfilesLowImpactFor2Weeks,
                            profileId: reportsStats.Any(x => x.ProfileId == p.Id && x.HeatFor1Weeks < 9 && x.HeatFor2Weeks < 9)
                                       || !reportsStats.Any(x => x.ProfileId == p.Id) ? p.Id : 0);
                    }
                }
            }

            if (!CommonHelper.IsMonday(fromDay))
            {
                var segments = organizationDb.Segments.Select(x => new { x.Id, x.Created }).ToList();
                foreach (var proj in segments)
                {
                    if (proj.Created <= DateHelper2.ParseDate(dateId4ago))
                    {
                        if (shopItemsAddedCount == 0)
                        {
                            MakeSegmentAP(organizationDb, ActionPointTypes.ShopNoItemsAddedIn4Weeks, proj.Id);
                        }
                        if (challengesCreatedCount == 0)
                        {
                            MakeSegmentAP(organizationDb, ActionPointTypes.ChallengeNotCreatedIn4Weeks, proj.Id);
                        }
                    }
                }
            }


            //var existing = organizationDb.ActionPoints.Count(x => x.DateId == dateId);
            //if (existing > 0)
            //{
            //    logService.Log<MakeActionPointsLoader>($"deleting {existing} records from database");
            //    organizationDb.Database.ExecuteSqlCommand($"delete from ProfileReportsWeekly where {nameof(ProfileReportWeekly.DateId)} = {dateId}", dateId); //this extra parameter is a workaround in ef 2.2
            //    organizationDb.SaveChanges();
            //}

            organizationDb.SaveChanges();

            logService.Log<MakeActionPointsLoader>($"{reportsToInsert.Count} new profile reports saved to database.");
        }

        private static void MakeProfileAP(OrganizationDbContext dbContext, ActionPointTypes type, int profileId, int dateId = 0)
        {
            if (profileId <= 0)
                return;

            if (dateId == 0)
                dateId = DateHelper2.ToDateId(DateTime.UtcNow);

            dbContext.Add(new ActionPoint
            {
                Type = type,
                DateId = dateId,
                Profiles = new List<ActionPointProfile> {
                    new ActionPointProfile
                    {
                        ProfileId = profileId
                    }}
            });
        }

        private static void MakeSegmentAP(OrganizationDbContext dbContext, ActionPointTypes type, int segmentId, int dateId = 0)
        {
            if (segmentId <= 0)
                return;

            if (dateId == 0)
                dateId = DateHelper2.ToDateId(DateTime.UtcNow);

            dbContext.Add(new ActionPoint
            {
                Type = type,
                DateId = dateId,
                Segments = new List<ActionPointSegment> {
                    new ActionPointSegment
                    {
                        SegmentId = segmentId
                    }}
            });
        }

        private static void MakeAP(OrganizationDbContext dbContext, ActionPointTypes type, int dateId = 0)
        {
            if (dateId == 0)
                dateId = DateHelper2.ToDateId(DateTime.UtcNow);

            dbContext.Add(new ActionPoint
            {
                Type = type,
                DateId = dateId
            });
        }

        #endregion
    }
}
