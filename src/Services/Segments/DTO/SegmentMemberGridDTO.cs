using System;
using Tayra.Common;

namespace Tayra.Services
{
    public class SegmentMemberGridDTO
    {
        public Guid ProfileId { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public ProfileRoles Role { get; set; }
        public string Avatar { get; set; }
        public DateTime MemberFrom { get; set; }
    }
}
