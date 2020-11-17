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
    
    [JsonConverter(typeof(SmartEnumValueConverter<MetricType, int>))]
    public abstract class MetricType: SmartEnum<MetricType>
    {    
        #region Tayra Metrics
        
        public static readonly MetricType Impact = new ImpactMetric("Impact", 101);
        public static readonly MetricType Speed = new SpeedMetric("Speed", 102);
        public static readonly MetricType Power = new PowerMetric("Power", 103);
        public static readonly MetricType Heat = new HeatMetric("Heat", 104);
        public static readonly MetricType Assists = new AssistsMetric("Assists", 105);
        public static readonly MetricType PraisesReceived = new PraisesReceivedMetric("Praises Received", 106);
        public static readonly MetricType PraisesGiven = new PraisesGivenMetric("Praises Given", 107);
        public static readonly MetricType TokensEarned = new TokensEarnedMetric("Tokens Earned", 108);
        public static readonly MetricType TokensSpent = new TokensSpentMetric("Tokens Spent", 109);
        public static readonly MetricType InventoryValueChange = new InventoryValueChangeMetric("Inventory Value Change", 110);
        public static readonly MetricType ItemsBought = new ItemsBoughtMetric("Items Bought", 111);
        public static readonly MetricType ItemsDisenchanted = new ItemsDisenchantedMetric("Items Disenchanted", 112);
        public static readonly MetricType GiftsReceived = new GiftsReceivedMetric("Gifts Received", 113);
        public static readonly MetricType GiftsSent = new GiftsSentMetric("Gifts Sent", 114);
        public static readonly MetricType CommentsPerPr = new CommentsPerPrMetric("Comments per PR",115);
        public static readonly MetricType PullRequestsCreated = new CommentsPerPrMetric("Pull requests created",116);
        public static readonly MetricType PullRequestsReviewed = new CommentsPerPrMetric("Pull requests reviewed",117);

        #endregion
        
        #region Task Metrics
        
        public static readonly MetricType TasksCompleted = new WorkUnitsCompletedMetric("Tasks Completed", 201);
        public static readonly MetricType Complexity = new ComplexityMetric("Complexity", 202);
        public static readonly MetricType TimeWorked = new TimeWorkedMetric("Time Worked", 203);
        public static readonly MetricType TimeWorkedLogged = new TimeWorkedLoggedMetric("Time Worked Logged", 204);
        public static readonly MetricType Effort = new EffortMetric("Effort", 205);
        //public static readonly MetricType Errors = new PureType("Errors", 206);
        //public static readonly MetricType Saves = new PureType("Saves", 207);
        
        #endregion
        
        #region Git Metrics
        
        public static readonly MetricType Commits = new CommitsMetric("Commits", 301);
        public static readonly MetricType CommitRate = new CommitRateMetric("Commit Rate", 302);
        
        #endregion

        protected MetricType(string name, int value) : base(name, value)
        {
        }
        
        
        public abstract MetricType[] BuildingMetrics { get; }
        public abstract float Calc(MetricShard[] buildingMetrics, DatePeriod datePeriod);
        public abstract float CalcGroup(MetricShard[] buildingMetrics, DatePeriod datePeriod);

        protected static IEnumerable<MetricShard> RawMetricByType(MetricShard[] metrics, MetricType type) => metrics.Where(x => x.Type == type.Value);
        protected static float SumRawMetricByType(MetricShard[] metrics, MetricType type) => RawMetricByType(metrics, type).Sum(x => x.Value);
    }
}
