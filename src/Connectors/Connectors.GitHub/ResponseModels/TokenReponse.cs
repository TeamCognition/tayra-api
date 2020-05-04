using System;
using Firdaws.Core;
using RestSharp.Deserializers;

namespace Tayra.Connectors.GitHub
{
    public class TokenResponse
    {
        [DeserializeAs(Name = "access_token")]
        public string AccessToken { get; set; }

        [DeserializeAs(Name = "token_type")]
        public string TokenType { get; set; }

        [DeserializeAs(Name = "refresh_token")]
        public string RefreshToken { get; set; }

        [DeserializeAs(Name = "scope")]
        public string Scope { get; set; }

        [DeserializeAs(Name = "expires_in")]
        public string ExpiresIn { get; set; }

        public string ExpirationDate
        {
            get
            {
                if (long.TryParse(ExpiresIn, out var seconds))
                {
                    return DateTime.UtcNow.AddSeconds(seconds).ToString(DateHelper2.DATE_TIME_FORMAT);
                }
                return null;
            }
        }
    }
}