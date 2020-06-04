﻿using IdentityServer4.ResponseHandling;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Tayra.Models.Catalog;
using Microsoft.Extensions.Hosting;
using Tayra.Models.Organizations;
using Tayra.Services;

namespace Tayra.Auth
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Environment { get; }

        public Startup(IConfiguration config, IWebHostEnvironment env)
        {
            Configuration = config;
            Environment = env;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<CatalogDbContext>(options => options.UseSqlServer(GetCatalogConnectionString()));
            services.AddDbContext<OrganizationDbContext>(options => { });

            var identityServer = services.AddIdentityServer()
                    .AddInMemoryPersistedGrants()
                    .AddInMemoryIdentityResources(Config.GetIdentityResources())
                    .AddInMemoryApiResources(Config.GetApis())
                    .AddInMemoryClients(Config.GetClients())
                    .AddProfileService<ProfileService>()
                    .AddResourceOwnerValidator<ResourceOwnerValidator>()
                    .AddCustomTokenRequestValidator<CustomTokenRequestValidator>();


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
            services.AddSingleton<IShardMapProvider>(new ShardMapProvider(Configuration));

            services.AddHttpContextAccessor();

            services.AddCors(c =>
            {
                c.AddPolicy("AllowAllOrigins", options => options.AllowAnyOrigin()
                                                                 .AllowAnyHeader());
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

            app.UseCors("AllowAllOrigins");

            app.UseIdentityServer();
        }

        /// <summary>
        ///  Gets the catalog connection string using the app settings
        /// </summary>
        private string GetCatalogConnectionString()
        {
            var databasePassword = Configuration["DatabasePassword"];
            var databaseUser = Configuration["DatabaseUser"];
            var catalogDatabase = Configuration["CatalogDatabase"];
            var catalogServer = Configuration["CatalogServer"];

            return
                $"Server=tcp:{catalogServer},1433;Database={catalogDatabase};User ID={databaseUser};Password={databasePassword};Trusted_Connection=False;Encrypt=True;";
        }
    }
}