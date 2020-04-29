using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Tayra.Models.Organizations
{
    //https://www.youtube.com/watch?v=-1cTReZ9lzY
    public class DynamicModelCacheKeyFactory : IModelCacheKeyFactory
    {
        public object Create(DbContext dbContext)
        {
            if(dbContext is OrganizationDbContext dynamicContext)
            {
                return new { dynamicContext.CurrentTenantId };
            }

            throw new Exception("Unknown DbContext type");
        }
    }
}
