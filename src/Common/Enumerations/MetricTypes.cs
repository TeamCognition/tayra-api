using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Ardalis.SmartEnum;
using Cog.Core;

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
        Heat = 7,
        Errors = 8,
        TokensEarned = 9,
        TokensSpent = 10,
        PraisesGiven = 11,
        PraisesReceived = 12,
        Effort = 13,
        Saves = 14,
        TimeWorked = 15,
        TimeWorkedLogged = 16,
        ItemsInInventory = 17,
        InventoryValue = 18,
        ItemsBought = 19,
        GiftsSent = 20,
        GiftsReceived = 21,
        ItemsDisenchanted = 22,
        Commits = 23,
    }

    public class DatePeriod
    {
        public DateTime From { get; }

        public DateTime To { get; }

        public int FromId => DateHelper2.ToDateId(From);
        public int ToId => DateHelper2.ToDateId(To);

        public DatePeriod(int fromId, int toId)
        {
            if (fromId > toId)
            {
                throw new ApplicationException("'from' must be smaller than 'to'");
            }

            From = DateHelper2.ParseDate(fromId);
            To = DateHelper2.ParseDate(toId);
        }

        public override string ToString() => $"{DateHelper2.ToDateId(From)}-{DateHelper2.ToDateId(To)}";

        public DatePeriod(DateTime from, DateTime to)
        {
            if (from >= to)
            {
                throw new ApplicationException("'from' must be smaller than 'to'");
            }
            From = from;
            To = to;
        }
        
        public IEnumerable<DatePeriod> SplitToIterations(int iterationDaysCount = 7)
        {
            DateTime iterationFrom = From;
            DateTime iterationTo;
            do
            {
                var tempTo = iterationFrom.AddDays(iterationDaysCount);
                iterationTo = tempTo >= To ? To : tempTo;
                yield return new DatePeriod(iterationFrom, iterationTo);
                iterationFrom = iterationTo.AddDays(1);
            } while (iterationTo < To);
        }
    }

    public class MetricRaw
    {
        public MetricType Type { get; set; }
        public float Value { get; set; }
        public int DateId { get; set; }
    }
    
    public class MetricDto
    {
        public MetricType Type { get; set; }
        public float Value { get; set; }
        public DatePeriod Period { get; set; }
        public BreakdownMetricDto[] BreakdownMetrics { get; set; }
        
        public class BreakdownMetricDto
        {
            public MetricType Type { get; set; }
            public IterationPeriodDto[] Periods { get; set; }

            public BreakdownMetricDto(MetricType type, DatePeriod period, MetricRaw[] raws)
            {
                Type = type;
                Periods = period.SplitToIterations().Select(p => new IterationPeriodDto
                {
                    IterationPeriod = p.ToString(),
                    Value = type.Calc(raws.Where(r => r.DateId >= p.FromId && r.DateId <= p.ToId).ToArray())
                }).ToArray();
            }
            public class IterationPeriodDto
            {
                public string IterationPeriod { get; set; }
                public float Value { get; set; }   
            }
        }

        public MetricDto(MetricType metricType, DatePeriod period, MetricRaw[] raws)
        {
            this.Type = metricType;
            this.Value = metricType.Calc(raws);
            this.Period = period;
            this.BreakdownMetrics = metricType.BuildingMetrics.Select(bType => new BreakdownMetricDto(bType, period, raws)).ToArray();
        }
    }
    
    public abstract class MetricType : SmartEnum<MetricType>
    {
        public static readonly MetricType Complexity = new PureType("Complexity", 4);
        public static readonly MetricType TasksCompleted = new PureType("TasksCompleted", 5); //WorkUnitsCompleted
        public static readonly MetricType Assists = new PureType("Assists", 5);
        public static readonly MetricType TokensEarned = new PureType("Tokens Earned", 9);
        public static readonly MetricType TokensSpent = new PureType("Tokens Spent", 10);
        public static readonly MetricType PraisesGiven = new PureType("Praises Given", 11);
        public static readonly MetricType PraisesReceived = new PureType("Praises Received", 12);
        public static readonly MetricType Errors = new PureType("Errors", 8);
        public static readonly MetricType Effort = new PureType("Effort", 13);
        public static readonly MetricType Saves = new PureType("Saves", 14);
        public static readonly MetricType TimeWorked = new PureType("Time Worked", 15);
        public static readonly MetricType TimeWorkedLogged = new PureType("Time Worked Logged", 16);
        public static readonly MetricType ItemsInInventory = new PureType("Items In Inventory", 17);
        public static readonly MetricType InventoryValue = new PureType("Inventory Value", 18);
        public static readonly MetricType ItemsBought = new PureType("Items Bought", 19);
        public static readonly MetricType GiftsReceived = new PureType("Gifts Received", 20);
        public static readonly MetricType GiftsSent = new PureType("Gifts Sent", 21);
        public static readonly MetricType ItemsDisenchanted = new PureType("Items Disenchanted", 22);
        public static readonly MetricType Commits = new PureType("Commits", 23);
        
        public static readonly MetricType Impact = new ImpactType("Impact", 1);

        private MetricType(string name, int value) : base(name, value)
        {
        }

        public abstract bool ShouldDivideWithIterationCount { get; }
        public abstract MetricType[] BuildingMetrics { get; }
        public abstract float Calc(MetricRaw[] buildingMetrics);

        static float SumRawMetricByType(MetricRaw[] metrics, MetricType type) =>
            metrics.Where(x => x.Type == type.Value).Sum(raw => raw.Value);

        
        private sealed class PureType: MetricType
        {
            public PureType(string name, int value) : base(name, value)
            {
            }

            public override bool ShouldDivideWithIterationCount => false;
            public override MetricType[] BuildingMetrics => new MetricType[]{};
            public override float Calc(MetricRaw[] buildingMetrics) => SumRawMetricByType(buildingMetrics, MetricType.FromValue(Value));
        }
        
        private sealed class ImpactType: MetricType
        {
            public ImpactType(string name, int value) : base(name, value)
            {
            }

            public override bool ShouldDivideWithIterationCount => true;
            public override MetricType[] BuildingMetrics => new[] {Complexity, TasksCompleted, Assists};
            public override float Calc(MetricRaw[] buildingMetrics)
            {
                var complexity = SumRawMetricByType(buildingMetrics, Complexity);
                var tasksCompleted = SumRawMetricByType(buildingMetrics, TasksCompleted);
                var assists = SumRawMetricByType(buildingMetrics, Assists);
                return complexity + tasksCompleted + assists;
            }
        }
    }
}
