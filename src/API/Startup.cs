using System.IO;
using System.Linq;
using Cog.Core;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using SixLabors.ImageSharp.Web.DependencyInjection;
using Tayra.API.Helpers;
using Tayra.Auth;
using Tayra.Common;
using Tayra.Connectors.Atlassian.Jira;
using Tayra.Connectors.Common;
using Tayra.Connectors.GitHub;
using Tayra.DAL;
using Tayra.Helpers;
using Tayra.Imager;
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
            
            //Add Application services
            services.AddTransient<ILogsService, LogsService>();
            services.AddTransient<IBlobsService, BlobsService>();
            services.AddTransient<IItemsService, ItemsService>();
            services.AddTransient<IShopsService, ShopsService>();
            services.AddTransient<ITasksService, TasksService>();
            services.AddTransient<ITeamsService, TeamsService>();
            services.AddTransient<IPraiseService, PraiseService>();
            services.AddTransient<IQuestsService, QuestsService>();
            services.AddTransient<ITokensService, TokensService>();
            services.AddTransient<ILookupsService, LookupsService>();
            services.AddTransient<IReportsService, ReportsService>();
            services.AddTransient<IProfilesService, ProfilesService>();
            services.AddTransient<ISegmentsService, SegmentsService>();
            services.AddTransient<IShopItemsService, ShopItemsService>();
            services.AddTransient<IAnalyticsService, AnalyticsService>();
            services.AddTransient<IAssistantService, AssistantService>();
            services.AddTransient<IIdentitiesService, IdentitiesService>();
            services.AddTransient<IInventoriesService, InventoryService>();
            services.AddTransient<IClaimBundlesService, ClaimBundlesService>();
            services.AddTransient<ICompetitionsService, CompetitionsService>();
            services.AddTransient<IIntegrationsService, IntegrationsService>();

            services.AddTransient<IOrganizationsService, Services.OrganizationsService>();
            
            services.AddSingleton<IShardMapProvider>(new ShardMapProvider(Configuration));
            services.AddScoped<ITenantProvider, ShardTenantProvider>();
            services.AddScoped<IClaimsPrincipalProvider<TayraPrincipal>, TayraPrincipalProvider>();
            
            services.AddHttpContextAccessor();
            services.AddTransient<IConnectorResolver, ConnectorResolver>();
            services.AddTransient<IOAuthConnector, AtlassianJiraConnector>();
            services.AddTransient<IOAuthConnector, GitHubConnector>();
            services.AddTransient<ILogger, DebugLogger>();

            // Adds a default in-memory implementation of IDistributedCache.
            services.AddDistributedMemoryCache();
            //services.AddSession();

            services.AddCors();

            services.AddControllers();
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer("Bearer", options =>
            {
                options.Authority = Configuration.GetValue<string>("Auth:authority");
                options.RequireHttpsMetadata = false;

                options.Audience = "tAPI";
            });

            services.AddMvcCore()
                .AddNewtonsoftJson()
                .AddApiExplorer(); //for swagger

            services.AddIdentityServerServices(Configuration);
            services.AddImagerServices(Configuration);
            ConfigureSwagger(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IOrganizationsService orgService)
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

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseCors(builder => builder
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());   

            app.UseSwagger();

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

            //Tayra.Auth
            app.UseIdentityServer();

            //Tayra.Imager
            string FilePath = "wwwroot";
            Directory.CreateDirectory(FilePath);
            app.UseImageSharp();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Tayra API V1");
                c.RoutePrefix = string.Empty;
            });

            orgService.EnsureOrganizationsAreCreatedAndMigrated();
        }

        #region Private Methods

        private static void ConfigureSwagger(IServiceCollection services)
        {
            services.AddSwaggerGenNewtonsoftSupport();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Tayra API", Version = "v1" });

                c.CustomSchemaIds(type =>
                {
                    if (type.IsNested)
                    {
                        return type.FullName?.Substring(type.FullName.LastIndexOf('.') + 1).Replace('+', '.');
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
