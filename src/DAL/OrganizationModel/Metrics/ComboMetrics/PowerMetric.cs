using System.Linq;
using Cog.Core;

namespace Tayra.Analytics.Metrics
{
    public class PowerMetric : ComboMetric
    {
        public PowerMetric(string name, int value) : base(name, value)
        {
        }

        public override MetricType[] BuildingMetrics => new MetricType[] { Complexity, TasksCompleted };

        public override float CalcGroup(MetricShard[] buildingMetrics, DatePeriod datePeriod) =>
            Calc(buildingMetrics, datePeriod);
        public override float Calc(MetricShard[] buildingMetrics, DatePeriod datePeriod)
        {
            var metricsInPeriod = buildingMetrics.Where(r => r.DateId >= datePeriod.FromId && r.DateId <= datePeriod.ToId).ToArray();
            var complexity = SumRawMetricByType(metricsInPeriod, Complexity);
            var tasksCompleted = SumRawMetricByType(metricsInPeriod, TasksCompleted);

            if (tasksCompleted == 0) return 0f;
            return complexity / tasksCompleted;
        }
    }
}