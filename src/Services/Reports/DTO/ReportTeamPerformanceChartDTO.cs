using System.Collections.Generic;

namespace Tayra.Services
{
    public class ReportTeamPerformanceChartDTO
    {
        public IEnumerable<string> Labels { get; set; }
        public IEnumerable<Dataset> ProfilesDataset { get; set; }

        public class Dataset
        {
            public int ProfileId { get; set; }
            public string FullName { get; set; }
            public string Username { get; set; }
            public string Avatar { get; set; }
            public double[] AverageScores { get; set; }
        }
    }
}
