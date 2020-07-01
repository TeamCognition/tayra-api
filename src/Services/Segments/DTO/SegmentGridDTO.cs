using System;
using Tayra.Common;

namespace Tayra.Services
{
    public class SegmentGridDTO
    {
        public int SegmentId { get; set; }
        public string Name { get; set; }
        public string Key { get; set; }
        public string Avatar { get; set; }
        public DateTime Created { get; set; }

        public int QuestsActive { get; set; }
        public int QuestsCompleted { get; set; }
        public int ShopItemsBought { get; set; }
        public IntegrationType[] Integrations  { get; set; }
        public int ActionPointsCount { get; set; }
    }
}
