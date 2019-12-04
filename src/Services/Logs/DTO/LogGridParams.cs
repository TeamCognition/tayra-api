using Firdaws.Core;

namespace Tayra.Services
{
    public class LogGridParams : GridParams
    {
        public string ProfileNickname { get; set; }
        public int? ProfileId { get; set; }
        public int? CompetitionId { get; set; }
        public int? ShopId { get; set; }
    }
}
