﻿using System;
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

        public void Execute(DateTime date, params TimeZoneDTO[] timeZoneDTO) => Execute(date, GetTenants(timeZoneDTO));

        public void Execute(DateTime date, string tenantKey) => Execute(date, GetTenant(tenantKey));

        public abstract void Execute(DateTime date, params Tenant[] tenants);

        #endregion

        #region Protected Methods

        protected Tenant[] GetTenants(params TimeZoneDTO[] timezoneInfo)
        {
            var timezones = timezoneInfo.Select(t => t.Id).ToList();

            var tenants = CatalogDbContext.Tenants
                .AsNoTracking();

            if (timezones.Count > 0)
            {
                tenants = tenants.Where(x => timezones.Contains(x.Timezone));
            }

            return tenants.ToArray();
        }

        protected Tenant GetTenant(string tenantKey)
        {
            var t = CatalogDbContext.Tenants.AsNoTracking().FirstOrDefault(x => x.Name == tenantKey);

            if (t == null)
                throw new ApplicationException("Tenant not found with key: " + tenantKey);

            return t;
        }

        #endregion
    }
}