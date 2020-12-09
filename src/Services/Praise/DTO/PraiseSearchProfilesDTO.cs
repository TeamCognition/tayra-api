using Cog.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tayra.Services
{
    public class PraiseSearchProfilesDTO
    {
        public Guid ProfileId { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string Avatar { get; set; }
    }
}