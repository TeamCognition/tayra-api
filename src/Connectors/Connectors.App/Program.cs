using System.IO;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace Tayra.Connectors.App
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

            // find the shared folder in the parent folder
            var sharedFolder = Path.Combine(env.ContentRootPath, "../../..", "shared");

            //load the SharedSettings first, so that appsettings.json overrwrites it
            config
                .AddJsonFile(Path.Combine(sharedFolder, "sharedSettings.json"), optional: true)
                            .AddJsonFile("sharedSettings.json", optional: true) // When app is published
                            .AddJsonFile("appsettings.json", optional: true)
                            .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            config.AddEnvironmentVariables();
        }
    }
}
