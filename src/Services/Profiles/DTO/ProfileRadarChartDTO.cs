namespace Tayra.Services
{
    public class ProfileRadarChartDTO
    {
        public double? AssistsAverage { get; set; }
        public int? AssistsTotal { get; set; }

        public double? TasksCompletedAverage { get; set; }
        public int? TasksCompletedTotal { get; set; }

        public double? ComplexityAverage { get; set; }
        public int? ComplexityTotal { get; set; }

        #region Team props

        public double? TeamAssistsAverage { get; set; }
        public double? TeamComplexityAverage { get; set; }
        public double? TeamTasksCompletedAverage { get; set; }

        #endregion
    }
}
