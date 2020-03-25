﻿using System;
using System.Collections.Generic;
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
        public override void Execute(DateTime date, JObject requestBody, params Tenant[] tenants)
        {
            foreach (var tenant in tenants)
            {
                LogService.SetOrganizationId(tenant.Key);
                using (var organizationDb = new OrganizationDbContext(null, new ShardTenantProvider(tenant.Key), _shardMapProvider))
                {
                    var profileDailyReports = GenerateProfileReportsLoader.GenerateProfileReportsDaily(organizationDb, date, LogService);
                    var profileWeeklyReports = GenerateProfileReportsLoader.GenerateProfileReportsWeekly(organizationDb, date, LogService);

                    GenerateSegmentReportsLoader.GenerateSegmentReportsDaily(organizationDb, date, LogService, profileDailyReports);
                    GenerateSegmentReportsLoader.GenerateSegmentReportsWeekly(organizationDb, date, LogService, profileDailyReports, profileWeeklyReports);

                    GenerateTeamReportsLoader.GenerateTeamReportsDaily(organizationDb, date, LogService, profileDailyReports);
                    GenerateTeamReportsLoader.GenerateTeamReportsWeekly(organizationDb, date, LogService, profileDailyReports, profileWeeklyReports);

                    MakeActionPointsLoader.MakeActionPoints(organizationDb, date, LogService);
                }
            }
        }

        #endregion
    }
}