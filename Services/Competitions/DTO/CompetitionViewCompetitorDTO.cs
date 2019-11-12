using Tayra.Common;

namespace Tayra.Services
{
    public class CompetitionViewCompetitorDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsIndividual { get; set; }
        public CompetitionStatus Status { get; set; }
        public string TeamName { get; set; }
        public string CompetitorName { get; set; }
    }
}
