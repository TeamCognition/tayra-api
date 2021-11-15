using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using Cog.Core;
using Microsoft.IdentityModel.Tokens;

namespace Tayra.Connectors.GitHub
{
    public static class Utils
    {
        public static string CreateGithubAppJwt(string githubAppId, string rsaPrivateKey)
        {
            var privateKey = rsaPrivateKey.ToByteArray();

            using RSA rsa = RSA.Create();
            rsa.ImportRSAPrivateKey(privateKey, out _);

            var signingCredentials = new SigningCredentials(new RsaSecurityKey(rsa), SecurityAlgorithms.RsaSha256)
            {
                CryptoProviderFactory = new CryptoProviderFactory { CacheSignatureProviders = false }
            };

            // Reduced -30 seconds in order to prevent "Expiration time' claim ('exp') is too far in the future" issue
            var now = DateTime.Now.AddSeconds(-30);
            var unixTimeSeconds = new DateTimeOffset(now).ToUnixTimeSeconds();

            var jwt = new JwtSecurityToken(
                issuer: githubAppId,
                claims: new[] {
                    new Claim(JwtRegisteredClaimNames.Iat, unixTimeSeconds.ToString(), ClaimValueTypes.Integer64),
                },
                notBefore: now,
                expires: now.AddMinutes(10),
                signingCredentials: signingCredentials
            );

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }

        public static UserInstallationsResponse.Installation FindTayraAppInstallation(UserInstallationsResponse.Installation[] installations, string githubAppId)
        {
            installations = installations.Where(x => x.AppId == githubAppId)
                                         .ToArray();

            var newestInstalationDate = installations.Max(x => x.UpdatedAt);

            var installation = installations.FirstOrDefault(x => x.UpdatedAt == newestInstalationDate);

            if (installation == null)
            {
                throw new ApplicationException("Github app Installation not found");
            }

            return installation;
        }

        public static string GetInstallationOrganizationName(string userToken, long organizationId)
        {
            var orgs = GitHubService.GetOrganizations(userToken, organizationId)?.Data;

            var org = orgs.LastOrDefault(x => x.Id == organizationId);
            if (org == null)
            {
                throw new ApplicationException("Organization by id not found");
            }

            return org.Login;
        }
    }
}