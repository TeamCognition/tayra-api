using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Tayra.Models.Catalog;

namespace SyncFunctions
{
    public class GetTenantsByLocalTimeService
    {
        private readonly CatalogDbContext _catalogDb;
        
        public GetTenantsByLocalTimeService(CatalogDbContext catalogDb)
        {
            _catalogDb = catalogDb;
        }

        public async Task<Tenant[]> Execute(int localTimeHours)
        {
            var tzIds = GetTimezonesIdsByTime(localTimeHours);
            return await _catalogDb.TenantInfo.Where(x => tzIds.Contains(x.Timezone)).ToArrayAsync();
        }

        private string[] GetTimezonesIdsByTime(int hours)
        {
            var utcTime = DateTime.UtcNow;
            var minHours = hours - 1;
            var maxHours = hours;
            return TimeZoneInfo.GetSystemTimeZones()
                .Select(tz => new { Zone = tz, DateTime = TimeZoneInfo.ConvertTimeFromUtc(utcTime, tz) })
                .Where(tz => tz.DateTime.TimeOfDay.TotalHours > minHours && tz.DateTime.TimeOfDay.TotalHours <= maxHours)
                .Select(tz => tz.Zone.Id)
                .ToArray();
        }
    }
}