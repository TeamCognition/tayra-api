using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Tayra.Models.Organizations;
using Tayra.Services;

namespace Tayra.Auth
{
    public static class ServiceCollectionExtensions
    {
        public static void AddIdentityServerServices(this IServiceCollection services, IConfiguration configuration)
        {
            var identityServer = services.AddIdentityServer()
                .AddInMemoryPersistedGrants()
                .AddInMemoryIdentityResources(Config.GetIdentityResources())
                .AddInMemoryApiResources(Config.GetApis())
                .AddInMemoryClients(Config.GetClients())
                .AddProfileService<ProfileService>()
                .AddResourceOwnerValidator<ResourceOwnerValidator>()
                .AddCustomTokenRequestValidator<CustomTokenRequestValidator>();

            services.AddTransient<IProfilesService, ProfilesService>();
            services.AddTransient<ITokensService, TokensService>();
            services.AddTransient<ILogsService, LogsService>();
            services.AddTransient<IIdentitiesService, IdentitiesService>();
            services.AddSingleton<IShardMapProvider>(new ShardMapProvider(configuration));

            //if (Environment.IsDevelopment())
            //{
                identityServer.AddDeveloperSigningCredential();
            //}
            //else
            //{
                //identityServer.AddDeveloperSigningCredential();
                //throw new Exception("need to configure key material");
            //}
        }
    }
}