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

            Guid[] segmentIds = new Guid[0];
            if (requestBody != null && requestBody.TryGetValue("segmentId", StringComparison.InvariantCultureIgnoreCase, out JToken id))
            {
                segmentIds = new[] { id.Value<Guid>() };
            }


            foreach (var tenant in tenants)
            {
                DateTime tempDate = date;
                LogService.SetOrganizationId(tenant.Key);
                using (var organizationDb = new OrganizationDbContext(null, new ShardTenantProvider(tenant.Key), _shardMapProvider))
                {
                    if (segmentIds.Length == 0)
                    {
                        segmentIds = organizationDb.Segments.Where(x => x.IsReportingUnlocked).Select(x => x.Id).ToArray();
                    }

                    if (segmentIds.Length == 0)
                        continue;

                    do
                    {
                        var profileMetrics = NewProfileMetricsLoader.GenerateProfileMetrics(organizationDb, tempDate, LogService);
                        var segmentMetrics = NewSegmentMetricsLoader.GenerateSegmentMetrics(organizationDb, tempDate, LogService);
                        var teamMetrics = NewTeamMetricsLoader.GenerateTeamMetrics(organizationDb, tempDate, LogService);

                        var profileDailyReports = GenerateProfileReportsLoader.GenerateProfileReportsDaily(organizationDb, tempDate, LogService, segmentIds);
                        var profileWeeklyReports = GenerateProfileReportsLoader.GenerateProfileReportsWeekly(organizationDb, tempDate, LogService, segmentIds);

                        GenerateSegmentReportsLoader.GenerateSegmentReportsDaily(organizationDb, tempDate, LogService, profileDailyReports, segmentIds);
                        GenerateSegmentReportsLoader.GenerateSegmentReportsWeekly(organizationDb, tempDate, LogService, profileDailyReports, profileWeeklyReports, segmentIds);

                        GenerateTeamReportsLoader.GenerateTeamReportsDaily(organizationDb, tempDate, LogService, profileDailyReports, segmentIds);
                        GenerateTeamReportsLoader.GenerateTeamReportsWeekly(organizationDb, tempDate, LogService, profileDailyReports, profileWeeklyReports, segmentIds);

                        tempDate = tempDate.AddDays(1);
                    } while (tempDate <= endDate);

                    MakeActionPointsLoader.MakeActionPoints(organizationDb, tempDate, LogService);
                }
            }
        }

        #endregion
    }
}