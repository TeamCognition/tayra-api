using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Imager
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration(SharedAppConfiguration)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

        public static void SharedAppConfiguration(HostBuilderContext hostingContext, IConfigurationBuilder config)
        {
            var env = hostingContext.HostingEnvironment;

            // find the shared folder in the parent folder
            var sharedFolder = Path.Combine(env.ContentRootPath, "..", "shared");

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
