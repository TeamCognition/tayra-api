using System;

namespace Tayra.Services
{
    public class ReportItemsTeamMetricsDTO
    {
        public float InventoryValueAverage { get; set; }
        public MemberDTO[] Members { get; set; }

        public class MemberDTO
        {
            public Guid ProfileId { get; set; }
            public string Name { get; set; }
            public float InventoryValue { get; set; }
            public int InventoryCount { get; set; }
            public int GiftedCount { get; set; }
            public int DisenchantCount { get; set; }
        }
    }
}
