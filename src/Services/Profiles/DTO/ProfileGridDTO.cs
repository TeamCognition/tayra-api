using System;

namespace Tayra.Services
{
    public class ProfileGridDTO
    {
        public int ProfileId { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string Avatar { get; set; }
        public DateTime Created { get; set; }
    }
}
