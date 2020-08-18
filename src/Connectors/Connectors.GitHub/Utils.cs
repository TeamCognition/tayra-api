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

            var now = DateTime.Now;
            var unixTimeSeconds = new DateTimeOffset(now).ToUnixTimeSeconds();

            var jwt = new JwtSecurityToken(
                issuer: githubAppId,
                claims: new [] {
                    new Claim(JwtRegisteredClaimNames.Iat, unixTimeSeconds.ToString(), ClaimValueTypes.Integer64),
                },
                notBefore: now,
                expires: now.AddMinutes(10),
                signingCredentials: signingCredentials
            );

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }
        
        public static string FindTayraAppInstallation(UserInstallationsResponse.Installation[] installations, string githubAppId)
        {
            var installation = installations.FirstOrDefault(x => x.AppId == githubAppId);

            if (installation == null)
            {
                throw new ApplicationException("Github app Installation not found");
            }
            
            return installation.Id;
        }
    }
}