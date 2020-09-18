using System;
using System.Collections.Generic;
using System.Linq;
using Ardalis.SmartEnum;
using Ardalis.SmartEnum.JsonNet;
using Cog.Core;
using Newtonsoft.Json;

namespace Tayra.Common
{
    public enum MetricTypes
    {
        Impact = 1,
        Speed = 2,
        Power = 3,
        Complexity = 4,
        Assist = 5,
        WorkUnitsCompleted = 6,
        Heat = 7
    }

    public class MetricRaw
    {
        public MetricType Type { get; set; }
        public float Value { get; set; }
        public int DateId { get; set; }
    }
    
    public class MetricRawWEntity
    {
        public int EntityId { get; set; }
        public MetricRaw MetricRaw { get; set; }
    }

    [JsonConverter(typeof(SmartEnumValueConverter<MetricType, int>))]
    public abstract class MetricType : SmartEnum<MetricType>
    {
        #region Tayra Metrics
        
        public static readonly MetricType Impact = new ImpactType("Impact", 101);
        public static readonly MetricType Speed = new SpeedType("Speed", 102);
        public static readonly MetricType Power = new PowerType("Power", 103);
        public static readonly MetricType Heat = new HeatType("Heat", 104);
        public static readonly MetricType Assists = new AssistsType("Assists", 105);
        public static readonly MetricType PraisesReceived = new PureType("Praises Received", 106);
        public static readonly MetricType PraisesGiven = new PureType("Praises Given", 107);
        public static readonly MetricType TokensEarned = new PureType("Tokens Earned", 108);
        public static readonly MetricType TokensSpent = new PureType("Tokens Spent", 109);
        public static readonly MetricType InventoryValueChange = new PureType("Inventory Value Change", 110);
        public static readonly MetricType ItemsBought = new PureType("Items Bought", 111);
        public static readonly MetricType ItemsDisenchanted = new PureType("Items Disenchanted", 112);
        public static readonly MetricType GiftsReceived = new PureType("Gifts Received", 113);
        public static readonly MetricType GiftsSent = new PureType("Gifts Sent", 114);
        
        #endregion
        
        #region Task Metrics
        
        public static readonly MetricType TasksCompleted = new PureType("Tasks Completed", 201);
        public static readonly MetricType Complexity = new PureType("Complexity", 202);
        public static readonly MetricType TimeWorked = new PureType("Time Worked", 203);
        public static readonly MetricType TimeWorkedLogged = new PureType("Time Worked Logged", 204);
        public static readonly MetricType Effort = new PureType("Effort", 205);
        //public static readonly MetricType Errors = new PureType("Errors", 206);
        //public static readonly MetricType Saves = new PureType("Saves", 207);
        
        #endregion

        #region Git Metrics
        
        public static readonly MetricType Commits = new PureType("Commits", 301);
        public static readonly MetricType CommitRate = new CommitRateType("Commit Rate", 302);

        #endregion

        private MetricType(string name, int value) : base(name, value)
        {
        }

        public abstract MetricType[] BuildingMetrics { get; }
        public abstract float Calc(MetricRaw[] buildingMetrics, DatePeriod datePeriod);
        public abstract float CalcGroup(MetricRaw[] buildingMetrics, DatePeriod datePeriod);

        private static IEnumerable<MetricRaw> RawMetricByType(MetricRaw[] metrics, MetricType type) => metrics.Where(x => x.Type == type.Value);
        private static float SumRawMetricByType(MetricRaw[] metrics, MetricType type) => RawMetricByType(metrics, type).Sum(x => x.Value);


        private sealed class PureType : MetricType
        {
            public PureType(string name, int value) : base(name, value)
            {
            }

            public override MetricType[] BuildingMetrics => Array.Empty<MetricType>();
            public override float CalcGroup(MetricRaw[] buildingMetrics, DatePeriod datePeriod) => Calc(buildingMetrics, datePeriod);
            public override float Calc(MetricRaw[] buildingMetrics, DatePeriod datePeriod)
            {
                var metricsInPeriod = buildingMetrics.Where(r => r.DateId >= datePeriod.FromId && r.DateId <= datePeriod.ToId).ToArray();
                return SumRawMetricByType(metricsInPeriod, this);
            }
        }

        private sealed class AssistsType : MetricType
        {
            public AssistsType(string name, int value) : base(name, value)
            {
            }

            public override MetricType[] BuildingMetrics => new[] { PraisesReceived };
            public override float CalcGroup(MetricRaw[] buildingMetrics, DatePeriod datePeriod)
            {
                var metricsInPeriod = buildingMetrics.Where(r => r.DateId >= datePeriod.FromId && r.DateId <= datePeriod.ToId).ToArray();
                return SumRawMetricByType(metricsInPeriod, MetricType.Assists);
            }
            public override float Calc(MetricRaw[] buildingMetrics, DatePeriod datePeriod)
            {
                var metricsInPeriod = buildingMetrics.Where(r => r.DateId >= datePeriod.FromId && r.DateId <= datePeriod.ToId).ToArray();
                var praisesGained = SumRawMetricByType(metricsInPeriod, PraisesReceived);
                return praisesGained;
            }
        }
        
        private sealed class ImpactType : MetricType
        {
            public ImpactType(string name, int value) : base(name, value)
            {
            }

