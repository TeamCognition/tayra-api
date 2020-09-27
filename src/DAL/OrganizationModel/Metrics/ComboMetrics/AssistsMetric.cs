using System.Linq;
using Cog.Core;

namespace Tayra.Analytics.Metrics
{
    public class AssistsMetric : ComboMetric
    {
        public AssistsMetric(string name, int value) : base(name, value)
        {
        }

        public override MetricType[] BuildingMetrics => new[] { PraisesReceived };
        public override float CalcGroup(MetricShard[] buildingMetrics, DatePeriod datePeriod)
        {
            var metricsInPeriod = buildingMetrics.Where(r => r.DateId >= datePeriod.FromId && r.DateId <= datePeriod.ToId).ToArray();
            return SumRawMetricByType(metricsInPeriod, MetricType.Assists);
        }
        public override float Calc(MetricShard[] buildingMetrics, DatePeriod datePeriod)
        {
            var metricsInPeriod = buildingMetrics.Where(r => r.DateId >= datePeriod.FromId && r.DateId <= datePeriod.ToId).ToArray();
            var praisesGained = SumRawMetricByType(metricsInPeriod, PraisesReceived);
            return praisesGained;
        }
    }
}