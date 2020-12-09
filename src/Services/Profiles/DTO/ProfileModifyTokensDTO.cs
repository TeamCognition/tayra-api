using System;

namespace Tayra.Services
{
    public class ProfileModifyTokensDTO
    {
        public Guid ProfileId { get; set; }
        public double TokenValue { get; set; }
    }
}
