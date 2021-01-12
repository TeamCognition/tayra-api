using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Tayra.Auth.Data
{
    public class OpeniddictContextFactory : IDesignTimeDbContextFactory<OpeniddictDbContext>
    {
        public OpeniddictDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<OpeniddictDbContext>();
            //optionsBuilder.UseSqlServer("Data Source=tcp:tayra-sqlserver.czyjrarofbip.eu-central-1.rds.amazonaws.com,1433;Initial Catalog=tayra-catalog;User ID=admin;Password=Kr7N9#p!2AbR;Connect Timeout=100;Encrypt=True;TrustServerCertificate=True;");
            optionsBuilder.UseSqlServer("Data Source=tcp:localhost,1433;Initial Catalog=tayra-auth;User ID=sa;Password=strong!Password;Connect Timeout=100;Encrypt=True;TrustServerCertificate=True;");
            return new OpeniddictDbContext(optionsBuilder.Options);
        }
    }
}