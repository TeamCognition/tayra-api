using System.Collections.Generic;
using Tayra.Common;

namespace Tayra.Services
{
    public class LogCreateDTO
    {
        public LogEvents Event { get; set; }

        public Dictionary<string,string> Data { get; set; }

        public int? ProfileId { get; set; }
        public IEnumerable<int> CompetitionIds { get; set; }
        public int? ShopId { get; set; }
    }
}
