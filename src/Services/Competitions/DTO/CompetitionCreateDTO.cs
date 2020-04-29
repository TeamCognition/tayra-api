using Tayra.Common;

namespace Tayra.Services
{
    public class CompetitionCreateDTO
    {
        public bool IsIndividual { get; set; }

        public string Name { get; set; }

        public CompetitionType? Type { get; set; }
        public CompetitionTheme? Theme { get; set; }
        public CompetitionStatus? Status { get; set; }

        public TokenType? Token { get; set; }
    }
}
