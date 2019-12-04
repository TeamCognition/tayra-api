namespace Tayra.Services
{
    public class CompetitionAddCompetitorDTO
    {
        public int? ProfileId { get; set; }

        public int? TeamId { get; set; }

        //temp name used in competition only
        public string DisplayName { get; set; }
    }
}
