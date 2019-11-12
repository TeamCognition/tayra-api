using System;

namespace Tayra.Services
{
    public class IdentityEmailsGridDTO
    {
        public string EmailAddress { get; set; }
        public bool IsPrimary { get; set; }
        public DateTime AddedOn { get; set; }
    }
}
