using System;
using System.Linq;
using Cog.Core;

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
    }
}