using System;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Tayra.DAL;
using Tayra.Models.Catalog;
using Tayra.Models.Organizations;

namespace Tayra.Models.Seeder
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Seed started!");

            var config = LoadSettings();

            if (args.Length == 0)
            {
                Console.WriteLine("Usage: SeederExe option [params]");
                Console.WriteLine("  option can be:");
                Console.WriteLine("  all - seed all tenants");
                Console.WriteLine("  single or demo tenant_name - seed single tenant");
                Console.WriteLine("     if tenant_name == '" + Seeder.DemoKey + "', a special demo seed will be run");
                Console.WriteLine("  tasks tenant_name - seed tasks from Input/tasks.txt");
                Console.WriteLine("So ... what will it be ? Enter your command ");
                args = new string[1] { Console.ReadLine() };
            }

            if (args[0] == "all")
            {
                Seeder.SeedAll(config);
            }
            else if (args[0] == "single" || args[0] == "demo")
            {
                if (args.Length == 1)
                {
                    Console.WriteLine("Enter tenant name: ");
                    args = new string[2] { args[0], Console.ReadLine() };
                }
                
                using (var catalogDbContext =
                    new CatalogDbContext(ConnectionStringUtilities.GetCatalogDbConnStr(config)))
                {
                    var tenantConnStr = catalogDbContext.TenantInfo.Where(x => x.Identifier == args[1])
                        .Select(x => x.ConnectionString).FirstOrDefault();

                    var all = catalogDbContext.TenantInfo.ToArray();
                    all.ToList().ForEach(x => Console.WriteLine(x.Identifier));
                    if (string.IsNullOrEmpty(tenantConnStr))
                    {
                        Console.WriteLine("Could not find tenant with identifier " + args[1]);
                    }
                    Seeder.Seed(shouldDemoSeed: args[0] == "demo", tenantConnStr);
                }
            }
            Console.WriteLine("Seed finished!");
        }

        public static IConfigurationRoot LoadSettings()
        {
            return new ConfigurationBuilder()
                .AddJsonFile("sharedSettings.json", optional: true)
                .AddJsonFile("appsettings.json", optional: true)
                .Build();
        }
    }
}
