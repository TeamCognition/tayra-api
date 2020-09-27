using System;
using System.Collections.Generic;
using System.Linq;
using Ardalis.SmartEnum;
using Ardalis.SmartEnum.JsonNet;
using Cog.Core;
using Newtonsoft.Json;
using Tayra.Analytics.Metrics;
using Tayra.SyncServices.Metrics;

namespace Tayra.Analytics
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
    
    public class MetricRawWEntity
    {
        public int EntityId { get; set; }
        public MetricShard MetricShard { get; set; }
    }
    
    [JsonConverter(typeof(SmartEnumValueConverter<MetricType, int>))]
    public abstract class MetricType : SmartEnum<MetricType>
    {
        #region Tayra Metrics
        
        public static readonly ComboMetric Impact = new ImpactMetric("Impact", 101);
        public static readonly ComboMetric Speed = new SpeedMetric("Speed", 102);
        public static readonly ComboMetric Power = new PowerMetric("Power", 103);
        public static readonly ComboMetric Heat = new HeatMetric("Heat", 104);
        public static readonly ComboMetric Assists = new AssistsMetric("Assists", 105);
        public static readonly PraisesReceivedMetric PraisesReceived = new PraisesReceivedMetric("Praises Received", 106);
        public static readonly PraisesGivenMetric PraisesGiven = new PraisesGivenMetric("Praises Given", 107);
        public static readonly TokensEarnedMetric TokensEarned = new TokensEarnedMetric("Tokens Earned", 108);
        public static readonly TokensSpentMetric TokensSpent = new TokensSpentMetric("Tokens Spent", 109);
        public static readonly InventoryValueChangeMetric InventoryValueChange = new InventoryValueChangeMetric("Inventory Value Change", 110);
        public static readonly ItemsBoughtMetric ItemsBought = new ItemsBoughtMetric("Items Bought", 111);
        public static readonly ItemsDisenchantedMetric ItemsDisenchanted = new ItemsDisenchantedMetric("Items Disenchanted", 112);
        public static readonly GiftsReceivedMetric GiftsReceived = new GiftsReceivedMetric("Gifts Received", 113);
        public static readonly GiftsSentMetric GiftsSent = new GiftsSentMetric("Gifts Sent", 114);
        
        #endregion
        
        #region Task Metrics
        
        public static readonly WorkUnitsCompletedMetric TasksCompleted = new WorkUnitsCompletedMetric("Tasks Completed", 201);
        public static readonly ComplexityMetric Complexity = new ComplexityMetric("Complexity", 202);
        public static readonly TimeWorkedMetric TimeWorked = new TimeWorkedMetric("Time Worked", 203);
        public static readonly TimeWorkedLoggedMetric TimeWorkedLogged = new TimeWorkedLoggedMetric("Time Worked Logged", 204);
        public static readonly EffortMetric Effort = new EffortMetric("Effort", 205);
        //public static readonly MetricType Errors = new PureType("Errors", 206);
        //public static readonly MetricType Saves = new PureType("Saves", 207);
        
        #endregion

        #region Git Metrics
        
        public static readonly CommitsMetric Commits = new CommitsMetric("Commits", 301);
        public static readonly ComboMetric CommitRate = new CommitRateMetric("Commit Rate", 302);

        #endregion

        protected MetricType(string name, int value) : base(name, value)
        {
        }
        
        
        public abstract MetricType[] BuildingMetrics { get; }
        public abstract float Calc(MetricShard[] buildingMetrics, DatePeriod datePeriod);
        public abstract float CalcGroup(MetricShard[] buildingMetrics, DatePeriod datePeriod);

        protected static IEnumerable<MetricShard> RawMetricByType(MetricShard[] metrics, MetricType type) => metrics.Where(x => x.Type == type.Value);
        protected static float SumRawMetricByType(MetricShard[] metrics, MetricType type) => RawMetricByType(metrics, type).Sum(x => x.Value);


        private sealed class PureType : MetricType
        {
            public PureType(string name, int value) : base(name, value)
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
}
