using System;
using Tayra.Common;

namespace Tayra.Services
{
    public class TokenLookupDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Symbol { get; set; }
        public TokenType Type { get; set; }
    }
}
