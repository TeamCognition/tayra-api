using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenIddict.Abstractions;
using Tayra.Auth.Data;

namespace Tayra.Auth
{
    public class OpenIdSeedWorker : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;
        public OpenIdSeedWorker(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using var scope = _serviceProvider.CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<OpeniddictDbContext>();
            await context.Database.EnsureCreatedAsync(cancellationToken);

            await CreateApplicationsAsync(scope, cancellationToken);
            await CreateScopesAsync(scope, cancellationToken);
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

        private static async Task CreateApplicationsAsync(IServiceScope scope, CancellationToken cancellationToken)
        {
            var manager = scope.ServiceProvider.GetRequiredService<IOpenIddictApplicationManager>();
            
            if (await manager.FindByClientIdAsync("postman", cancellationToken) is null)
            {
                await manager.CreateAsync(new OpenIddictApplicationDescriptor
                {
                    ClientId = "postman",
                    ClientSecret = "postman-secret",
                    DisplayName = "Postman",
                    RedirectUris = { new Uri("https://oauth.pstmn.io/v1/callback") },
                    Permissions =
                    {
                        OpenIddictConstants.Permissions.Endpoints.Authorization,
                        OpenIddictConstants.Permissions.Endpoints.Token,
                        
                        OpenIddictConstants.Permissions.GrantTypes.AuthorizationCode,
                        OpenIddictConstants.Permissions.GrantTypes.ClientCredentials,
                        OpenIddictConstants.Permissions.GrantTypes.RefreshToken,

                        OpenIddictConstants.Permissions.Prefixes.Scope + "api",
                        OpenIddictConstants.Permissions.ResponseTypes.Code
                    }
                }, cancellationToken);
            }
            
            if (await manager.FindByClientIdAsync("web", cancellationToken) is null)
            {
                await manager.CreateAsync(new OpenIddictApplicationDescriptor
                {
                    ClientId = "web",
                    ClientSecret = "web-secret",
                    DisplayName = "Web Client",
                    Permissions =
                    {
                        OpenIddictConstants.Permissions.Endpoints.Authorization,
                        OpenIddictConstants.Permissions.Endpoints.Token,
                        
                        OpenIddictConstants.Permissions.GrantTypes.Password,
                        OpenIddictConstants.Permissions.GrantTypes.RefreshToken,
                        OpenIddictConstants.Permissions.GrantTypes.AuthorizationCode,
                        
                        OpenIddictConstants.Permissions.Prefixes.Scope + "api",
                        OpenIddictConstants.Permissions.Prefixes.Scope + "email"
                    }
                }, cancellationToken);
            }
        }
        
        private static async Task CreateScopesAsync(IServiceScope scope, CancellationToken cancellationToken)
        {
            var manager = scope.ServiceProvider.GetRequiredService<IOpenIddictScopeManager>();
            
            if (await manager.FindByNameAsync("api", cancellationToken) == null)
            {
                var descriptor = new OpenIddictScopeDescriptor
                {
                    Name = "api",
                    Resources =
                    {
                        "resource_server-api"
                    }
                };
        
                await manager.CreateAsync(descriptor, cancellationToken);
            }
        }
    }
}