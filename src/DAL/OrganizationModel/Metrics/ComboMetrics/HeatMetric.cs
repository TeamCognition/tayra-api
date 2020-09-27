using System;
using System.Collections.Generic;
using System.Linq;
using Cog.Core;

namespace Tayra.Analytics.Metrics
{
    public class HeatMetric : ComboMetric
    {
        public HeatMetric(string name, int value) : base(name, value)
        {
        }

        public override MetricType[] BuildingMetrics => new MetricType[] { Commits, TasksCompleted };

        public override float CalcGroup(MetricShard[] buildingMetrics, DatePeriod datePeriod)
        {
            var metricsInPeriod = buildingMetrics.Where(r => r.DateId >= datePeriod.FromId && r.DateId <= datePeriod.ToId).ToArray();
            return SumRawMetricByType(metricsInPeriod, this) / datePeriod.DaysCount;
        }

        public override float Calc(MetricShard[] buildingMetrics, DatePeriod datePeriod)
        {
            var commitsMatrix = new[] { 0, 8, 16, 19, 21, 22 };
            var tasksCompletedMatrix = new[] { 0, 4, 8, 10, 11 };
            var metricsInPeriod = buildingMetrics.Where(r => r.DateId >= datePeriod.FromId && r.DateId <= datePeriod.ToId).ToArray();

            var commitsScoreSum = SumDailyScores(RawMetricByType(metricsInPeriod, Commits), commitsMatrix);
            var tasksScoreSum = SumDailyScores(RawMetricByType(metricsInPeriod, TasksCompleted), tasksCompletedMatrix);

            return commitsScoreSum + tasksScoreSum;

            float SumDailyScores(IEnumerable<MetricShard> raws, int[] matrix) => raws.GroupBy(x => x.DateId)
                .Sum(x => matrix[(int)Math.Min(x.Sum(m => m.Value), matrix.Length - 1)]) / (float)datePeriod.DaysCount;
        }
    }
}