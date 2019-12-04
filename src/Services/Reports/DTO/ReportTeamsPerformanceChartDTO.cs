using System.Collections.Generic;

namespace Tayra.Services
{
    public class ReportTeamsPerformanceChartDTO
    {
        public IEnumerable<string> Labels { get; set; }
        public IEnumerable<Dataset> Datasets { get; set; }

        public class Dataset
        {
            public int TeamId { get; set; }
            public string TeamName { get; set; }
            public double[] AverageScores { get; set; }
        }
    }
}
