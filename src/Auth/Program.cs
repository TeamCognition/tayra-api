using System;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Tayra.Auth
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.Title = "Tayra.Auth";

            var seed = args.Contains("/seed");
            if (seed)
            {
                args = args.Except(new[] { "/seed" }).ToArray();
            }

            var host = CreateWebHostBuilder(args).Build();

            if (seed)
            {
                var config = host.Services.GetRequiredService<IConfiguration>();
                var connectionString = config.GetConnectionString("DefaultConnection");
                SeedData.EnsureSeedData(connectionString);
                return;
            }

            host.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            return WebHost.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration(SharedAppConfiguration)
                    .UseStartup<Startup>();

        }

        public static void SharedAppConfiguration(WebHostBuilderContext hostingContext, IConfigurationBuilder config)
        {
            var env = hostingContext.HostingEnvironment;

            // find the shared folder in the parent folder
            var sharedFolder = Path.Combine(env.ContentRootPath, "../..", "shared");

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