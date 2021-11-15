using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Tayra.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureLogging(logging =>
                {
                    //logging.ClearProviders();
                    logging.AddDebug();
                    logging.AddConsole();
                })
                .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.ConfigureAppConfiguration(LoadAppConfiguration);
                webBuilder.UseStartup<Startup>();
            });

        public static void LoadAppConfiguration(WebHostBuilderContext hostingContext, IConfigurationBuilder config)
        {
            var env = hostingContext.HostingEnvironment;

            // find the shared folder in the parent folder
            var sharedFolder = Path.Combine(env.ContentRootPath, "../..", "build");

            //load the SharedSettings first, so that appsettings.json overrwrites it
            config
                .AddJsonFile(Path.Combine(sharedFolder, $"sharedSettings.Development.json"), optional: true)
                .AddJsonFile($"sharedSettings.Production.json", optional: true) // When app is published
                .AddJsonFile("appsettings.json", optional: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);
               

            config.AddEnvironmentVariables();
        }
    }
}
