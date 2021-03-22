using System.Collections.Generic;
using Tayra.Analytics;

namespace Tayra.Services
{
    public class SegmentRawScoreDTO
    {
        public Dictionary<int, MetricValue> Metrics { get; init; }
        public int DaysOnTayra { get; init; }

    }
}