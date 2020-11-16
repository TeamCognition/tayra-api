using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using Cog.Core;
using MailChimp.Net.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Tayra.Analytics;
using Tayra.Analytics.Metrics;
using Tayra.Common;
using Tayra.Models.Organizations;
using Tayra.Services;
using Tayra.Services.Analytics;
using Tayra.SyncServices.Metrics;
using MetricValue = Tayra.Services.MetricValue;

namespace Tayra.API.Controllers
{
    public class AnalyticsController : BaseController
    {
        #region Constructor
        public AnalyticsController(IServiceProvider serviceProvider, OrganizationDbContext dbContext) : base(serviceProvider)
        {
            OrganizationContext = dbContext;
        }

        #endregion

        #region Properties

        protected OrganizationDbContext OrganizationContext;

        #endregion

        #region Action Methods
        
        [HttpGet("metrics")]
        public Dictionary<int, MetricValue> GetMetrics([FromQuery] int[] m, [FromQuery] int entityId, [FromQuery] EntityTypes entityType, [FromQuery] string period)
        {
            var datePeriod = new DatePeriod(period);
            var metricTypes = m.Select(x => MetricType.FromValue(x)).ToArray();
            
            return AnalyticsService.GetMetrics(metricTypes, entityId, entityType, datePeriod);
        }
        
        [HttpGet("metricsWithBreakdown")]
        public Dictionary<int, AnalyticsMetricWithBreakdownDto> GetAnalyticsWithBreakdown([FromQuery] int[] m, [FromQuery] int entityId, [FromQuery] EntityTypes entityType, [FromQuery] string period)
        {
            var datePeriod = new DatePeriod(period);
            var metricTypes = m.Select(x => MetricType.FromValue(x)).ToArray();
            
            return AnalyticsService.GetMetricsWithBreakdown(metricTypes, entityId, entityType, datePeriod);
        }

        [HttpGet("rawMetrics")]
        public TableData GetRawMetrics([FromQuery] int m, [FromQuery] int entityId, [FromQuery] EntityTypes entityType, [FromQuery] string period)
        {
            var datePeriod = new DatePeriod(period);
            var metricType = MetricType.FromValue(m) as PureMetric;
            
            return new TableData(metricType.TypeOfRawMetric, metricType.GetRawMetrics(OrganizationContext, datePeriod, entityId, entityType));
        }
        
        public class AnalyticsWorkFocusDto
        {
            public int[] TotalTasks { get; set; }
            
            public int[] NewWorkBugIndex { get; set; }
            
            public int[] ComplexityTasksIndex { get; set; }
            
            public double[] SteadyVolatileIndex { get; set; }
           
        }

        [HttpGet("workFocus")]
public ActionResult<AnalyticsWorkFocusDto> GetWorkFocus([FromQuery] string period, [FromQuery] int teamId,
    [FromQuery] int profileId)
{
    var datePeriod = new DatePeriod(period);
    var profileHeatMetric = AnalyticsService.GetMetricsWithIterationSplit(
        new MetricType[]
        {
            MetricType.Heat
        }, profileId, EntityTypes.Profile, datePeriod);
    
    var teamHeatMetric = AnalyticsService.GetMetricsWithIterationSplit(
        new MetricType[]
        {
            MetricType.Heat
        }, 1, EntityTypes.Segment, datePeriod);
 
    double userHeatStandardDeviation = StandardDeviation(variance(profileHeatMetric[MetricType.Heat.Value].Iterations.Select(i => (double)i.Value).ToArray()));
    double teamHeatStandardDeviation = StandardDeviation(variance(teamHeatMetric[MetricType.Heat.Value].Iterations.Select(i => (double)i.Value).ToArray()));
    
    
    double StandardDeviation(double var)
    {
        return Math.Sqrt(var);
    }

    double variance(double[] nums) {
        if (nums.Length > 1)
        {
            double avg = nums.Average();
            double sumOfSquares = 0.0;
            foreach (int num in nums) {
                sumOfSquares += Math.Pow((num - avg), 2.0);
            }
            return sumOfSquares /  (nums.Length - 1);
        }
        else { return 0.0; }
    }
 
    var tasks = OrganizationContext.Tasks.Where(x => x.Status == TaskStatuses.Done && x.TeamId == teamId && x.LastModifiedDateId >= datePeriod.FromId && x.LastModifiedDateId <= datePeriod.ToId)
    .Select(x => new
    {
        AssigneeProfileId = x.AssigneeProfileId,
        TeamId = x.TeamId,
        Complexity = x.Complexity,
        Status = x.Status,
        Type = x.Type
    });
    
    var userDoneTasks = tasks.Count(x => x.AssigneeProfileId == profileId);
    var teamDoneTasks = tasks.Count(x => x.TeamId == teamId);
    
    var userNewWorkBug = tasks.Count(x => x.Type != TaskTypes.Bug && x.AssigneeProfileId == profileId);
    var teamNewWorkBug = tasks.Count(x => x.Type != TaskTypes.Bug && x.TeamId == teamId);
    
    var userComplexityIndex = tasks.Count(x => x.Complexity <= 2 && x.AssigneeProfileId == profileId) - tasks.Count(x => x.Complexity >= 3 && x.AssigneeProfileId == profileId);
    var teamComplexityIndex = tasks.Count(x => x.Complexity <= 2 && x.TeamId == teamId) - tasks.Count(x => x.Complexity >= 3 && x.TeamId == teamId);
    
    return new AnalyticsWorkFocusDto
    {
        TotalTasks = new []{userDoneTasks, teamDoneTasks},
        NewWorkBugIndex = new []{userNewWorkBug, teamNewWorkBug},
        ComplexityTasksIndex = new []{userComplexityIndex, teamComplexityIndex},
        SteadyVolatileIndex = new []{userHeatStandardDeviation, teamHeatStandardDeviation}
    };
}
        
        #endregion
    }
}
