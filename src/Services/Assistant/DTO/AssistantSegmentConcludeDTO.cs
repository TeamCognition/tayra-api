using Tayra.Common;

namespace Tayra.Services
{
    public class AssistantConcludeDTO
    {
        public int SegmentId { get; set; }
        public ActionPointTypes Type { get; set; }
        public int?[] Members { get; set; }
    }
}
