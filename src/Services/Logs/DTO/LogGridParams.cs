using System;
using Cog.Core;

namespace Tayra.Services
{
    public class LogGridParams : GridParams
    {
        public string ProfileUsername { get; set; }
        public Guid[] ProfileIds { get; set; }
        public Guid[] SegmentIds { get; set; }
        public Guid[] TeamIds { get; set; }
        public bool? ShopLogs { get; set; }
    }
}
