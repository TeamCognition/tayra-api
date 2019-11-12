using System;
using Microsoft.AspNetCore.Mvc;
using Tayra.API.Filters;
using Tayra.Models.Organizations;

namespace Tayra.API.Controllers
{
    [Route("{project}/[controller]"), TypeFilter(typeof(ProjectAuthorizationFilter))]
    public class BaseDataController : BaseController
    {
        private OrganizationDbContext _organizationContext;

        public BaseDataController(IServiceProvider serviceProvider, OrganizationDbContext context) : base(serviceProvider)
        {
            _organizationContext = context;
        }

        public Project CurrentProject { get; set; }

        public OrganizationDbContext OrganizationContext => _organizationContext ?? (_organizationContext = new OrganizationDbContext(CurrentProject.DataWarehouse));
    }
}