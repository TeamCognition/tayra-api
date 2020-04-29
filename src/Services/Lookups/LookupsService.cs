using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Tayra.Models.Organizations;

namespace Tayra.Services
{
    public class LookupsService : BaseService<OrganizationDbContext>, ILookupsService
    {
        #region Constructor

        public LookupsService(OrganizationDbContext dbContext) : base(dbContext)
        {
        }

        #endregion

        #region Public Methods

        public IEnumerable<LookupDTO> GetFromOrganizationDb<T>(Expression<Func<T, LookupDTO>> selector) where T : class
        {
            return DbContext
                .Set<T>()
                .Select(selector)
                .Distinct()
                .Where(x => x.Key != null)
                .OrderBy(x => x.Value)
                .ToList();
        }

        public IEnumerable<LookupDTO> GetFromEnum<T>() where T : struct
        {
            return typeof(T)
                .GetMembers()
                .Where(x => x.CustomAttributes.Any(ca => ca.AttributeType == typeof(DescriptionAttribute)))
                .Select(x => new LookupDTO((int)(object)Enum.Parse<T>(x.Name), x.GetCustomAttribute<DescriptionAttribute>()?.Description))
                .OrderBy(x => x.Value)
                .ToList();
        }

        #endregion


    }
}
