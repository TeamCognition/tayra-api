namespace Tayra.Services
{
    public class ReportDeliverySegmentMetricsDTO
    {
        public int StartDateId { get; set; }
        public int EndDateId { get; set; }

        public int TotalTasksCompleted { get; set; }
        public int MaxTime { get; set; }
        public int MinTime { get; set; }
        public double AvgTime { get; set; }
        public TeamDTO[] Teams { get; set; }

        public class TeamDTO
        {
            public int TeamId { get; set; }
            public int[] AverageTaskCompletionTime { get; set; }
        }
    }
}
