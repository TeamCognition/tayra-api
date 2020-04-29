﻿using Tayra.Common;

namespace Tayra.Services
{
    public class AssistantOverviewDTO
    {
        public ActionPointDTO[] ActionPoints { get; set; }
        public class ActionPointDTO
        {
            public int? SegmentId { get; set; }
            public ActionPointTypes[] Types { get; set; }
        }
    }
}
