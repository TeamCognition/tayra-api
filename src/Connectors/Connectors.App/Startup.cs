﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Tayra.Connectors.App.Helpers;
using Tayra.Connectors.Atlassian.Jira;
using Tayra.Connectors.Common;
using Tayra.Connectors.GitHub;
using Tayra.Connectors.Slack;
using Tayra.DAL;
using Tayra.Models.Catalog;
using Tayra.Models.Organizations;

namespace Tayra.Connectors.App
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<ILogger, Models.DebugLogger>();

            services.AddDbContext<CatalogDbContext>(options => options.UseSqlServer(ConnectionStringUtilities.GetCatalogDbConnStr(Configuration)));
            services.AddDbContext<OrganizationDbContext>(
                options => options.UseSqlServer(Configuration.GetConnectionString("org-mop")));
            services.AddDbContext<ATJiraDataContext>(
                options => options.UseSqlServer(Configuration.GetConnectionString("org-mop")));

            services.AddTransient<IConnectorResolver, ConnectorResolver>();
            services.AddTransient<IOAuthConnector, AtlassianJiraConnector>();
            services.AddTransient<IOAuthConnector, GitHubConnector>();
            services.AddTransient<IOAuthConnector, SlackConnector>();
            services.AddSingleton<IShardMapProvider>(new ShardMapProvider(Configuration));
            services.AddScoped<ITenantProvider, FakeTenantProvider>();

            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => false;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddMvc().AddMvcOptions(x =>
            {
                x.EnableEndpointRouting = false;
            });
            services.Configure<RouteOptions>(op => op.LowercaseUrls = true);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                // app.UseHsts();
            }

            //app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
