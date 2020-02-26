using System;
using Firdaws.Core;
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

        public override void Execute(DateTime date, params Tenant[] tenants)
        {
            date = DateHelper2.ParseDate(20191118);
            do
            {
                foreach (var tenant in tenants)
                {
                    LogService.SetOrganizationId(tenant.Name);
                    using (var organizationDb = new OrganizationDbContext(null, new ShardTenantProvider(tenant.Name), _shardMapProvider))
                    {
                        var profileDailyReports = GenerateProfileReportsLoader.GenerateProfileReportsDaily(organizationDb, date, LogService);
                        var profileWeeklyReports = GenerateProfileReportsLoader.GenerateProfileReportsWeekly(organizationDb, date, LogService);

                        GenerateSegmentReportsLoader.GenerateSegmentReportsDaily(organizationDb, date, LogService, profileDailyReports);
                        GenerateSegmentReportsLoader.GenerateSegmentReportsWeekly(organizationDb, date, LogService, profileDailyReports, profileWeeklyReports);

                        GenerateTeamReportsLoader.GenerateTeamReportsDaily(organizationDb, date, LogService, profileDailyReports);
                        GenerateTeamReportsLoader.GenerateTeamReportsWeekly(organizationDb, date, LogService, profileDailyReports, profileWeeklyReports);

                        //MakeActionPointsLoader.MakeActionPoints(organizationDb, date, LogService);
                    }
                }
                date = date.AddDays(1);
            } while (date <= DateHelper2.ParseDate(20200224));
        }

        #endregion
    }
}