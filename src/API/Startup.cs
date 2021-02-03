using System;
using System.IO;
using System.Linq;
using AutoMapper;
using Cog.Core;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using OpenIddict.Validation.AspNetCore;
using SixLabors.ImageSharp.Web.DependencyInjection;
using Tayra.API.Helpers;
using Tayra.Common;
using Tayra.Connectors.Atlassian.Jira;
using Tayra.Connectors.Common;
using Tayra.Connectors.GitHub;
using Tayra.Connectors.Slack;
using Tayra.DAL;
using Tayra.Helpers;
using Tayra.Imager;
using Tayra.Mailer;
using Tayra.Mailer.Contracts;
using Tayra.Models.Catalog;
using Tayra.Models.Organizations;
using Tayra.Services;
using Tayra.Services.Contracts;
using Tayra.Services.webhooks;

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
            services.AddDbContext<CatalogDbContext>(options => options.UseSqlServer(ConnectionStringUtilities.GetCatalogDbConnStr(Configuration)));
            services.AddMultiTenant<Tenant>()
                .WithEFCoreStore<CatalogDbContext, Tenant>()
                //.WithStrategy<TenantStrategy>(ServiceLifetime.Singleton)
                .WithDelegateStrategy(context =>
                {
                    if (!(context is Microsoft.AspNetCore.Http.HttpContext httpContext))
                        return null;

                    var tenantIdentifierFromAccessToken = new CogPrincipal(httpContext?.User)?.CurrentTenantIdentifier;
                    if (tenantIdentifierFromAccessToken is not null)
                    {
                        return System.Threading.Tasks.Task.FromResult(
                            tenantIdentifierFromAccessToken);
                    }

                    httpContext.Request.Query.TryGetValue("tenant", out var identifier);

                    return System.Threading.Tasks.Task.FromResult(identifier.FirstOrDefault());
                });
            
            services.AddDbContext<OrganizationDbContext>();

            services.AddAutoMapper(typeof(Startup));

            services.AddMediatR(typeof(Startup));
            
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme;
            });
            
            // Register the OpenIddict validation components.
            services.AddOpenIddict()
                .AddValidation(options =>
                {
                    options.SetIssuer(Configuration["AuthIssuer"]);
                    options.AddAudiences("resource_server-api");
                    
                    // Register the System.Net.Http integration.
                    options.UseSystemNetHttp();

                    // Register the ASP.NET Core host.
                    options.UseAspNetCore();
                });
            
            //Add Application services
            services.AddTransient<ILogsService, LogsService>();
            services.AddTransient<IBlobsService, BlobsService>();
            services.AddTransient<IItemsService, ItemsService>();
            services.AddTransient<IShopsService, ShopsService>();
            services.AddTransient<ITasksService, TasksService>();
            services.AddTransient<IPraiseService, PraiseService>();
            services.AddTransient<IQuestsService, QuestsService>();
            services.AddTransient<ITokensService, TokensService>();
            services.AddTransient<ILookupsService, LookupsService>();
            services.AddTransient<IReportsService, ReportsService>();
            services.AddTransient<ISegmentsService, SegmentsService>();
            services.AddTransient<IShopItemsService, ShopItemsService>();
            services.AddTransient<IAssistantService, AssistantService>();
            services.AddTransient<IIdentitiesService, IdentitiesService>();
            services.AddTransient<IInventoriesService, InventoryService>();
            services.AddTransient<IClaimBundlesService, ClaimBundlesService>();
            services.AddTransient<IIntegrationsService, IntegrationsService>();
            services.AddTransient<IGithubWebhookService, GithubWebhookServiceService>();
            services.AddTransient<IMailerService, MailerService>();

            services.AddScoped<IClaimsPrincipalProvider<TayraPrincipal>, TayraPrincipalProvider>();

            services.AddHttpContextAccessor();
            services.AddTransient<IConnectorResolver, ConnectorResolver>();
            services.AddTransient<IOAuthConnector, AtlassianJiraConnector>();
            services.AddTransient<IOAuthConnector, GitHubConnector>();
            services.AddTransient<IOAuthConnector, SlackConnector>();
            services.AddTransient<ILogger, DebugLogger>();

            // Adds a default in-memory implementation of IDistributedCache.
            services.AddDistributedMemoryCache();
            //services.AddSession();

            services.AddCors();

            services.AddControllers();
            
            services.AddMvcCore()
                .AddNewtonsoftJson()
                .AddApiExplorer(); //for swagger
            
            services.AddImagerServices(Configuration);
            ConfigureSwagger(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, CatalogDbContext catalogDbContext)
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
            
            //Tayra.Imager
            string FilePath = "wwwroot";
            Directory.CreateDirectory(FilePath);
            app.UseImageSharp();
            
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseMultiTenant();
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            
            Console.WriteLine("GITHUB");
            Console.WriteLine(Configuration["Connectors:Github:AppName"]);
            
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Tayra API V1");
                c.RoutePrefix = string.Empty;
            });
            
            // catalogDbContext.TenantInfo.AsNoTracking().ToArray().ForEach(x => OrganizationDbContext.DatabaseEnsureCreatedAndMigrated(x.ConnectionString));
        }

        #region Private Methods

        private static void ConfigureSwagger(IServiceCollection services)
        {
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
                    return type.FullName;
                });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description =
                        "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });
            });
        }

        #endregion
    }
}
