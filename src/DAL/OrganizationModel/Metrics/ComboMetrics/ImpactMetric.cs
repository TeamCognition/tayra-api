using System.Linq;
using Cog.Core;

namespace Tayra.Analytics.Metrics
{
    public class ImpactMetric : ComboMetric
    {
        public ImpactMetric(string name, int value) : base(name, value)
        {
        }

        public override MetricType[] BuildingMetrics => new MetricType[] { Complexity, TasksCompleted, Assists };
        public override float CalcGroup(MetricShard[] buildingMetrics, DatePeriod datePeriod)
        {
            var metricsInPeriod = buildingMetrics.Where(r => r.DateId >= datePeriod.FromId && r.DateId <= datePeriod.ToId).ToArray();
            return SumRawMetricByType(metricsInPeriod, this) / datePeriod.IterationsCount;
        }
        public override float Calc(MetricShard[] buildingMetrics, DatePeriod datePeriod)
        {
            var metricsInPeriod = buildingMetrics.Where(r => r.DateId >= datePeriod.FromId && r.DateId <= datePeriod.ToId).ToArray();
            var complexity = SumRawMetricByType(metricsInPeriod, Complexity);
            var tasksCompleted = SumRawMetricByType(metricsInPeriod, TasksCompleted);
            var assists = Assists.Calc(metricsInPeriod, datePeriod);
            return (complexity + tasksCompleted + assists) / datePeriod.IterationsCount;
        }
    }
}