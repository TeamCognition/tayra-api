using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using OpenIddict.Abstractions;
using Tayra.Auth.Data;
using Tayra.Models.Catalog;
using Tayra.Models.Organizations;
using Tayra.Services;

namespace Tayra.Auth
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<CatalogDbContext>(options => options.UseSqlServer(GetCatalogConnectionString()));
            services.AddDbContext<OrganizationDbContext>(options => { });
            services.AddDbContext<OpeniddictDbContext>(options =>
            {
                // Configure the context to use Microsoft SQL Server.
                options.UseSqlServer(Configuration.GetConnectionString("OpeniddictDatabase"));

                // Register the entity sets needed by OpenIddict.
                // Note: use the generic overload if you need
                // to replace the default OpenIddict entities.
                options.UseOpenIddict();
            });
            
            services.AddControllers();
            services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new OpenApiInfo {Title = "Auth", Version = "v1"}); });
            
            services.AddTayraAuthServices(Configuration);
            
            // services.AddHttpContextAccessor();
            // services.AddSingleton<IShardMapProvider>(new ShardMapProvider(Configuration));
            // services.AddScoped<ITenantProvider, ShardTenantProvider>();
            // services.AddTransient<IIdentitiesService, IdentitiesService>();
            // services.AddTransient<ITokensService, TokensService>();
            // services.AddTransient<ILogsService, LogsService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, OpeniddictDbContext dbContext)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Auth v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

            dbContext.Database.Migrate();
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
                $"Data Source=tcp:{catalogServer},1433;Database={catalogDatabase};User ID={databaseUser};Password={databasePassword};Encrypt=True;TrustServerCertificate=True;";
        }
    }
}