using Tayra.Common;

namespace Tayra.Services
{
    public class ProfileCreateDTO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Nickname { get; set; }

        public string Avatar { get; set; }

        public ProfileRoles Role { get; set; }
    }
}
