using Tayra.Common;

namespace Tayra.Services
{
    public class AdvisorSingleSegmentDTO
    {
        public ActionPointDTO[] ActionPoints { get; set; }
        public class ActionPointDTO
        {
            public int Id { get; set; }
            public ActionPointTypes Type { get; set; }
        }
    }
}