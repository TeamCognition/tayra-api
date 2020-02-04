using System;
using System.Collections.Generic;

namespace Tayra.Services
{
    public class IdentityManageAssignsDTO
    {
        public ICollection<int> Current { get; set; }
        public ICollection<AssignDTO> Available { get; set; }

        public class AssignDTO
        {
            public int SegmentId { get; set; }
            public int[] TeamIds { get; set; }
        }
    }
}
