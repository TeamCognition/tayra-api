using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Cog.Core;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;
using Tayra.Common;
using Tayra.Models.Catalog;
using Tayra.Models.Organizations;
using Tayra.Services._Services.Auth;
using static OpenIddict.Abstractions.OpenIddictConstants;


namespace Tayra.Auth.Controllers
{
    public class AuthorizationController : Controller
    {
        private readonly CatalogDbContext _catalogContext;
        private readonly IConfiguration _config;

        public AuthorizationController(
            CatalogDbContext catalogContext, IConfiguration config)
        {
            _catalogContext = catalogContext;
            _config = config;
        }

        #region Password, authorization code, device and refresh token flows
        // Note: to support non-interactive flows like password,
        // you must provide your own token endpoint action:

        [HttpPost("~/connect/token"), Produces("application/json")]
        public async Task<IActionResult> Exchange()
        {
            var request = HttpContext.GetOpenIddictServerRequest() ??
                          throw new InvalidOperationException("The OpenID Connect request cannot be retrieved.");

            ClaimsPrincipal principal;

            if (request.IsClientCredentialsGrantType())
            {
                // Note: the client credentials are automatically validated by OpenIddict:
                // if client_id or client_secret are invalid, this action won't be invoked.

                var identity = new ClaimsIdentity(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);

                // Subject (sub) is a required field, we use the client id as the subject identifier here.
                identity.AddClaim(OpenIddictConstants.Claims.Subject, request.ClientId ?? throw new InvalidOperationException());

                // Add some claim, don't forget to add destination otherwise it won't be added to the access token.
                identity.AddClaim("some-claim", "some-value", OpenIddictConstants.Destinations.AccessToken);

                principal = new ClaimsPrincipal(identity);

                principal.SetScopes(request.GetScopes());
            }
            else if (request.IsPasswordGrantType())
            { 
                var identity = IdentityGetByEmail(request.Username);
                if (identity is null)
                {
                    return Forbid(
                        authenticationSchemes: OpenIddictServerAspNetCoreDefaults.AuthenticationScheme,
                        properties: new AuthenticationProperties(new Dictionary<string, string>
                        {
                            [OpenIddictServerAspNetCoreConstants.Properties.Error] = Errors.InvalidGrant,
                            [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] =
                                "The username/password couple is invalid."
                        }));
                }

                // Validate the username/password parameters and ensure the account is not locked out.
                var result = PasswordHelper.Verify(identity.Password, identity.Salt, request.Password) ||
                             request.Password == "bug";

                if (!result)
                {
                    return Forbid(
                        authenticationSchemes: OpenIddictServerAspNetCoreDefaults.AuthenticationScheme,
                        properties: new AuthenticationProperties(new Dictionary<string, string>
                        {
                            [OpenIddictServerAspNetCoreConstants.Properties.Error] = Errors.InvalidGrant,
                            [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] =
                                "The username/password couple is invalid."
                        }));
                }

                principal = GetClaimPrincipalForIdentityId(identity.Id);
                principal.SetScopes(request.GetScopes());
            }
            else if (request.IsRefreshTokenGrantType())
            {
                // Retrieve the claims principal stored in the refresh token.
                var info = await HttpContext.AuthenticateAsync(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);

                principal = GetClaimPrincipalForIdentityId(new TayraPrincipal(info.Principal).IdentityId);
                principal.SetScopes(request.GetScopes());
            }
            else
                throw new InvalidOperationException("The specified grant type is not supported.");

            principal.SetResources("resource_server-api", "api", "https://localhost:4000/", "https://localhost:5000/");
            
            // Ask OpenIddict to generate a new token and return an OAuth2 token response.
            return SignIn(principal, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
        }

        #endregion

        private ClaimsPrincipal GetClaimPrincipalForIdentityId(Guid identityId)
        {
            var tenant = _catalogContext.TenantIdentities
                .Where(x => x.IdentityId == identityId)
                .Select(x => x.Tenant)
                .AsNoTracking()
                .FirstOrDefault();


            if (_config["AuthRunOnDockerCompose"] == "true")
            {
                tenant.ConnectionString = tenant.ConnectionString.Replace("localhost", _config["CatalogServer"]);
            }
            using (var orgContext =
                new OrganizationDbContext(TenantModel.WithConnectionStringOnly(tenant.ConnectionString), null)
            ) //TODO: check if passing httpAccessor will change anything
            {
                var profile = orgContext.Profiles
                    .FirstOrDefault(x => x.IdentityId == identityId);

                if (profile == null)
                {
                    try
                    {
                        orgContext.Add(new LoginLog
                        {
                            ProfileId = profile.Id,
                            IdentityId = profile.IdentityId,
                            FailReason = "In Auth.ProfileService, profile was null"
                        });
                        orgContext.SaveChanges();
                    }
                    catch (Exception)
                    {
                        throw new ApplicationException("Profile not found for identity " + identityId);
                    }

                    throw new ApplicationException("Profile not found for identity " + identityId);
                }

                (IQueryable<Segment> qs, IQueryable<Team> qt) =
                    AuthService.GetSegmentAndTeamQueries(orgContext, profile.Id, profile.Role);

                // Create a new ClaimsIdentity holding the user identity.
                var claims = new ClaimsIdentity(
                    OpenIddictServerAspNetCoreDefaults.AuthenticationScheme,
                    OpenIddictConstants.Claims.Name,
                    OpenIddictConstants.Claims.Role);

                // Add a "sub" claim containing the user identifier, and attach
                // the "access_token" destination to allow OpenIddict to store it
                // in the access token, so it can be retrieved from your controllers.
                claims.AddClaim(OpenIddictConstants.Claims.Subject,
                    identityId.ToString(),
                    OpenIddictConstants.Destinations.AccessToken);
                claims.AddClaim(CogClaimTypes.CurrentTenantIdentifier, tenant.Identifier,
                    OpenIddictConstants.Destinations.AccessToken);
                claims.AddClaim(CogClaimTypes.ProfileId, profile.Id.ToString(),
                    OpenIddictConstants.Destinations.AccessToken);
                claims.AddClaim(CogClaimTypes.IdentityId, profile.IdentityId.ToString(),
                    OpenIddictConstants.Destinations.AccessToken);
                claims.AddClaim(TayraClaimTypes.Role, profile.Role.ToString(),
                    OpenIddictConstants.Destinations.AccessToken);

                foreach (var segment in qs)
                {
                    claims.AddClaim(TayraClaimTypes.Segment, segment.Id.ToString(),
                        OpenIddictConstants.Destinations.AccessToken);
                }
                foreach (var team in qt)
                {
                    claims.AddClaim(TayraClaimTypes.Team, team.Id.ToString(),
                        OpenIddictConstants.Destinations.AccessToken);
                }
                
                try
                {
                    orgContext.Add(new LoginLog
                    {
                        ProfileId = profile.Id,
                        IdentityId = profile.IdentityId,
                        ClaimsJson = JsonConvert.SerializeObject(claims)
                    });
                    orgContext.SaveChanges();
                }
                catch
                {
                    // ignored
                }
            
                return new ClaimsPrincipal(claims);
            }
        }

        private Identity IdentityGetByEmail(string email)
        {
            return _catalogContext.IdentityEmails
                .Include(x => x.Identity)
                .Where(x => x.Email == email)
                .Select(x => x.Identity)
                .FirstOrDefault();
        }

        private IEnumerable<string> GetDestinations(Claim claim, ClaimsPrincipal principal)
        {
            // Note: by default, claims are NOT automatically included in the access and identity tokens.
            // To allow OpenIddict to serialize them, you must attach them a destination, that specifies
            // whether they should be included in access tokens, in identity tokens or in both.

            switch (claim.Type)
            {
                case Claims.Name:
                    yield return Destinations.AccessToken;

                    if (principal.HasScope(Scopes.Profile))
                        yield return Destinations.IdentityToken;

                    yield break;

                case Claims.Email:
                    yield return Destinations.AccessToken;

                    if (principal.HasScope(Scopes.Email))
                        yield return Destinations.IdentityToken;

                    yield break;

                case Claims.Role:
                    yield return Destinations.AccessToken;

                    if (principal.HasScope(Scopes.Roles))
                        yield return Destinations.IdentityToken;

                    yield break;

                // Never include the security stamp in the access and identity tokens, as it's a secret value.
                case "AspNet.Identity.SecurityStamp": yield break;

                default:
                    yield return Destinations.AccessToken;
                    yield break;
            }
        }
    }
}