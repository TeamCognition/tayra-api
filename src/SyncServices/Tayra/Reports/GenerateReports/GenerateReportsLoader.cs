using System;
using Firdaws.Core;
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
            if(requestBody.TryGetValue("startDateId", StringComparison.InvariantCultureIgnoreCase, out JToken value))
            {
                date = DateHelper2.ParseDate(value.Value<int>());
                if(date > endDate)
                {
                    throw new ApplicationException("startDateId has to be older date");
                }
            }
                
            foreach (var tenant in tenants)
            {
                LogService.SetOrganizationId(tenant.Key);
                using (var organizationDb = new OrganizationDbContext(null, new ShardTenantProvider(tenant.Key), _shardMapProvider))
                {
                    do
                    {
                        var profileDailyReports = GenerateProfileReportsLoader.GenerateProfileReportsDaily(organizationDb, date, LogService);
                        var profileWeeklyReports = GenerateProfileReportsLoader.GenerateProfileReportsWeekly(organizationDb, date, LogService);

                        GenerateSegmentReportsLoader.GenerateSegmentReportsDaily(organizationDb, date, LogService, profileDailyReports);
                        GenerateSegmentReportsLoader.GenerateSegmentReportsWeekly(organizationDb, date, LogService, profileDailyReports, profileWeeklyReports);

                        GenerateTeamReportsLoader.GenerateTeamReportsDaily(organizationDb, date, LogService, profileDailyReports);
                        GenerateTeamReportsLoader.GenerateTeamReportsWeekly(organizationDb, date, LogService, profileDailyReports, profileWeeklyReports);

                        date = date.AddDays(1);
                    } while (date <= endDate);

                    MakeActionPointsLoader.MakeActionPoints(organizationDb, date, LogService);
                }
            }
        }

        #endregion
    }
}