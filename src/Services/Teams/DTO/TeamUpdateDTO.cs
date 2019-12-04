namespace Tayra.Services
{
    public class TeamUpdateDTO : TeamCreateDTO
    {
        public string TeamKey { get; set; }
        public int? NewProjectId { get; set; }
    }
}
