namespace Tayra.Services
{
    public class ReportTeamsCompletedTasksChartDTO
    {
        public int TeamId { get; set; }
        public string TeamName { get; set; }
        public string TeamAvatar { get; set; }
        public int TasksCompletedTotal { get; set; }
    }
}
