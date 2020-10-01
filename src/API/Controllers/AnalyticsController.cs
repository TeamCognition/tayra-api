using System;
using System.Collections.Generic;
using System.Linq;
using Cog.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Tayra.Analytics;
using Tayra.Analytics.Metrics;
using Tayra.Common;
using Tayra.Models.Organizations;
using Tayra.Services;
using Tayra.Services.Analytics;
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

        #endregion
    }
}
