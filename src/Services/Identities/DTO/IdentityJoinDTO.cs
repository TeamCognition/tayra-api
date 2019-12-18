using System.ComponentModel.DataAnnotations;

namespace Tayra.Services
{
    public class IdentityJoinDTO
    {
        public string InvitationCode { get; set; }
        public string Avatar { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        [MaxLength(100)]//this is not handled I think
        public string JobPosition { get; set; }
        public string Password { get; set; }
    }
}
