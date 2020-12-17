using System.Collections.Generic;
using Tayra.Analytics;

namespace Tayra.Services
{
    public class TeamSwarmPlotDTO
    {
        public int LastUpdateDateId { get; set; }
        public Dictionary<int, MetricsValueWEntity[]> ProfileMetrics { get; set; }
        public Dictionary<int, MetricValue> Averages { get; set; }
    }
}