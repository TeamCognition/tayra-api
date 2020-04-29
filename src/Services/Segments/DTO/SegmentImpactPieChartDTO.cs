namespace Tayra.Services
{
    public class SegmentImpactPieChartDTO
    {
        public TeamDTO[] Teams { get; set; }

        public class TeamDTO
        {
            public string Name { get; set; }
            public float ImpactPercentage { get; set; }
        }
    }
}
