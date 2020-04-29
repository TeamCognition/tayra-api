using System;
using Tayra.Common;

namespace Tayra.Services
{
    public class ProfilePraiseDTO
    {
        public int ProfileId { get; set; }
        public PraiseTypes Type { get; set; }
        public DateTime? DemoDate  { get; set; }
    }
}
