namespace Tayra.Services
{
    public class CompetitionGridDTO
    {
        public string TeamKey { get; set; } //TODO: add ?
        public double Points { get; set; } //ScoreValue is the name in entity
        public string Name { get; set; }
        public string Subtitle { get; set; }
        public string Avatar { get; set; }
    }
}
