using System;
using Cog.Core;
using RestSharp.Deserializers;

namespace Tayra.Connectors.GitHub
{
    public class TokenResponse
    {
        [DeserializeAs(Name = "access_token")]
        public string AccessToken { get; set; }

        [DeserializeAs(Name = "token_type")]
        public string TokenType { get; set; }

        [DeserializeAs(Name = "scope")]
        public string Scope { get; set; }
    }
}