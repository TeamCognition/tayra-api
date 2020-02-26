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

            //Seeder.SeedAll(shardMapProvider, config);
            //Seeder.Seed(shardMapProvider, "tajra");
            Seeder.SeedTasksFromTxt(shardMapProvider, "tajra");
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
