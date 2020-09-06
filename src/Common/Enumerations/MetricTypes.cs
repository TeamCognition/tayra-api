using System;
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

    [JsonConverter(typeof(SmartEnumValueConverter<MetricType, int>))]
    public abstract class MetricType : SmartEnum<MetricType>
    {
        public static readonly MetricType Impact = new ImpactType("Impact", 1);
        public static readonly MetricType Speed = new SpeedType("Speed", 2);
        public static readonly MetricType Power = new PowerType("Power", 3);
        
        public static readonly MetricType Complexity = new PureType("Complexity", 4);
        public static readonly MetricType Assists = new PureType("Assists", 5);
        public static readonly MetricType TasksCompleted = new PureType("Tasks Completed", 6); //WorkUnitsCompleted
        //7 fali 
        public static readonly MetricType Errors = new PureType("Errors", 8);
        public static readonly MetricType TokensEarned = new PureType("Tokens Earned", 9);
        public static readonly MetricType TokensSpent = new PureType("Tokens Spent", 10);
        public static readonly MetricType PraisesGiven = new PureType("Praises Given", 11);
        public static readonly MetricType PraisesReceived = new PureType("Praises Received", 12);
        public static readonly MetricType Effort = new PureType("Effort", 13);
        public static readonly MetricType Saves = new PureType("Saves", 14);
        public static readonly MetricType TimeWorked = new PureType("Time Worked", 15);
        public static readonly MetricType TimeWorkedLogged = new PureType("Time Worked Logged", 16);
        public static readonly MetricType ItemsInInventory = new PureType("Items in Inventory", 17);//items added to inventory
        public static readonly MetricType InventoryValue = new PureType("Inventory Value", 18);
        public static readonly MetricType ItemsBought = new PureType("Items Bought", 19);
        public static readonly MetricType GiftsReceived = new PureType("Gifts Received", 20);
        public static readonly MetricType GiftsSent = new PureType("Gifts Sent", 21);
        public static readonly MetricType ItemsDisenchanted = new PureType("Items Disenchanted", 22);
        public static readonly MetricType Commits = new PureType("Commits", 23);

        

        private MetricType(string name, int value) : base(name, value)
        {
        }

        public abstract MetricType[] BuildingMetrics { get; }
        public abstract float Calc(MetricRaw[] buildingMetrics, DatePeriod datePeriod);

        private static float SumRawMetricByType(MetricRaw[] metrics, MetricType type) =>
            metrics.Where(x => x.Type == type.Value).Sum(raw => raw.Value);


        private sealed class PureType : MetricType
        {
            public PureType(string name, int value) : base(name, value)
            {
            }

            public override MetricType[] BuildingMetrics => Array.Empty<MetricType>();
            public override float Calc(MetricRaw[] buildingMetrics, DatePeriod datePeriod)
            {
                var metricsInPeriod = buildingMetrics.Where(r => r.DateId >= datePeriod.FromId && r.DateId <= datePeriod.ToId).ToArray();
                return SumRawMetricByType(metricsInPeriod, this);
            }
        }

        private sealed class ImpactType : MetricType
        {
            public ImpactType(string name, int value) : base(name, value)
            {
            }

            public override MetricType[] BuildingMetrics => new[] { Complexity, TasksCompleted, Assists };
            public override float Calc(MetricRaw[] buildingMetrics, DatePeriod datePeriod)
            {
                var metricsInPeriod = buildingMetrics.Where(r => r.DateId >= datePeriod.FromId && r.DateId <= datePeriod.ToId).ToArray();
                var complexity = SumRawMetricByType(metricsInPeriod, Complexity);
                var tasksCompleted = SumRawMetricByType(metricsInPeriod, TasksCompleted);
                var assists = SumRawMetricByType(metricsInPeriod, Assists);
                return (complexity + tasksCompleted + assists) / datePeriod.SplitToIterations().Count();
            }
        }

        private sealed class SpeedType : MetricType
        {
            public SpeedType(string name, int value) : base(name, value)
            {
            }

            public override MetricType[] BuildingMetrics => new[] { TasksCompleted };
            public override float Calc(MetricRaw[] buildingMetrics, DatePeriod datePeriod)
            {
                var metricsInPeriod = buildingMetrics.Where(r => r.DateId >= datePeriod.FromId && r.DateId <= datePeriod.ToId).ToArray();
                var tasksCompleted = SumRawMetricByType(metricsInPeriod, TasksCompleted);
                return tasksCompleted / datePeriod.SplitToIterations().Count();
            }
        }

        private sealed class PowerType : MetricType
        {
            public PowerType(string name, int value) : base(name, value)
            {
            }

            public override MetricType[] BuildingMetrics => new[] { Complexity, TasksCompleted };
            public override float Calc(MetricRaw[] buildingMetrics, DatePeriod datePeriod)
            {
                var metricsInPeriod = buildingMetrics.Where(r => r.DateId >= datePeriod.FromId && r.DateId <= datePeriod.ToId).ToArray();
                var complexity = SumRawMetricByType(metricsInPeriod, Complexity);
                var tasksCompleted = SumRawMetricByType(metricsInPeriod, TasksCompleted);
                
                if (tasksCompleted == 0) return 0f;
                return complexity / tasksCompleted;
            }
        }
    }
}
