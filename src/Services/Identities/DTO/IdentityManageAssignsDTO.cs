using System.Collections.Generic;

namespace Tayra.Services
{
    public class IdentityManageAssignsDTO
    {
        public ICollection<CurrentAssignDTO> Current { get; set; }
        public ICollection<AvailableAssignDTO> Available { get; set; }

        public class CurrentAssignDTO
        {
            public int SegmentId { get; set; }
            public string SegmentName { get; set; }
            public int TeamId { get; set; }
            public string TeamName { get; set; }
        }

        public class AvailableAssignDTO
        {
            public int SegmentId { get; set; }
            public (int TeamId, string Name)[] Teams { get; set; }
        }
    }
}
