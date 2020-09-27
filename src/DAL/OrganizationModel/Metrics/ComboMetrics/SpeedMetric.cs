using System.Linq;
using Cog.Core;

namespace Tayra.Analytics.Metrics
{
    public class SpeedMetric : ComboMetric
    {
        public SpeedMetric(string name, int value) : base(name, value)
        {
        }

        public override MetricType[] BuildingMetrics => new[] { TasksCompleted };

        public override float CalcGroup(MetricShard[] buildingMetrics, DatePeriod datePeriod) =>
            Calc(buildingMetrics, datePeriod);
        public override float Calc(MetricShard[] buildingMetrics, DatePeriod datePeriod)
        {
            var metricsInPeriod = buildingMetrics.Where(r => r.DateId >= datePeriod.FromId && r.DateId <= datePeriod.ToId).ToArray();
            var tasksCompleted = SumRawMetricByType(metricsInPeriod, TasksCompleted);

            return tasksCompleted / datePeriod.IterationsCount;
        }
    }
}