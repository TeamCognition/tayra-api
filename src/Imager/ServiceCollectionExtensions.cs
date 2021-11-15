using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SixLabors.ImageSharp.Web;
using SixLabors.ImageSharp.Web.Caching;
using SixLabors.ImageSharp.Web.Commands;
using SixLabors.ImageSharp.Web.DependencyInjection;
using SixLabors.ImageSharp.Web.Middleware;
using SixLabors.ImageSharp.Web.Processors;
using SixLabors.ImageSharp.Web.Providers;
using SixLabors.ImageSharp.Web.Providers.Azure;

namespace Tayra.Imager
{
    public static class ServiceCollectionExtensions
    {
        public static void AddImagerServices(this IServiceCollection services, IConfiguration configuration, string cacheFolderName = "./is-cache")
        {
            services.AddImageSharp()
                .SetRequestParser<QueryCollectionRequestParser>()
                .Configure<PhysicalFileSystemCacheOptions>(x => x.CacheFolder = cacheFolderName)
                .SetCache<PhysicalFileSystemCache>()
                .SetCacheHash<CacheHash>()
                .Configure<AzureBlobStorageImageProviderOptions>(options =>
                {
                    options.BlobContainers.Add(new AzureBlobContainerClientOptions
                    {
                        ConnectionString = configuration["BlobStorageConnectionStr"],
                        ContainerName = configuration["BlobContainerImages"]
                    });
                })
                .AddProvider<AzureBlobStorageImageProvider>()
                .AddProcessor<ResizeWebProcessor>();
            
            
            //diffs on new sample
            // services.AddImageSharp()
            //     .SetCache(provider => new PhysicalFileSystemCache(
            //         provider.GetRequiredService<IOptions<PhysicalFileSystemCacheOptions>>(),
            //         provider.GetRequiredService<IWebHostEnvironment>(),
            //         provider.GetRequiredService<IOptions<ImageSharpMiddlewareOptions>>(),
            //         provider.GetRequiredService<FormatUtilities>()))
            //     .AddProcessor<ResizeWebProcessor>()
            //     .AddProcessor<FormatWebProcessor>()
            //     .AddProcessor<BackgroundColorWebProcessor>()
            //     .AddProcessor<JpegQualityWebProcessor>();
        }
    }
}