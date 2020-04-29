using System;

namespace Tayra.Services
{
    public class ProfileUpdateDTO
    {
        public string Avatar { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string JobPosition { get; set; }
        public DateTime? BornOn { get; set; }
        public DateTime? EmployedOn { get; set; }
        public string Username { get; set; }
    }
}