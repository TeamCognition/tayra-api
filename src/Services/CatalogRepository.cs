using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using Tayra.Models.Catalog;

namespace Tayra.Services
{
    public class CatalogRepository : ICatalogRepository
    {
        #region Private variables

        private readonly CatalogDbContext _catalogDbContext;

        #endregion

        #region Constructor

        public CatalogRepository(CatalogDbContext catalogDbContext)
        {
            _catalogDbContext = catalogDbContext;
        }

        #endregion

        public List<TenantModel> GetAllTenants()
        {
            var allTenantsList = _catalogDbContext.Tenants.ToList();

            if (allTenantsList.Count > 0)
            {
                return allTenantsList.Select(tenant => ToTenantModel(tenant)).ToList();
            }

            return null;
        }

        public TenantModel GetTenant(string tenantName)
        {
            var tenants = _catalogDbContext.Tenants.Where(i => Regex.Replace(i.Name.ToLower(), @"\s+", "") == tenantName).ToList();

            if (tenants.Any())
            {
                var tenant = tenants.FirstOrDefault();

                if(tenant != null)
                    return ToTenantModel(tenant);
            }

            return null;
        }

        public bool Add(Tenant tenant)
        {
            _catalogDbContext.Tenants.Add(tenant);
            _catalogDbContext.SaveChangesAsync();

            return true;
        }

        #region Private methods

        private static TenantModel ToTenantModel(Tenant tenant)
        {
            return new TenantModel
            {
                ServicePlan = tenant.ServicePlan,
                Id = ConvertByteKeyIntoInt(tenant.Id),
                Name = tenant.Name.ToLower().Replace(" ", ""),
                IdInString = BitConverter.ToString(tenant.Id).Replace("-", "")
            };
        }

        /// <summary>
        /// Converts the byte key into int.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public static int ConvertByteKeyIntoInt(byte[] key)
        {
            // Make a copy of the normalized array
            byte[] denormalized = new byte[key.Length];

            key.CopyTo(denormalized, 0);

            // Flip the last bit and cast it to an integer
            denormalized[0] ^= 0x80;

            return IPAddress.HostToNetworkOrder(BitConverter.ToInt32(denormalized, 0));
        }

        #endregion
    }
}
