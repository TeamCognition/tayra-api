namespace Tayra.Services
{
    public class IdentityCreateDTO
    {
        public string Username { get; set; }
        public string Password { get; set; }

        public ProfileCreateDTO Profile { get; set; }
    }
}
