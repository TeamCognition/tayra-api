using IdentityServer4.ResponseHandling;
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

            services.AddIdentityServerServices(Configuration);

            //.AddConfigurationStore(options =>
            //{
            //    options.ConfigureDbContext = builder => builder.UseSqlite(connectionString);
            //})
            //.AddOperationalStore(options =>
            //{
            //    options.ConfigureDbContext = builder => builder.UseSqlite(connectionString);
            //    options.EnableTokenCleanup = false;
            //}

            services.AddHttpContextAccessor();

            services.AddCors(c =>
            {
                c.AddPolicy("AllowAllOrigins", options => options.AllowAnyOrigin()
                                                                 .AllowAnyHeader());
            });
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