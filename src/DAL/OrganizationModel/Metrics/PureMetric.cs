using System;
using System.Linq;
using Cog.Core;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Tayra.Common;
using Tayra.Models.Organizations;

namespace Tayra.Analytics.Metrics
{
    public abstract class PureMetric : MetricType
    {
        protected PureMetric(string name, int value) : base(name, value)
        {
        }

        public override MetricType[] BuildingMetrics => Array.Empty<MetricType>();
        public override float CalcGroup(MetricShard[] buildingMetrics, DatePeriod datePeriod) => Calc(buildingMetrics, datePeriod);
        public override float Calc(MetricShard[] buildingMetrics, DatePeriod datePeriod)
        {
            var metricsInPeriod = buildingMetrics.Where(r => r.DateId >= datePeriod.FromId && r.DateId <= datePeriod.ToId).ToArray();
            return SumRawMetricByType(metricsInPeriod, this);
        }

        public abstract object[] GetRawMetrics(OrganizationDbContext db, DatePeriod period, int entityId, EntityTypes entityType);

        protected int[] GetProfileIds(OrganizationDbContext db, int entityId, EntityTypes entityType)
        {
            switch (entityType)
            {
                case EntityTypes.Team:
                    return db.ProfileAssignments.Where(x => x.TeamId == entityId).Select(x => x.ProfileId).Distinct()
                        .ToArray();
                case EntityTypes.Segment:
                    return db.ProfileAssignments.Where(x => x.SegmentId == entityId).Select(x => x.ProfileId).Distinct()
                        .ToArray();
                case EntityTypes.Profile:
                    return new int[] {entityId};
                
                default: return new int[] {entityId};
            }
        }
    }
}