using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SixLabors.ImageSharp.Web.Caching;
using SixLabors.ImageSharp.Web.Commands;
using SixLabors.ImageSharp.Web.DependencyInjection;
using SixLabors.ImageSharp.Web.Processors;
using SixLabors.ImageSharp.Web.Providers.Azure;

namespace Tayra.Imager
{
    public static class ServiceCollectionExtensions
    {
        public static void AddImagerServices(this IServiceCollection services, IConfiguration configuration, string cacheFolderName = "./is-cache")
        {
            services.AddImageSharpCore()
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
        }
    }
}