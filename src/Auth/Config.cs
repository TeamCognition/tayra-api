using System.Collections.Generic;
using IdentityServer4;
using IdentityServer4.Models;

namespace Tayra.Auth
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
            };
        }

        public static IEnumerable<ApiResource> GetApis()
        {
            return new ApiResource[]
            {
                new ApiResource("tAPI", "Tayra API")
            };
        }

        public static IEnumerable<Client> GetClients()
        {
            return new[]
            {
                // Tayra Web Client
                new Client
                {
                    ClientId = "twc",
                    ClientName = "Tayra Web Client",

                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    ClientSecrets = { new Secret("9C17710A-89C0-4D91-A945-9AEC9F071EE1".Sha256()) },

                    UpdateAccessTokenClaimsOnRefresh = true,
                    AllowOfflineAccess = true,

                    AllowedScopes = new List<string>
                    { 
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "tAPI" }
                    }
            };
        }
    }
}
