using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Tayra.Services
{
    public interface ILookupsService
    {
        IEnumerable<LookupDTO> GetFromOrganizationDb<T>(Expression<Func<T, LookupDTO>> selector) where T : class;
        IEnumerable<LookupDTO> GetFromEnum<T>() where T : struct;
    }
}
