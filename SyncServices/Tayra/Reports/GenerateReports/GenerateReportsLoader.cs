using System;
using Microsoft.Extensions.Configuration;
using Tayra.Models.Catalog;
using Tayra.Models.Organizations;
using Tayra.SyncServices.Common;

namespace Tayra.SyncServices.Tayra
{
    public class GenerateReportsLoader : BaseLoader
    {
        private readonly IConfiguration _config;

        #region Constructor

        public GenerateReportsLoader(IConfiguration config, LogService logService, CatalogDbContext catalogDb) : base(logService, catalogDb)
        {
            _config = config;
        }

        #endregion

        #region Public Methods

        public override void Execute(DateTime date, params Tenant[] tenants)
        {
            foreach (var tenant in tenants)
            {
                LogService.SetOrganizationId(tenant.Name);
                using (var organizationDb = new OrganizationDbContext(null, new ShardTenantProvider(tenant.Name, _config)))
                {
                    var profileDailyReports = GenerateProfileReportsLoader.GenerateProfileReportsDaily(organizationDb, date, LogService);
                    var profileWeeklyReports = GenerateProfileReportsLoader.GenerateProfileReportsWeekly(organizationDb, date, LogService);

                    GenerateProjectReportsLoader.GenerateProjectReportsDaily(organizationDb, date, LogService, profileDailyReports);
                    GenerateProjectReportsLoader.GenerateProjectReportsWeekly(organizationDb, date, LogService, profileDailyReports, profileWeeklyReports);

                    GenerateTeamReportsLoader.GenerateTeamReportsDaily(organizationDb, date, LogService, profileDailyReports);
                    GenerateTeamReportsLoader.GenerateTeamReportsWeekly(organizationDb, date, LogService, profileDailyReports, profileWeeklyReports);
                }
            }
        }

        #endregion
    }
}