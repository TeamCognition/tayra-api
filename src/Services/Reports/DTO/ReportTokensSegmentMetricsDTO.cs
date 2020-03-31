namespace Tayra.Services
{
    public class ReportTokensSegmentMetricsDTO
    {
        public int StartDateId { get; set; }
        public int EndDateId { get; set; }

        public float TokensEarnedTotal { get; set; }
        public float TokensSpentTotal { get; set; }

        public float[] Earnings { get; set; }
        public float[] Spendings { get; set; }
    }
}
