using System;
using Tayra.Common;

namespace Tayra.Services
{
    public class AdvisorOverviewDTO
    {
        public ActionPointsDTO[] ActionPoints { get; set; }
        public class ActionPointsDTO
        {
            public int SegmentId { get; set; }
            public int[] ActionPoints { get; set; }
        }
    }
}
