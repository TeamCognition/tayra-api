using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Tayra.Models.Organizations;

namespace Tayra.Connectors.App.Helpers
{
    public static class WebHostExtentions
    {
        public static IWebHost InitializeDatabase(this IWebHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetService<OrganizationDbContext>();
                context.Database.EnsureCreated();
            }

            return host;
        }
    }
}