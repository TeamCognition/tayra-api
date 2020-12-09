using System;

namespace Tayra.Services
{
    public class ReportStatusDTO
    {
        public Guid SegmentId { get; set; }

        public bool IsReportingUnlocked { get; set; }

        public int TotalMembers { get; set; }
        public int LinkedMembers { get; set; }
    }
}
