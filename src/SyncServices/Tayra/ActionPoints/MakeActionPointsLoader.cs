﻿using System;
using System.Collections;
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

        public override void Execute(DateTime date, Dictionary<string, string> requestParams, params Tenant[] tenants)
        {
            foreach (var tenant in tenants)
            {
                LogService.SetOrganizationId(tenant.Key);
                using (var organizationDb = new OrganizationDbContext(null, new ShardTenantProvider(tenant.Key), _shardMapProvider))
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
                             group t by new { t.SegmentId, t.AssigneeProfileId } into total
                             let last1 = total.Where(x => x.LastModifiedDateId > dateId1ago) //last 1 iteration
                             let last2 = total.Where(x => x.LastModifiedDateId > dateId2ago) //last 2 iteration
                             select new
                             {
                                 SegmentId = total.Key.SegmentId,
                                 ProfileId = total.Key.AssigneeProfileId,
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


            var shopItemsAddedInLast4 = (from si in organizationDb.ShopItems
                                        where si.CreatedBy > 0
                                        where si.Created > DateHelper2.ParseDate(dateId4ago)
                                        select si).Count();


            var challengesCreatedInLast4 = (from c in organizationDb.ChallengeSegments
                                            where c.Created > DateHelper2.ParseDate(dateId4ago)
                                            group c by c.SegmentId into g
                                            select new
                                            {
                                                SegmentId = g.Key,
                                                Count = g.Count()
                                            }).ToArray();


            {//if Action Points where already generated today, delete since we are going to re-create them
                var existing = organizationDb.ActionPoints.Count(x => x.DateId == dateId);
                if (existing > 0)
                {
                    logService.Log<MakeActionPointsLoader>($"deleting {existing} records from database");
                    organizationDb.Database.ExecuteSqlCommand($"delete from {nameof(ActionPoint)}s where {nameof(ProfileReportWeekly.DateId)} = {dateId}", dateId); //this extra parameter is a workaround in ef 2.2
                    organizationDb.SaveChanges();
                }
            }

            var aps = organizationDb.ActionPoints.Where(x => x.ConcludedOn == null).ToArray();
            var profiles = organizationDb.Profiles.Select(x => new { x.Id, x.Created }).ToArray();
            var segments = organizationDb.Segments.Select(x => new { x.Id, x.Created, x.Members }).ToArray();
            foreach (var s in segments)
            {
                foreach (var p in profiles)
                {
                    if (!s.Members.Any(x => x.ProfileId == p.Id))
                        continue;

                    var joinDate = s.Members.OrderBy(x => x.Created).First(x => x.ProfileId == p.Id).Created;
                    var taskStatsBySegment = taskStats.Where(x => x.SegmentId == s.Id);
                    if (s.Created <= DateHelper2.ParseDate(dateId1ago) && joinDate <= DateHelper2.ParseDate(dateId1ago))
                    {
                        MakeProfileAP(organizationDb,
                            ActionPointTypes.ProfilesNoCompletedTasksIn1Week,
                            segmentId: s.Id,
                            profileId: p.Id,
                            isTrue: taskStatsBySegment.Any(x => x.ProfileId == p.Id && x.CompletedInLast1 == 0 && x.CompletedInLast2 != 0),//|| !taskStatsBySegment.Any(x => x.ProfileId == p.Id),
                            aps);
                    }

                    if (p.Created <= DateHelper2.ParseDate(dateId2ago) && joinDate <= DateHelper2.ParseDate(dateId2ago))
                    {
                        MakeProfileAP(organizationDb,
                            ActionPointTypes.ProfilesNoCompletedTasksIn2Week,
                            segmentId: s.Id,
                            profileId: p.Id,
                            isTrue: taskStatsBySegment.Any(x => x.ProfileId == p.Id && x.CompletedInLast2 == 0),//|| !taskStatsBySegment.Any(x => x.ProfileId == p.Id) ? p.Id : 0);
                            aps);
                        //lkpookpkopkopkopkopkopkoppokpihiuhiuhiuhoiu hoiu hoiu oh uo huiohu i hui hui huh uh ouh ouh uo oiuhoiuh oiuh oiuh oui
                        if (!CommonHelper.IsMonday(fromDay))
                        {
                            MakeProfileAP(organizationDb,
                                ActionPointTypes.ProfilesLowImpactFor2Weeks,
                                segmentId: s.Id,
                                profileId: p.Id,
                                isTrue: reportsStats.Any(x => x.ProfileId == p.Id && x.ImpactFor1Weeks < 6 && x.ImpactFor2Weeks < 6),//|| !reportsStats.Any(x => x.ProfileId == p.Id) ? p.Id : 0);
                                aps);

                            MakeProfileAP(organizationDb,
                                ActionPointTypes.ProfilesLowSpeedFor2Weeks,
                                segmentId: s.Id,
                                profileId: p.Id,
                                isTrue: reportsStats.Any(x => x.ProfileId == p.Id && x.SpeedFor1Weeks < 3 && x.SpeedFor2Weeks < 3),//|| !reportsStats.Any(x => x.ProfileId == p.Id) ? p.Id : 0);
                                aps);

                            MakeProfileAP(organizationDb,
                                ActionPointTypes.ProfilesLowHeatFor2Weeks,
                                segmentId: s.Id,
                                profileId: p.Id,
                                isTrue: reportsStats.Any(x => x.ProfileId == p.Id && x.HeatFor1Weeks < 15 && x.HeatFor2Weeks < 15),//|| !reportsStats.Any(x => x.ProfileId == p.Id) ? p.Id : 0);
                                aps);
                        }
                    }
                }
                //if (!CommonHelper.IsMonday(fromDay))
                {
                    if (s.Created <= DateHelper2.ParseDate(dateId4ago))
                    {
                        MakeSegmentAP(organizationDb, ActionPointTypes.ShopNoItemsAddedIn4Weeks, s.Id,
                            isTrue: shopItemsAddedInLast4 == 0,
                            aps);
                        
                        MakeSegmentAP(organizationDb, ActionPointTypes.ChallengeNotCreatedIn4Weeks, s.Id,
                            isTrue: (challengesCreatedInLast4.FirstOrDefault(x => x.SegmentId == s.Id)?.Count ?? 0) == 0,
                            aps);   
                    }
                }
            }

            organizationDb.SaveChanges();

            logService.Log<MakeActionPointsLoader>($"{reportsToInsert.Count} new profile reports saved to database.");
        }

        private static void MakeProfileAP(OrganizationDbContext dbContext, ActionPointTypes type, int? segmentId, int profileId, bool isTrue, ActionPoint[] aps, int dateId = 0)
        {
            var ap = aps.FirstOrDefault(x => x.Type == type && x.ProfileId == profileId); //if active ap exists
            if (ap != null)
            {
                if (!isTrue)
                {
                    ap.ConcludedOn = DateTime.UtcNow;
                }
                return;
            }

            if (!isTrue)
                return;

            if (dateId == 0)
                dateId = DateHelper2.ToDateId(DateTime.UtcNow);

            dbContext.Add(new ActionPoint
            {
                Type = type,
                DateId = dateId,
                ProfileId = profileId,
                SegmentId = segmentId
            });
        }

        private static void MakeSegmentAP(OrganizationDbContext dbContext, ActionPointTypes type, int segmentId, bool isTrue, ActionPoint[] aps, int dateId = 0)
        {
            var ap = aps.FirstOrDefault(x => x.Type == type && x.SegmentId == segmentId); //if active ap exists
            if (ap != null)
            {
                if (!isTrue)
                {
                    ap.ConcludedOn = DateTime.UtcNow;
                }
                return;
            }

            if (!isTrue)
                return;

            if (dateId == 0)
                dateId = DateHelper2.ToDateId(DateTime.UtcNow);

            dbContext.Add(new ActionPoint
            {
                Type = type,
                DateId = dateId,
                SegmentId = segmentId
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

            
        private class APointDTO
        {
            public int ProfileId { get; set; }
            public ActionPointTypes Type { get; set; }
        }

        #endregion
    }
}
