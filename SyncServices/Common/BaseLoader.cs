using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Tayra.Models.Catalog;

namespace Tayra.SyncServices.Common
{
    public abstract class BaseLoader
    {
        #region Protected Members

        protected LogService LogService;
        protected CatalogDbContext CatalogDbContext;

        #endregion

        #region Constructors

        protected BaseLoader(LogService logService, CatalogDbContext catalogDb)
        {
            LogService = logService;
            CatalogDbContext = catalogDb;
        }

        #endregion

        #region Abstract Methods

        public void Execute(DateTime date, params TimeZoneDTO[] timeZoneDTO) => Execute(date, GetOrganizations(timeZoneDTO));

        public void Execute(DateTime date, string organizationKey) => Execute(date, GetOrganization(organizationKey));

        public abstract void Execute(DateTime date, params Organization[] organizations);

        #endregion

        #region Protected Methods

        protected Organization[] GetOrganizations(params TimeZoneDTO[] timezoneInfo)
        {
            var timezones = timezoneInfo.Select(t => t.Id).ToList();

            var organizations = CatalogDbContext.Organizations
                .AsNoTracking();

            if (timezones.Count > 0)
            {
                organizations = organizations.Where(x => timezones.Contains(x.Timezone));
            }

            var orgs = organizations.ToArray();
            return orgs;
        }

        protected Organization GetOrganization(string organizationKey)
        {
            return CatalogDbContext.Organizations.AsNoTracking().FirstOrDefault(x => x.Key == organizationKey);
        }

        #endregion
    }
}