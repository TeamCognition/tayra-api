﻿using System.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Tayra.Models.Catalog
{
    public class OrganizationContextFactory : IDesignTimeDbContextFactory<CatalogDbContext>
    {
        public CatalogDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<CatalogDbContext>();
            optionsBuilder.UseSqlServer("Data Source=tcp:tayra-sqlserver.czyjrarofbip.eu-central-1.rds.amazonaws.com,1433;Initial Catalog=tayra-catalog;User ID=admin;Password=Kr7N9#p!2AbR;Connect Timeout=100;Encrypt=True;TrustServerCertificate=True;");

            return new CatalogDbContext(optionsBuilder.Options);
        }
    }

    public static class OrganizationContextFactoryForTests
    {
        public static CatalogDbContext CreateDbContext(DbContextOptions<CatalogDbContext> options)
        {
            return new CatalogDbContext(options);
        }
    }
}