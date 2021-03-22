using System;

namespace Tayra.Services
{
    public class SegmentMemberAddRemoveDTO
    {
        public Guid ProfileId { get; set; }
        public Guid SegmentId { get; set; }
        public Guid TeamId { get; set; }
    }
}
