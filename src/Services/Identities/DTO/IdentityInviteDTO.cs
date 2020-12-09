using System;
using Tayra.Common;

namespace Tayra.Services
{
    public class IdentityInviteDTO
    {
        public string EmailAddress { get; set; }
        public ProfileRoles Role { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        public Guid? SegmentId { get; set; }
        public Guid? TeamId { get; set; }
    }
}
