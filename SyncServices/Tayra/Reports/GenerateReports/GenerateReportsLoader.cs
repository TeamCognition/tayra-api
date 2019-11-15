using System;
using System.Collections.Generic;
using System.Linq;
using Firdaws.Core;
using Microsoft.EntityFrameworkCore;
using MoreLinq;
using Tayra.Common;
using Tayra.Models.Catalog;
using Tayra.Models.Organizations;
using Tayra.SyncServices.Common;

namespace Tayra.SyncServices.Tayra
{
    public class GenerateReportsLoader : BaseLoader
    {
        #region Constructor

        public GenerateReportsLoader(LogService logService, CatalogDbContext catalogDb) : base(logService, catalogDb)
        {

        }

        #endregion

        #region Public Methods

        public override void Execute(DateTime date, params Organization[] organizations)
        {
            foreach (var org in organizations)
            {
                LogService.SetOrganizationId(org.Id);
                using (var organizationDb = new OrganizationDbContext(org.Database, false))
                {
                    var profileDailyReports = GenerateProfileReportsDailyLoader.GenerateProfileReports(organizationDb, date, LogService);
                    var profileWeeklyReports = GenerateProfileReportsWeeklyLoader.GenerateProfileReports(organizationDb, date, LogService);
                    
                    GenerateProjectReportsDailyLoader.GenerateProjectReports(organizationDb, date, LogService, profileDailyReports);
                    GenerateProjectReportsWeeklyLoader.GenerateProjectReports(organizationDb, date, LogService, profileDailyReports, profileWeeklyReports);

                    GenerateTeamReportsDailyLoader.GenerateTeamReports(organizationDb, date, LogService, profileDailyReports);
                    GenerateTeamReportsWeeklyLoader.GenerateTeamReports(organizationDb, date, LogService, profileDailyReports, profileWeeklyReports);
                }
            }
        }

        #endregion
        }
    }