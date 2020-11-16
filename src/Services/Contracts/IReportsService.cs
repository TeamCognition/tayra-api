﻿using System.Collections.Generic;
using Cog.Core;
using Tayra.Common;

namespace Tayra.Services
{
    public interface IReportsService
    {
        Dictionary<int, MetricsValueWEntity[]> GetReports(string entityKey, EntityTypes entityType,ReportsType reportsType, DatePeriod period);
        
        ReportStatusDTO[] GetReportStatus(int[] segmentIds);
        void UnlockReporting(string tenantKey, int segmentId);

        ReportOverviewDTO GetOverviewReport(ReportParams reportParams);
        ReportMembersPerformanceDTO GetMembersPerformanceReport(ReportParams reportParams);

        ReportDeliverySegmentMetricsDTO GetDeliverySegmentMetricsReport(ReportParams reportParams);
        ReportDeliveryTeamMetricsDTO GetDeliveryTeamMetricsReport(int teamId, ReportParams reportParams);

        ReportStatisticsSegmentMetricsDTO GetStatisticsSegmentMetricsReport(ReportParams reportParams);
        ReportStatisticsTeamMetricsDTO GetStatisticsTeamMetricsReport(int teamId, ReportParams reportParams);

        ReportTokensSegmentMetricsDTO GetTokensSegmentMetricsReport(ReportParams reportParams);
        ReportTokensTeamMetricsDTO GetTokensTeamMetricsReport(ReportAggregationMethods aggrType, ReportTimeIntervals timeInternal, ReportParams reportParams);

        ReportItemsSegmentMetricsDTO GetItemsSegmentMetricsReport(ReportParams reportParams);
        ReportItemsTeamMetricsDTO GetItemTeamMetricsReport(int teamId, ReportParams reportParams);
    }
}
