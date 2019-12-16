using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace Tayra.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration(SharedAppConfiguration)
                .UseStartup<Startup>();

        public static void SharedAppConfiguration(WebHostBuilderContext hostingContext, IConfigurationBuilder config)
        {
            var env = hostingContext.HostingEnvironment;

            var dict = new Dictionary<string, string>
            {
                {"DatabaseUser", "tyradmin"},
                {"DatabasePassword", "Kr7N9#p!2AbR"},
                {"DatabaseServerPort", "1433"},
                {"ConnectionTimeOut", "100"},

                {"CatalogServer", "sqlserver-tayra.database.windows.net"},
                {"CatalogDatabase", "sqldb-tayra-catalog-prod"},


                {"BlobPath", "https://tayra.blob.core.windows.net/"},
                {"BlobStorageConnectionStr", "DefaultEndpointsProtocol=https;AccountName=tayra;AccountKey=98ZragdvKWY2WgKDOsKTZfhXze0nAe8/wkZKzOanlcF7W9qrjqwX/HMg6upmZ9c3Sgu9FvvGyfh1N+zb1gtPVA==;EndpointSuffix=core.windows.net"},
                {"BlobContainerImages", "imgs"}
            };

            config.AddInMemoryCollection(dict);


            config
                .AddJsonFile("appsettings.json", optional: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            config.AddEnvironmentVariables();
        }
    }
}
