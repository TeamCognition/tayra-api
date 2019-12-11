using System;
using System.Linq;
using Firdaws.Core;
using Tayra.Common;
using Tayra.Models.Catalog;
using Tayra.Models.Organizations;
using Tayra.Services;
using Tayra.SyncServices.Common;

namespace Tayra.SyncServices.Tayra
{
    public class SyncCompetitionsLoader : BaseLoader
    {
        #region Private Variables

        private readonly IShardMapProvider _shardMapProvider;

        #endregion

        #region Constructor

        public SyncCompetitionsLoader(IShardMapProvider shardMapProvider, LogService logService, CatalogDbContext catalogDb) : base(logService, catalogDb)
        {
            _shardMapProvider = shardMapProvider;
        }

        #endregion

        #region Public Methods

        public override void Execute(DateTime date, params Tenant[] tenants)
        {
            foreach (var tenant in tenants)
            {
                LogService.SetOrganizationId(tenant.Name);
                using (var organizationDb = new OrganizationDbContext(null, new ShardTenantProvider(tenant.Name), _shardMapProvider))
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
                    logService.Log<SyncCompetitionsLoader> ($"Competition {c.Id} has been ended.");
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
