namespace Tayra.Services
{
    public class IdentityCreateDTO
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string TenantHost { get; set; }

        public ProfileCreateDTO Profile { get; set; }
    }
}
