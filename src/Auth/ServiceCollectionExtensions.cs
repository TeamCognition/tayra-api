using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using OpenIddict.Abstractions;
using Tayra.Auth.Data;
using Tayra.Models.Organizations;
using Tayra.Services;

namespace Tayra.Auth
{
    public static class ServiceCollectionExtensions
    {
        public static void AddTayraAuthServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<OpeniddictDbContext>(options =>
            {
                // Configure the context to use Microsoft SQL Server.
                options.UseSqlServer(configuration.GetConnectionString("OpeniddictDatabase"));

                // Register the entity sets needed by OpenIddict.
                // Note: use the generic overload if you need
                // to replace the default OpenIddict entities.
                options.UseOpenIddict();
            });

            // Configure Identity to use the same JWT claims as OpenIddict instead
            // of the legacy WS-Federation claims it uses by default (ClaimTypes),
            // which saves you from doing the mapping in your authorization controller.
            services.Configure<IdentityOptions>(options =>
            {
                options.ClaimsIdentity.UserNameClaimType = OpenIddictConstants.Claims.Name;
                options.ClaimsIdentity.UserIdClaimType = OpenIddictConstants.Claims.Subject;
                options.ClaimsIdentity.RoleClaimType = OpenIddictConstants.Claims.Role;
            });

            services.AddOpenIddict()
                
                // Register the OpenIddict core components.
                .AddCore(options =>
                {
                    // Configure OpenIddict to use the Entity Framework Core stores and models.
                    // Note: call ReplaceDefaultEntities() to replace the default OpenIddict entities.
                    options.UseEntityFrameworkCore()
                           .UseDbContext<OpeniddictDbContext>();
                })

                // Register the OpenIddict server components.
                .AddServer(options =>
                {
                    // Enable the authorization, logout, userinfo, and introspection endpoints.
                    options.SetTokenEndpointUris("/connect/token");

                    // Mark the "email", "profile" and "roles" scopes as supported scopes.
                    options.RegisterScopes(OpenIddictConstants.Scopes.OfflineAccess, OpenIddictConstants.Permissions.Scopes.Email, OpenIddictConstants.Permissions.Scopes.Profile, OpenIddictConstants.Permissions.Scopes.Roles);

                    // Note: the sample only uses the implicit flow but you can enable the other
                    // flows if you need to support code, password or client credentials.
                    options.AllowPasswordFlow();
                    options.AllowRefreshTokenFlow();
                    
                    // Accept anonymous clients (i.e clients that don't send a client_id).
                    options.AcceptAnonymousClients();

                    // Register the signing and encryption credentials.
                    options.AddDevelopmentEncryptionCertificate()
                        .AddDevelopmentSigningCertificate();

                    // Register the ASP.NET Core host and configure the ASP.NET Core-specific options.
                    options.UseAspNetCore()
                        .EnableTokenEndpointPassthrough();
                })

                // Register the OpenIddict validation components.
                .AddValidation(options =>
                {
                    // Import the configuration from the local OpenIddict server instance.
                    options.UseLocalServer();
                    
                    // Register the ASP.NET Core host.
                    options.UseAspNetCore();
                });
            
            services.AddHttpContextAccessor();
            services.AddTransient<IIdentitiesService, IdentitiesService>();
            services.AddTransient<ITokensService, TokensService>();
            services.AddTransient<ILogsService, LogsService>();
        }
    }
}