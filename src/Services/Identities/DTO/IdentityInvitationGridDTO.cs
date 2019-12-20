using System;
using Tayra.Common;

namespace Tayra.Services
{
    public class IdentityInvitationGridDTO
    {
        public string EmailAddress { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public InvitationStatus Status { get; set; }
        public DateTime Created { get; set; }
    }
}
