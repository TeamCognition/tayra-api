using System.Linq;
using Firdaws.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Tayra.API.Helpers;
using Tayra.Common;
using Tayra.Connectors.Atlassian.Jira;
using Tayra.Connectors.Common;
using Tayra.DAL;
using Tayra.Helpers;
using Tayra.Models.Catalog;
using Tayra.Models.Organizations;
using Tayra.Services;

namespace Tayra.API
{
    public class Startup
    {
        public Startup(IConfiguration config)
        {
            Configuration = config;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //register DBs
            services.AddDbContext<CatalogDbContext>(options => options.UseSqlServer(ConnectionStringUtilities.GetCatalogDbConnStr(Configuration)));
            services.AddDbContext<OrganizationDbContext>(options => { });
            
            services.AddAuthentication("Bearer")
               .AddJwtBearer("Bearer", options =>
               {
                   options.Authority = Configuration["Auth:authority"];
                   options.RequireHttpsMetadata = false;

                   options.Audience = "tAPI";
               });

            //Add Application services
            services.AddTransient<ILogsService, LogsService>();
            services.AddTransient<IBlobsService, BlobsService>();
            services.AddTransient<IShopsService, ShopsService>();
            services.AddTransient<ITasksService, TasksService>();
            services.AddTransient<ITeamsService, TeamsService>();
            services.AddTransient<ITokensService, TokensService>();
            services.AddTransient<ILookupsService, LookupsService>();
            services.AddTransient<IReportsService, ReportsService>();
            services.AddTransient<IProfilesService, ProfilesService>();
            services.AddTransient<IProjectsService, ProjectsService>();
            services.AddTransient<IShopItemsService, ShopItemsService>();
            services.AddTransient<IChallengesService, ChallengesService>();
            services.AddTransient<IIdentitiesService, IdentitiesService>();
            services.AddTransient<IInventoriesService, InventoryService>();
            services.AddTransient<IClaimBundlesService, ClaimBundlesService>();
            services.AddTransient<ICompetitionsService, CompetitionsService>();
            services.AddTransient<IntegrationsService, IntegrationsService>();

            services.AddTransient<IOrganizationsService, Services.OrganizationsService>();

            services.AddSingleton<IShardMapProvider>(new ShardMapProvider(Configuration));
            services.AddScoped<ITenantProvider, ShardTenantProvider>();
            services.AddScoped<IClaimsPrincipalProvider<TayraPrincipal>, TayraPrincipalProvider>();
            
            services.AddHttpContextAccessor();
            services.AddTransient<IConnectorResolver, ConnectorResolver>();
            services.AddTransient<IOAuthConnector, AtlassianJiraConnector>();
            services.AddTransient<ILogger, DebugLogger>();

            // Adds a default in-memory implementation of IDistributedCache.
            services.AddDistributedMemoryCache();
            //services.AddSession();

            services.AddCors(c =>
            {
                c.AddPolicy("AllowOrigin", options => options.AllowAnyOrigin());
            });

            services.AddMvcCore()
                .AddAuthorization()
                .AddJsonFormatters()
                .AddApiExplorer(); //for swagger

            ConfigureSwagger(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IOrganizationsService orgService)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseMiddleware<ExceptionMiddleware>();
                app.UseHsts();
            }

            //app.UseSession(); probably not needed

            app.UseSwagger();

            app.UseCors(options => options.AllowAnyOrigin());

            app.UseHttpsRedirection();

            app.Use(async (context, next) =>
            {
                //if(context.Request.Path.Value.Contains("/ws"))
                if (string.IsNullOrWhiteSpace(context.Request.Headers["Authorization"]))
                {
                    if (context.Request.QueryString.HasValue)
                    {
                        var token = context.Request.QueryString.Value
                            .Split('&').FirstOrDefault(x => x.Contains("accessToken"))?.Split('=')[1];

                        if (!string.IsNullOrWhiteSpace(token))
                        {
                            context.Request.Headers.Add("Authorization", new[] { $"Bearer {token}" });
                        }
                    }
                }
                await next.Invoke();
            });


            app.UseAuthentication();

            app.UseMvc();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Tayra API V1");
                c.RoutePrefix = string.Empty;
            });

            orgService.EnsureOrganizationsAreCreatedAndMigrated();
        }

        #region Private Methods

        private void ConfigureSwagger(IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Tayra API", Version = "v1" });
                c.DescribeAllEnumsAsStrings();

                c.CustomSchemaIds(type =>
                {
                    if (type.IsNested)
                    {
                        return type.FullName.Substring(type.FullName.LastIndexOf('.') + 1).Replace('+', '.');
                    }
                    if (type.IsGenericType)
                    {
                        return string.Format("{0}<{1}>",
                            type.Name.Substring(0, type.Name.Length - 2),
                            string.Join(',', type.GenericTypeArguments.Select(x => x.Name)));
                    }
                    return type.Name;
                });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description =
                        "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                });
                //c.AddSecurityRequirement(new OpenApiSecurityRequirement
                //{
                //    {
                //        new OpenApiSecurityScheme
                //        {
                //            Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "oauth2" }
                //        },
                //        new[] { "readAccess", "writeAccess" }
                //    }
                //});
            });
        }

        #endregion
    }
}
