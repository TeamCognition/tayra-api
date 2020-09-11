namespace Tayra.Services
{
    public class TeamPulseDTO
    {
        public int InProgress { get; set; }
        public int RecentlyDone { get; set; }
        public string JiraBoardUrl { get; set; }
    }
}