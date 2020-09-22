using System.Collections.Generic;
using Tayra.Common;

namespace Tayra.Services
{
    public class TeamSwarmPlotDTO
    {
        public int LastUpdateDateId { get; set; }
        public Dictionary<int, AnalyticsMetricsWEntityDto[]> ProfileMetrics { get; set; }
        public Dictionary<int, AnalyticsMetricDto> Averages { get; set; }
    }
}