using System;
using Tayra.Common;

namespace Tayra.Services
{
    public class ActionPointGridDTO
    {
        public string SegmentKey {get; set;}
        public int TotalActionPoints { get; set; }
        public int HighImpact { get; set; }
        public int MediumImpact { get; set; }
        public int LowImpact { get; set; }
    }
}
