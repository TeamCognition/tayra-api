﻿using Tayra.Common;

namespace Tayra.Services
{
    public class AdvisorOverviewDTO
    {
        public ActionPointDTO[] ActionPoints { get; set; }
        public class ActionPointDTO
        {
            public int? SegmentId { get; set; }
            public int Count { get; set; }
            public ActionPointTypes[] Types { get; set; }
        }
    }
}
