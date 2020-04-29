using System;
namespace Tayra.Services
{
    public class SegmentImpactLineChartDTO
    {
        public int StartDateId { get; set; }
        public int EndDateId { get; set; }

        public float[] Averages { get; set; }
    }
}
