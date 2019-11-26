using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Tayra.Models.Catalog;
using Tayra.Models.Organizations;
using Tayra.Services;

namespace Tayra.Auth
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public IHostingEnvironment Environment { get; }

        public Startup(IConfiguration config, IHostingEnvironment env)
        {
            Configuration = config;
            Environment = env;
        }


        public void ConfigureServices(IServiceCollection services)
        {
            var connectionString = Configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<CatalogDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("catalog")));

            var identityServer = services.AddIdentityServer()
                    .AddInMemoryPersistedGrants()
                    .AddInMemoryIdentityResources(Config.GetIdentityResources())
                    .AddInMemoryApiResources(Config.GetApis())
                    .AddInMemoryClients(Config.GetClients())
                    .AddProfileService<ProfileService>()
                    .AddResourceOwnerValidator<ResourceOwnerValidator>();


            //.AddConfigurationStore(options =>
            //{
            //    options.ConfigureDbContext = builder => builder.UseSqlite(connectionString);
            //})
            //.AddOperationalStore(options =>
            //{
            //    options.ConfigureDbContext = builder => builder.UseSqlite(connectionString);
            //    options.EnableTokenCleanup = false;
            //}

            services.AddTransient<IProfilesService, ProfilesService>();
            services.AddTransient<ITokensService, TokensService>();
            services.AddTransient<ILogsService, LogsService>();
            services.AddTransient<IIdentitiesService, IdentitiesService>();

            services.AddCors(c =>
            {
                c.AddPolicy("AllowOrigin", options => options.AllowAnyOrigin());
            });

            if (Environment.IsDevelopment())
            {
                identityServer.AddDeveloperSigningCredential();
            }
            else
            {
                identityServer.AddDeveloperSigningCredential();
                //throw new Exception("need to configure key material");
            }

        }

        public void Configure(IApplicationBuilder app)
        {
            if (Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(options => options.AllowAnyOrigin());

            app.UseIdentityServer();
        }
    }
}