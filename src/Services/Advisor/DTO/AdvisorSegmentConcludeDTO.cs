using Tayra.Common;

namespace Tayra.Services
{
    public class AdvisorSegmentConcludeDTO
    {
        public int SegmentId { get; set; }
        public ActionPointTypes Type { get; set; }
        public int?[] Members { get; set; }
    }
}
