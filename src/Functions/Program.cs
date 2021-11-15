using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Tayra.DAL;
using Tayra.Models.Catalog;

namespace Tayra.Functions
{
    class Program
    {
        static async Task Main(string[] args)
        {
            IConfiguration? configuration = null;
            
            var host = new HostBuilder()
                .ConfigureFunctionsWorkerDefaults()
                .ConfigureAppConfiguration((context, config) =>
                {
                    LoadAppConfiguration(context, config);
                    config.AddCommandLine(args);
                    
                    //hack
                    configuration = config.Build();
                })
                .ConfigureServices(services =>
                {
                    services.AddDbContext<CatalogDbContext>(options =>
                        options.UseSqlServer(ConnectionStringUtilities.GetCatalogDbConnStr(configuration)));
                })
                .Build();
            
            await host.RunAsync();
        }
        
        private static void LoadAppConfiguration(HostBuilderContext context, IConfigurationBuilder config)
        {
            var env = context.HostingEnvironment;
            
            var sharedFolder = Path.Combine(env.ContentRootPath, "../../../..", "build");

            config
                .AddJsonFile(Path.Combine(sharedFolder, "sharedSettings.Development.json"), optional: true)
                .AddJsonFile("sharedSettings.Production.json", optional: true) // When app is published
                .AddJsonFile("appsettings.json", optional: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            config.AddEnvironmentVariables();
        }
    }
}