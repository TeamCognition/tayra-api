using System;
using Microsoft.AspNetCore.Mvc;
using Tayra.API.Filters;
using Tayra.Models.Organizations;

namespace Tayra.API.Controllers
{
    [Route("{segment}/[controller]"), TypeFilter(typeof(SegmentAuthorizationFilter))]
    public class BaseDataController : BaseController
    {
        private OrganizationDbContext _organizationContext;

        public BaseDataController(IServiceProvider serviceProvider, OrganizationDbContext context) : base(serviceProvider)
        {
            _organizationContext = context;
        }

        public Segment CurrentSegment { get; set; }

        //public OrganizationDbContext OrganizationContext => _organizationContext ?? (_organizationContext = new OrganizationDbContext(CurrentSegment.DataWarehouse));
        public OrganizationDbContext OrganizationContext => _organizationContext;
    }
}