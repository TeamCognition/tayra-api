using System;

namespace Tayra.Services
{
    public class ReportParams
    {
        public Guid SegmentId { get; set; }
        public int From { get; set; }
        public int To { get; set; }
    }
}