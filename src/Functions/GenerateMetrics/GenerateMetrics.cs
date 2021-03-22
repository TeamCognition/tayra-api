using System;
using System.Text.Json;
using System.Threading.Tasks;
using Cog.Core;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using SyncFunctions;
using Tayra.Models.Catalog;
using Tayra.Models.Organizations;
using TimeZoneConverter;

namespace Tayra.Functions.GenerateMetrics
{
    public class GenerateMetrics
    {
        private readonly CatalogDbContext _catalogDb;
        
        public GenerateMetrics(CatalogDbContext catalogDb)
        {
            _catalogDb = catalogDb;
        }
        
        [Function(nameof(GenerateMetrics) + "Http")]
        public async Task<bool> Run([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req, FunctionContext context)
        {
            var logger = context.GetLogger(nameof(GenerateMetrics));
            var command = new Command();
            try
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                };
                command = await JsonSerializer.DeserializeAsync<Command>(req.Body, options);
            }
            catch (Exception _)
            {
                return false;
            }

            var tenants = await new GetTenantsByLocalTimeService(_catalogDb).Execute(localTimeHours: 16);
            
            Handle(command, tenants, new LogService(logger));
            return true;
        }

        public record Command
        {
            public int? StartDateId { get; init; }
        }
        
        // [Function(nameof(GenerateMetrics) + "Timer")]
        // public void Run([TimerTrigger("0 */5 * * * *")] MyTimerInfo timerInfo, FunctionContext context)
        // {
        //     
        // }

        public void Handle(Command command, Tenant[] tenants, LogService logService)
        {
            foreach (var tenant in tenants)
            {
                var localTimeZone = TZConvert.GetTimeZoneInfo(tenant.Timezone);
                DateTime endDate = DateTime.UtcNow.Add(localTimeZone.BaseUtcOffset).AddDays(-1);
                
                if (command.StartDateId.HasValue)
                {
                    endDate = DateHelper2.ParseDate(command.StartDateId.Value);
                }
                
                logService.SetOrganizationId(tenant.Identifier);
                var tempDate = endDate; 
                using (var organizationDb = new OrganizationDbContext(tenant, null))
                {
                    do
                    {
                        ProfileMetricsGenerator.GenerateAndSave(organizationDb, tempDate, logService);
                        SegmentMetricsGenerator.GenerateAndSave(organizationDb, tempDate, logService);
                        TeamMetricsGenerator.GenerateAndSave(organizationDb, tempDate, logService);
                        
                        tempDate = tempDate.AddDays(1);
                    } while (tempDate <= endDate);
            
                    //MakeActionPointsLoader.MakeActionPoints(organizationDb, tempDate, logService);
                }
            }
        }
    }
}
