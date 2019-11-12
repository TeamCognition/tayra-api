using System;
using System.Linq;
using Firdaws.Core;
using Microsoft.Extensions.Logging;
using Tayra.Common;
using Tayra.Models.Core;
using Tayra.Models.Organizations;
using Tayra.Services;
using Tayra.SyncServices.Common;

namespace Tayra.SyncServices.Tayra
{
    public class SyncCompetitionsLoader : BaseLoader
    {
        #region Private Variables

        #endregion

        #region Constructor

        public SyncCompetitionsLoader(LogService logService, CoreDbContext coreDb) : base(logService, coreDb)
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
                    FetchCompetitionsData(organizationDb, date, LogService);
                }
            }
        }



        #endregion

        #region Private Methods

        private void FetchCompetitionsData(OrganizationDbContext organizationDb, DateTime fromDay, LogService logService)
        {
            try
            {
                var dateId = DateHelper2.ToDateId(fromDay);
                var competitions = (from c in organizationDb.Competitions
                                    where c.Status == CompetitionStatus.Started
                                    where c.ScheduledEndAt.HasValue && c.ScheduledEndAt.Value.Date <= fromDay.Date
                                    select c)
                                    .ToList();

                var competitionsService = new CompetitionsService(organizationDb);
                foreach (var c in competitions)
                {
                    competitionsService.EndCompetition(c.Id);
                    logService.Log<SyncCompetitionsLoader>($"Competition {c.Id} has been ended.");
                }

                organizationDb.SaveChanges();
            }
            catch (Exception e)
            {
                if (e == null)
                    throw new Exception();
            }


        }

        #endregion
    }
}
