using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Web;
using SixLabors.ImageSharp.Web.Caching;
using SixLabors.ImageSharp.Web.Commands;
using SixLabors.ImageSharp.Web.DependencyInjection;
using SixLabors.ImageSharp.Web.Middleware;
using SixLabors.ImageSharp.Web.Processors;
using SixLabors.ImageSharp.Web.Providers;
using SixLabors.Memory;
using IHostingEnv = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace Imager
{
    public class Startup
    {
        public Startup(IConfiguration configuration) => this.AppConfiguration = configuration;

        public IConfiguration AppConfiguration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddImageSharpCore()
                .SetRequestParser<QueryCollectionRequestParser>()
                .Configure<PhysicalFileSystemCacheOptions>(_ => { })
                .SetCache<PhysicalFileSystemCache>()
                .SetCacheHash<CacheHash>()
                .Configure<AzureBlobStorageImageProviderOptions>(options =>
                {
                    options.ConnectionString = AppConfiguration["BlobStorageConnectionStr"];
                    options.ContainerName = AppConfiguration["BlobContainerName"];
                })
                .AddProvider<AzureBlobStorageImageProvider>()
                .AddProcessor<ResizeWebProcessor>();

            // Add the default service and options.
            //
            // services.AddImageSharp();

            // Or add the default service and custom options.
            //
            // this.ConfigureDefaultServicesAndCustomOptions(services);

            // Or we can fine-grain control adding the default options and configure all other services.
            //
            // this.ConfigureCustomServicesAndDefaultOptions(services);

            // Or we can fine-grain control adding custom options and configure all other services
            // There are also factory methods for each builder that will allow building from configuration files.
            //
            // this.ConfigureCustomServicesAndCustomOptions(services);

            services.AddControllers();
        }

        private void ConfigureDefaultServicesAndCustomOptions(IServiceCollection services)
        {
            services.AddImageSharp(
                options =>
                    {
                        options.Configuration = Configuration.Default;
                        options.MaxBrowserCacheDays = 7;
                        options.MaxCacheDays = 365;
                        options.CachedNameLength = 8;
                        options.OnParseCommands = _ => { };
                        options.OnBeforeSave = _ => { };
                        options.OnProcessed = _ => { };
                        options.OnPrepareResponse = _ => { };
                    });
        }

        private void ConfigureCustomServicesAndDefaultOptions(IServiceCollection services)
        {
            services.AddImageSharp()
                    .RemoveProcessor<FormatWebProcessor>()
                    .RemoveProcessor<BackgroundColorWebProcessor>();
        }

        private void ConfigureCustomServicesAndCustomOptions(IServiceCollection services)
        {
            services.AddImageSharpCore(
                options =>
                    {
                        options.Configuration = Configuration.Default;
                        options.MaxBrowserCacheDays = 7;
                        options.MaxCacheDays = 365;
                        options.CachedNameLength = 8;
                        options.OnParseCommands = _ => { };
                        options.OnBeforeSave = _ => { };
                        options.OnProcessed = _ => { };
                        options.OnPrepareResponse = _ => { };
                    })
                .SetRequestParser<QueryCollectionRequestParser>()
                .SetMemoryAllocator(provider => ArrayPoolMemoryAllocator.CreateWithMinimalPooling())
                .Configure<PhysicalFileSystemCacheOptions>(options =>
                {
                    options.CacheFolder = "different-cache";
                })
                .SetCache(provider =>
                {
                    return new PhysicalFileSystemCache(
                        provider.GetRequiredService<IOptions<PhysicalFileSystemCacheOptions>>(),
#pragma warning disable CS0618 // Type or member is obsolete
                        provider.GetRequiredService<IHostingEnv>(),
#pragma warning restore CS0618 // Type or member is obsolete
                        provider.GetRequiredService<IOptions<ImageSharpMiddlewareOptions>>(),
                        provider.GetRequiredService<FormatUtilities>());
                })
                .SetCacheHash<CacheHash>()
                .AddProvider<PhysicalFileSystemProvider>()
                .AddProcessor<ResizeWebProcessor>()
                .AddProcessor<FormatWebProcessor>()
                .AddProcessor<BackgroundColorWebProcessor>();
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
#pragma warning disable CS0618 // Type or member is obsolete
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHostingEnv hostingEnv)
#pragma warning restore CS0618 // Type or member is obsolete
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //app.UseHttpsRedirection();

            //app.UseRouting();

            //app.UseAuthorization();
            string FilePath = "wwwroot";
            Directory.CreateDirectory(FilePath);
            app.UseImageSharp();
            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapControllers();
            //});
        }
    }
}
