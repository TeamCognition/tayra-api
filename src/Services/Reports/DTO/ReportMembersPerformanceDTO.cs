using System.Collections.Generic;
using Tayra.Common;

namespace Tayra.Services
{
    public class ReportMembersPerformanceDTO
    {
        public MembersPerformanceDTO[] MembersPerformance { get; set; }
        public class MembersPerformanceDTO
        {
            public string Name { get; set; }
            public float Impact { get; set; }
            public int TasksCompleted { get; set; }
            public float Complexity { get; set; }
            public int Assists { get; set; }
            public float AvarageCompletionTime { get; set; }
            public float Speed { get; set; }
            public float Power { get; set; }
            public float Tokens { get; set; }
            public float InventoryValue { get; set; }
            public int InventoryItems { get; set; }
        }
    }
}