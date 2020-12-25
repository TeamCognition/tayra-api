using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cog.Core;
using Microsoft.EntityFrameworkCore;
using MoreLinq;
using Newtonsoft.Json.Linq;
using Tayra.Analytics;
using Tayra.Common;
using Tayra.Models.Catalog;
using Tayra.Models.Organizations;
using Tayra.Services;
using Tayra.SyncServices.Common;
using Tayra.SyncServices.Metrics;

namespace Tayra.SyncServices.Tayra
{
    public class NewSegmentMetricsLoader : BaseLoader
    {
        #region Constructor

        public NewSegmentMetricsLoader(LogService logService, CatalogDbContext catalogDb) : base(logService, catalogDb)
        {
        }

        #endregion

        #region Public Methods
        public override void Execute(DateTime date, JObject requestBody, params Tenant[] tenants)
        {
            foreach (var tenant in tenants)
            {
                LogService.SetOrganizationId(tenant.Identifier);
                using (var organizationDb = new OrganizationDbContext(TenantModel.WithConnectionStringOnly(tenant.ConnectionString), null))
                {
                    GenerateSegmentMetrics(organizationDb, date, LogService);
                }
            }
        }

        #endregion

        #region Private Methods

        public static List<SegmentMetric> GenerateSegmentMetrics(OrganizationDbContext organizationDb, DateTime fromDay, LogService logService)
        {
            var metricsToInsert = new List<SegmentMetric>();

            var dateId = DateHelper2.ToDateId(fromDay);

            var segmentIds = organizationDb.Segments.Select(x => x.Id).ToArray();

            foreach (var segmentId in segmentIds)
            {
                var profileIds = organizationDb.ProfileAssignments
                    .Where(
                        x => x.SegmentId == segmentId &&
                             x.Profile.IsAnalyticsEnabled /*&& x.Created <= DateHelper2.ParseDate(dateId)*/)
                    .Select(x => x.ProfileId)
                    .Distinct()
                    .ToArray();

                var rawMetrics = organizationDb.ProfileMetrics
                    .Where(x => x.DateId == dateId && profileIds.Contains(x.ProfileId))
                    .Where(x => x.SegmentId == null || x.SegmentId == segmentId)
                    .Select(x => new MetricShardWEntity
                    {
                        EntityId = x.ProfileId,
                        Type = x.Type,
                        Value = x.Value,
                        DateId = x.DateId
                    })
                    .ToArray();

                var segmentMetrics = MetricType.List
                        .Select(m => new SegmentMetric(segmentId, dateId, m, profileIds.Sum(x =>
                                m.Calc(rawMetrics.Where(m => m.EntityId == x).ToArray(),
                                new DatePeriod(dateId, dateId)) / profileIds.Length)));



                metricsToInsert.AddRange(segmentMetrics);
            }

            var existing = organizationDb.SegmentMetrics.Count(x => x.DateId == dateId);
            if (existing > 0)
            {
                logService.Log<NewSegmentMetricsLoader>($"date: ${dateId},  deleting {existing} records from database");
                organizationDb.Database.ExecuteSqlInterpolated($"delete from ProfileReportsDaily where {nameof(ProfileReportDaily.DateId)} = {dateId}");
                //organizationDb.Database.ExecuteSqlCommand($"delete from SegmentMetrics where {nameof(ProfileReportDaily.DateId)} = {dateId}", dateId); //this extra parameter is a workaround in ef 2.2
                organizationDb.SaveChanges();
            }

            organizationDb.SegmentMetrics.AddRange(metricsToInsert);

            organizationDb.SaveChanges();

            logService.Log<NewSegmentMetricsLoader>($"date: ${dateId}, {metricsToInsert.Count} new segment metrics saved to database.");
            return metricsToInsert;
        }

        #endregion
    }
}
