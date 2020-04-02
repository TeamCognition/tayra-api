using System;
using Microsoft.Extensions.Configuration;
using Tayra.Models.Organizations;

namespace Tayra.Models.Seeder
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Seed started!");

            var config = LoadSettings();
            var shardMapProvider = new ShardMapProvider(config);

            if (args.Length == 0)
            {
                Console.WriteLine("Usage: SeederExe option [params]");
                Console.WriteLine("  option can be:");
                Console.WriteLine("  all - seed all tenants");
                Console.WriteLine("  single tenant_name - seed single tenant");
                Console.WriteLine("     if tenant_name == '" + Seeder.DemoKey + "', a special demo seed will be run");
                Console.WriteLine("  tasks tenant_name - seed tasks from Input/tasks.txt");
                Console.WriteLine("So ... what will it be ? Enter your command ");
                args = new string[1] {Console.ReadLine()};
            }
            
            if (args[0] == "all")
            {
                Seeder.SeedAll(shardMapProvider, config);
            }
            else if (args[0] == "single")
            {
                if (args.Length == 1)
                {
                    Console.WriteLine("Enter tenant name: ");
                    args = new string[2] {args[0], Console.ReadLine()};
                }
                Seeder.Seed(shardMapProvider, args[1]);
            }
            else if (args[0] == "tasks")
            {
                if (args.Length == 1)
                {
                    Console.WriteLine("Enter tenant name: ");
                    args = new string[2] {args[0], Console.ReadLine()};
                }
                Seeder.SeedTasksFromTxt(shardMapProvider, args[1]);
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
