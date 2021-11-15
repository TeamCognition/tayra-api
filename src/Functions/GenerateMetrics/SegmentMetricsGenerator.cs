using System;
using System.Collections.Generic;
using System.Linq;
using Cog.Core;
using Microsoft.EntityFrameworkCore;
using Tayra.Analytics;
using Tayra.Models.Organizations;

namespace Tayra.Functions.GenerateMetrics
{
    public static class SegmentMetricsGenerator
    {
        public static List<SegmentMetric> GenerateAndSave(OrganizationDbContext organizationDb, DateTime fromDay, LogService logService)
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
                logService.Log(nameof(SegmentMetricsGenerator), $"date: ${dateId},  deleting {existing} records from database");
                organizationDb.Database.ExecuteSqlInterpolated($"delete from ProfileReportsDaily where {nameof(SegmentMetric.DateId)} = {dateId}");
                //organizationDb.Database.ExecuteSqlCommand($"delete from SegmentMetrics where {nameof(ProfileReportDaily.DateId)} = {dateId}", dateId); //this extra parameter is a workaround in ef 2.2
                organizationDb.SaveChanges();
            }

            organizationDb.SegmentMetrics.AddRange(metricsToInsert);

            organizationDb.SaveChanges();

            logService.Log(nameof(SegmentMetricsGenerator), $"date: ${dateId}, {metricsToInsert.Count} new segment metrics saved to database.");
            return metricsToInsert;
        }
    }
}
