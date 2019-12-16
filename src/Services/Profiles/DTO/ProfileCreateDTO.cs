using Tayra.Common;

namespace Tayra.Services
{
    public class ProfileCreateDTO
    {
        public string Nickname { get; set; }

        public string Avatar { get; set; }

        public ProfileRoles Role { get; set; }
    }
}
