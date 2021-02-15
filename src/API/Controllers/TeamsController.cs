using System;
using Cog.Core;
using Microsoft.AspNetCore.Mvc;
using Tayra.Models.Organizations;
using Tayra.Services;

namespace Tayra.API.Controllers
{
    public class TeamsController : BaseController
    {
        #region Constructor

        public TeamsController(IServiceProvider serviceProvider, OrganizationDbContext dbContext) : base(serviceProvider)
        {
            OrganizationContext = dbContext;
        }

        #endregion

        public OrganizationDbContext OrganizationContext { get; set; }

        #region Action Methods

        [HttpPost("search")]
        public ActionResult<GridData<TeamViewGridDTO>> Search([FromBody] TeamViewGridParams gridParams)
        {
            return TeamsService.GetViewGridData(CurrentUser.SegmentsIds, gridParams);
        }

        [HttpPost("searchProfiles")]
        public ActionResult<GridData<TeamProfilesGridDTO>> GetTeamProfiles([FromBody] TeamProfilesGridParams gridParams)
        {
            return TeamsService.GetTeamProfilesGridData(gridParams);
        }

        
        #endregion
    }
}