            public override MetricType[] BuildingMetrics => new[] { Complexity, TasksCompleted, Assists };
            public override float CalcGroup(MetricRaw[] buildingMetrics, DatePeriod datePeriod)
            {
                var metricsInPeriod = buildingMetrics.Where(r => r.DateId >= datePeriod.FromId && r.DateId <= datePeriod.ToId).ToArray();
                return SumRawMetricByType(metricsInPeriod, this) / datePeriod.IterationsCount;
            }
            public override float Calc(MetricRaw[] buildingMetrics, DatePeriod datePeriod)
            {
                var metricsInPeriod = buildingMetrics.Where(r => r.DateId >= datePeriod.FromId && r.DateId <= datePeriod.ToId).ToArray();
                var complexity = SumRawMetricByType(metricsInPeriod, Complexity);
                var tasksCompleted = SumRawMetricByType(metricsInPeriod, TasksCompleted);
                var assists = Assists.Calc(metricsInPeriod, datePeriod);
                return (complexity + tasksCompleted + assists) / datePeriod.IterationsCount;
            }
        }

        private sealed class SpeedType : MetricType
        {
            public SpeedType(string name, int value) : base(name, value)
            {
            }

            public override MetricType[] BuildingMetrics => new[] { TasksCompleted };

            public override float CalcGroup(MetricRaw[] buildingMetrics, DatePeriod datePeriod) =>
                Calc(buildingMetrics, datePeriod);
            public override float Calc(MetricRaw[] buildingMetrics, DatePeriod datePeriod)
            {
                var metricsInPeriod = buildingMetrics.Where(r => r.DateId >= datePeriod.FromId && r.DateId <= datePeriod.ToId).ToArray();
                var tasksCompleted = SumRawMetricByType(metricsInPeriod, TasksCompleted);
                
                return tasksCompleted / datePeriod.IterationsCount;
            }
        }

        private sealed class PowerType : MetricType
        {
            public PowerType(string name, int value) : base(name, value)
            {
            }

            public override MetricType[] BuildingMetrics => new[] { Complexity, TasksCompleted };

            public override float CalcGroup(MetricRaw[] buildingMetrics, DatePeriod datePeriod) =>
                Calc(buildingMetrics, datePeriod);
            public override float Calc(MetricRaw[] buildingMetrics, DatePeriod datePeriod)
            {
                var metricsInPeriod = buildingMetrics.Where(r => r.DateId >= datePeriod.FromId && r.DateId <= datePeriod.ToId).ToArray();
                var complexity = SumRawMetricByType(metricsInPeriod, Complexity);
                var tasksCompleted = SumRawMetricByType(metricsInPeriod, TasksCompleted);
                
                if (tasksCompleted == 0) return 0f;
                return complexity / tasksCompleted;
            }
        }
        
        private sealed class CommitRateType : MetricType
        {
            public CommitRateType(string name, int value) : base(name, value)
            {
            }

            public override MetricType[] BuildingMetrics => new[] { Commits };
            
            public override float CalcGroup(MetricRaw[] buildingMetrics, DatePeriod datePeriod)
            {
                var metricsInPeriod = buildingMetrics.Where(r => r.DateId >= datePeriod.FromId && r.DateId <= datePeriod.ToId).ToArray();
                return SumRawMetricByType(metricsInPeriod, this) / datePeriod.WorkingDaysCount;
            }
            public override float Calc(MetricRaw[] buildingMetrics, DatePeriod datePeriod)
            {
                var metricsInPeriod = buildingMetrics.Where(r => r.DateId >= datePeriod.FromId && r.DateId <= datePeriod.ToId).ToArray();
                var daysWithCommits = RawMetricByType(metricsInPeriod, Commits).GroupBy(x => x.DateId).Count(d => d.Sum(c => c.Value) > 0);

                if (datePeriod.WorkingDaysCount == 0) return daysWithCommits;
                return (float)daysWithCommits / datePeriod.WorkingDaysCount;
            }
        }
        
        private sealed class HeatType : MetricType
        {
            public HeatType(string name, int value) : base(name, value)
            {
            }

            public override MetricType[] BuildingMetrics => new[] { Commits, TasksCompleted };

            public override float CalcGroup(MetricRaw[] buildingMetrics, DatePeriod datePeriod)
            {
                var metricsInPeriod = buildingMetrics.Where(r => r.DateId >= datePeriod.FromId && r.DateId <= datePeriod.ToId).ToArray();
                return SumRawMetricByType(metricsInPeriod, this) / datePeriod.DaysCount;
            }

            public override float Calc(MetricRaw[] buildingMetrics, DatePeriod datePeriod)
            {
                var commitsMatrix = new [] {0, 8, 16, 19, 21, 22};
                var tasksCompletedMatrix = new [] {0, 4, 8, 10, 11};
                var metricsInPeriod = buildingMetrics.Where(r => r.DateId >= datePeriod.FromId && r.DateId <= datePeriod.ToId).ToArray();

                var commitsScoreSum = SumDailyScores(RawMetricByType(metricsInPeriod, Commits), commitsMatrix);
                var tasksScoreSum = SumDailyScores(RawMetricByType(metricsInPeriod, TasksCompleted), tasksCompletedMatrix);

                return commitsScoreSum + tasksScoreSum;

                float SumDailyScores(IEnumerable<MetricRaw> raws, int[] matrix) => raws.GroupBy(x => x.DateId)
                    .Sum(x => matrix[(int) Math.Min(x.Sum(m => m.Value), matrix.Length - 1)]) / (float)datePeriod.DaysCount;  
            }
        }
    }
}
