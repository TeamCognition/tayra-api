using Tayra.Common;

namespace Tayra.Services
{
    public class IdentityInvitationViewDTO
    {
        public string EmailAddress { get; set; }
        public ProfileRoles Role { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public InvitationStatus Status { get; set; }
    }
}
