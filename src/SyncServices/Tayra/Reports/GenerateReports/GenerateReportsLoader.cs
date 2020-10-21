using System;
using System.Linq;
using Cog.Core;
using Newtonsoft.Json.Linq;
using Tayra.Models.Catalog;
using Tayra.Models.Organizations;
using Tayra.SyncServices.Common;

namespace Tayra.SyncServices.Tayra
{
    public class GenerateReportsLoader : BaseLoader
    {
        private readonly IShardMapProvider _shardMapProvider;

        #region Constructor

        public GenerateReportsLoader(IShardMapProvider shardMapProvider, LogService logService, CatalogDbContext catalogDb) : base(logService, catalogDb)
        {
            _shardMapProvider = shardMapProvider;
        }

        #endregion

        #region Public Methods

        //DateId should be local date and not utc?
        public override void Execute(DateTime endDate, JObject requestBody, params Tenant[] tenants)
        {
            DateTime date = endDate;
            if (requestBody != null && requestBody.TryGetValue("startDateId", StringComparison.InvariantCultureIgnoreCase, out JToken value))
            {
                date = DateHelper2.ParseDate(value.Value<int>());
                if (date.Date > endDate.Date)
                {
                    date = endDate;
                }
            }

            int[] segmentIds = new int[0];
            if (requestBody != null && requestBody.TryGetValue("segmentId", StringComparison.InvariantCultureIgnoreCase, out JToken id))
            {
                segmentIds = new int[] { id.Value<int>() };
            }
            
            foreach (var tenant in tenants)
            {
                LogService.SetOrganizationId(tenant.Key);
                using (var organizationDb = new OrganizationDbContext(null, new ShardTenantProvider(tenant.Key), _shardMapProvider))
                {
                    if (segmentIds.Length == 0)
                    {
                        segmentIds = organizationDb.Segments.Where(x => x.IsReportingUnlocked).Select(x => x.Id).ToArray();
                    }

                    do
                    {
                        var profileMetrics = NewProfileMetricsLoader.GenerateProfileMetrics(organizationDb, date, LogService);
                        var segmentMetrics = NewSegmentMetricsLoader.GenerateSegmentMetrics(organizationDb, date, LogService);
                        var teamMetrics = NewTeamMetricsLoader.GenerateTeamMetrics(organizationDb, date, LogService);
                        
                         var profileDailyReports = GenerateProfileReportsLoader.GenerateProfileReportsDaily(organizationDb, date, LogService, segmentIds);
                         var profileWeeklyReports = GenerateProfileReportsLoader.GenerateProfileReportsWeekly(organizationDb, date, LogService, segmentIds);
                        
                         GenerateSegmentReportsLoader.GenerateSegmentReportsDaily(organizationDb, date, LogService, profileDailyReports, segmentIds);
                        GenerateSegmentReportsLoader.GenerateSegmentReportsWeekly(organizationDb, date, LogService, profileDailyReports, profileWeeklyReports, segmentIds);
                        
                        GenerateTeamReportsLoader.GenerateTeamReportsDaily(organizationDb, date, LogService, profileDailyReports, segmentIds);
                        GenerateTeamReportsLoader.GenerateTeamReportsWeekly(organizationDb, date, LogService, profileDailyReports, profileWeeklyReports, segmentIds);

                        date = date.AddDays(1);
                    } while (date <= endDate);

                    MakeActionPointsLoader.MakeActionPoints(organizationDb, date, LogService);
                }
            }
        }

        #endregion
    }
}