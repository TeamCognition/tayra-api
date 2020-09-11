namespace Tayra.Services
{
    public class ProfileHeatStreamDTO
    {
        public int LatestUpdateDateId { get; set; }
        public HeatWeekNode[] Nodes { get; set; }

        public class HeatWeekNode
        {
            public int DateId { get; set; }
            public float Value { get; set; }
        }
    }
}