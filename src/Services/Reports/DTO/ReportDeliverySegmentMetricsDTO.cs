namespace Tayra.Services
{
    public class ReportDeliverySegmentMetricsDTO
    {
        public int StartDateId { get; set; }
        public int EndDateId { get; set; }

        public TeamDTO[] Teams { get; set; }

        public class TeamDTO
        {
            public int TeamId { get; set; }
            public int[] AverageTaskCompletionTime { get; set; }
        }
    }
}
