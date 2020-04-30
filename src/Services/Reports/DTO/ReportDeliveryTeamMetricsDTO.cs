namespace Tayra.Services
{
    public class ReportDeliveryTeamMetricsDTO
    {
        public int StartDateId { get; set; }
        public int EndDateId { get; set; }

        public float SpeedAverage { get; set; }
        public float ComplexityAverage { get; set; }

        public DataDTO[] Data { get; set; }

        public class DataDTO
        {
            public int DateId { get; set; }
            public TaskDTO[] Tasks { get; set; }

            public class TaskDTO
            {
                public string Name { get; set; }
                public int MinutesSpent { get; set; }
                public int Complexity { get; set; }
                public string AssigneeName { get; set; }
                public string AssigneeUsername { get; set; }
            }
        }
    }
}
