using System;
using Microsoft.Extensions.Configuration;

namespace Seeder
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var config = LoadSettings();
            Console.WriteLine(config["CatalogServer"]);
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
