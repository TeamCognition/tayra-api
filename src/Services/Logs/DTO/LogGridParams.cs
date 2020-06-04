using Cog.Core;

namespace Tayra.Services
{
    public class LogGridParams : GridParams
    {
        public string ProfileUsername { get; set; }
        public int[] ProfileIds { get; set; }
        public int[] SegmentIds { get; set; }
        public int[] TeamIds { get; set; }
        public bool? ShopLogs { get; set; }
    }
}
