using Cog.Core;

namespace Tayra.Services
{
    public class IdentityInvitationGridParams : GridParams
    {
        public bool ActiveStatusesOnly { get; set; } //maybe use gridParams.filters here
    }
}
