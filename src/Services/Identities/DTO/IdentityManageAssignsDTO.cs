using System;
using System.Collections.Generic;

namespace Tayra.Services
{
    public class IdentityManageAssignsDTO
    {
        public ICollection<CurrentAssignDTO> Current { get; set; }
        public ICollection<AvailableAssignDTO> Available { get; set; }

        public class CurrentAssignDTO
        {
            public Guid SegmentId { get; set; }
            public string SegmentName { get; set; }
            public Guid? TeamId { get; set; }
            public string TeamName { get; set; }
        }

        public class AvailableAssignDTO
        {
            public Guid SegmentId { get; set; }
            public TeamDTO[] Teams { get; set; }

            public class TeamDTO
            {
                public Guid TeamId { get; set; }
                public string Name { get; set; }
            }
        }
    }
}
