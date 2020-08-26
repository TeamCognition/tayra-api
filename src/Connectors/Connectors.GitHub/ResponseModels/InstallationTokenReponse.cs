using RestSharp.Deserializers;

namespace Tayra.Connectors.GitHub
{
    public class InstallationTokenResponse
    {
        [DeserializeAs(Name = "token")]
        public string AccessToken { get; set; }

        [DeserializeAs(Name = "expires_in")]
        public string ExpiresIn { get; set; }
    }
}