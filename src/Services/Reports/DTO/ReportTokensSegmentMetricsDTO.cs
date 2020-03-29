namespace Tayra.Services
{
    public class ReportTokensSegmentMetricsDTO
    {
        public int StartDateId { get; set; }
        public int EndDateId { get; set; }

        public float TokensEarnedAverage { get; set; }
        public float TokensSpentAverage { get; set; }

        public float[] Earnings { get; set; }
        public float[] Spendings { get; set; }
    }
}
