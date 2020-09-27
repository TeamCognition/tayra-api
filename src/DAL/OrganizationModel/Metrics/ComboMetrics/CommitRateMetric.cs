using System.Linq;
using Cog.Core;

namespace Tayra.Analytics.Metrics
{
    public class CommitRateMetric : ComboMetric
    {
        public CommitRateMetric(string name, int value) : base(name, value)
        {
        }

        public override MetricType[] BuildingMetrics => new[] { Commits };

        public override float CalcGroup(MetricShard[] buildingMetrics, DatePeriod datePeriod)
        {
            var metricsInPeriod = buildingMetrics.Where(r => r.DateId >= datePeriod.FromId && r.DateId <= datePeriod.ToId).ToArray();
            return SumRawMetricByType(metricsInPeriod, this) / datePeriod.WorkingDaysCount;
        }
        public override float Calc(MetricShard[] buildingMetrics, DatePeriod datePeriod)
        {
            var metricsInPeriod = buildingMetrics.Where(r => r.DateId >= datePeriod.FromId && r.DateId <= datePeriod.ToId).ToArray();
            var daysWithCommits = RawMetricByType(metricsInPeriod, Commits).GroupBy(x => x.DateId).Count(d => d.Sum(c => c.Value) > 0);

            if (datePeriod.WorkingDaysCount == 0) return daysWithCommits;
            return (float)daysWithCommits / datePeriod.WorkingDaysCount;
        }
    }
}