﻿using System;
using System.Data.SqlClient;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.SqlDatabase.ElasticScale.ShardManagement;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Tayra.API.Helpers;
using Tayra.Connectors.Atlassian.Jira;
using Tayra.Connectors.Common;
using Tayra.Helpers;
using Tayra.Models.Catalog;
using Tayra.Models.Organizations;
using Tayra.Services;

namespace Tayra.API
{
    public class Startup
    {
        public Startup(IConfiguration config, IHostingEnvironment env)
        {
            Configuration = config;

            //read config settigs from appsettings.json
            ReadAppConfig();
        }

        #region Private fields

        private ICatalogRepository _catalogRepository;

        #endregion


        public static DatabaseConfig DatabaseConfig { get; set; }
        public static CatalogConfig CatalogConfig { get; set; }
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddDbContext<CatalogDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("core-dev")));

            //services.AddDbContext<OrganizationDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("org-dev")));
            //services.AddDbContext<OrganizationDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("org-local")));

            services.AddAuthentication("Bearer")
               .AddJwtBearer("Bearer", options =>
               {
                   options.Authority = Configuration["Auth:authority"];
                   options.RequireHttpsMetadata = false;

                   options.Audience = "tAPI";
               });

            services.AddTransient<ILogsService, LogsService>();
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
            services.AddTransient<IIntegrationsService, IntegrationsService>();

            services.AddTransient<IOrganizationsService, Services.OrganizationsService>();


            services.AddTransient<IConnectorResolver, ConnectorResolver>();
            services.AddTransient<IOAuthConnector, AtlassianJiraConnector>();
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<ILogger, DebugLogger>();

            // Adds a default in-memory implementation of IDistributedCache.
            services.AddDistributedMemoryCache();
            services.AddSession();

            //register catalog DB
            services.AddDbContext<CatalogDbContext>(options => options.UseSqlServer(GetCatalogConnectionString(CatalogConfig, DatabaseConfig)));
            services.AddDbContext<OrganizationDbContext>(options => { });

            //Add Application services
            services.AddTransient<ICatalogRepository, CatalogRepository>();
            services.AddScoped<ITenantProvider, ShardTenantProvider>();

            //create instance of utilities class
            var provider = services.BuildServiceProvider();
            _catalogRepository = provider.GetService<ICatalogRepository>();

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
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
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
        }

        #region Private Methods

        /// <summary>
        ///  Gets the catalog connection string using the app settings
        /// </summary>
        /// <param name="catalogConfig">The catalog configuration.</param>
        /// <param name="databaseConfig">The database configuration.</param>
        /// <returns></returns>
        private string GetCatalogConnectionString(CatalogConfig catalogConfig, DatabaseConfig databaseConfig)
        {
            return
                $"Server=tcp:{catalogConfig.CatalogServer},1433;Database={catalogConfig.CatalogDatabase};User ID={databaseConfig.DatabaseUser};Password={databaseConfig.DatabasePassword};Trusted_Connection=False;Encrypt=True;";
                //$"Server=tcp:sqlserver-tayra.database.windows.net,1433;Database=sqldb-tayra-tenants_free-prod;User ID={databaseConfig.DatabaseUser};Password={databaseConfig.DatabasePassword};Trusted_Connection=False;Encrypt=True;";
        }

        /// <summary>
        /// Reads the application settings from appsettings.json
        /// </summary>
        private void ReadAppConfig()
        {
            DatabaseConfig = new DatabaseConfig
            {
                DatabasePassword = Configuration["DatabasePassword"],
                DatabaseUser = Configuration["DatabaseUser"],
                DatabaseServerPort = Convert.ToInt32(Configuration["DatabaseServerPort"]),
                SqlProtocol = SqlProtocol.Tcp,
                ConnectionTimeOut = Convert.ToInt32(Configuration["ConnectionTimeOut"]),
            };

            CatalogConfig = new CatalogConfig
            {
                ServicePlan = Configuration["ServicePlan"],
                CatalogDatabase = Configuration["CatalogDatabase"],
                CatalogServer = Configuration["CatalogServer"] + ".database.windows.net"
            };
        }

        /// <summary>
        /// Gets the basic SQL connection string.
        /// </summary>
        /// <returns></returns>
        private string GetBasicSqlConnectionString()
        {
            var connStrBldr = new SqlConnectionStringBuilder
            {
                UserID = DatabaseConfig.DatabaseUser,
                Password = DatabaseConfig.DatabasePassword,
                ApplicationName = "Tayra",
                ConnectTimeout = DatabaseConfig.ConnectionTimeOut
            };

            return connStrBldr.ConnectionString;
        }

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
