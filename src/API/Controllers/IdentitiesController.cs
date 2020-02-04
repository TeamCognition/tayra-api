using System;
using System.Linq;
using Firdaws.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tayra.Models.Organizations;
using Tayra.Services;

namespace Tayra.API.Controllers
{
    public class IdentitiesController : BaseController
    {
        #region Constructor

        public IdentitiesController(ITenantProvider tenantProvider, OrganizationDbContext dbContext, IServiceProvider serviceProvider) : base(serviceProvider)
        {
            TenantProvider = tenantProvider;
            DbContext = dbContext;
        }

        #endregion

        #region Properties

        public readonly ITenantProvider TenantProvider;
        public readonly OrganizationDbContext DbContext;

        #endregion

        #region Action Methods

        [AllowAnonymous, HttpPost("create")]
        public ActionResult Create([FromBody] IdentityCreateDTO dto)
        {
            //var o = Resolve<IOrganizationsService>();
            //o.Create(new OrganizationCreateDTO
            //{
            //    Key = "localhost:3000",
            //    Name = "Localhost 3000",
            //    Timezone = "Europe Central",
            //    DatabaseServer = "sqlserver-tayra.database.windows.net",
            //    DatabaseName = "sqldb-tayra-tenants_free-prod",
            //    TemplateConnectionString = "User ID = tyradmin; Password = Kr7N9#p!2AbR;Connect Timeout=100;Application Name=Tayra"
            //});

            //IdentitiesService.InternalCreateWithProfile(dto);
            //DbContext.SaveChanges();

            return Ok();
        }

        [AllowAnonymous, HttpPost("join")]
        public IActionResult Sendinvitation([FromBody] IdentityJoinDTO dto)
        {
            IdentitiesService.InvitationJoinWithSaveChanges(dto);

            return Ok();
        }

        [HttpPost("invitation")]
        public IActionResult Sendinvitation([FromBody] IdentityInviteDTO dto)
        {
            IdentitiesService.CreateInvitation(CurrentUser.ProfileId, TenantProvider.GetTenant().Host, dto);

            DbContext.SaveChanges();
            return Ok();
        }

        [HttpPost("manage/search")]
        public ActionResult<GridData<IdentityManageGridDTO>> ManageSearch([FromBody] IdentityManageGridParams gridParams)
        {
            if(gridParams.SegmentId.HasValue && !CurrentUser.SegmentsIds.Contains(gridParams.SegmentId.Value))
            {
                throw new FirdawsSecurityException("You don' have perrmission to segment " + gridParams.SegmentId);
            }

            return IdentitiesService.GetIdentityManageGridData(gridParams);
        }

        [HttpGet("manage/assigns/{memberProfileId:int}")]
        public ActionResult<IdentityManageAssignsDTO> GetManageTeamAssignData([FromRoute] int memberProfileId)
        {
            return IdentitiesService.GetIdentityManageAssignsData(CurrentUser.SegmentsIds, memberProfileId);
        }

        [HttpPost("searchInvitations")]
        public ActionResult<GridData<IdentityInvitationGridDTO>> SearchInvitation([FromBody] IdentityInvitationGridParams gridParams)
        {
            return IdentitiesService.GetInvitationsGridData(gridParams);
        }

        [HttpPost("searchEmails")]
        public ActionResult<GridData<IdentityEmailsGridDTO>> GetIdentityEmails([FromBody] IdentityEmailsGridParams gridParams)
        {
            return IdentitiesService.GetIdentityEmailsGridData(CurrentUser.ProfileId, gridParams);
        }

        [AllowAnonymous, HttpGet("invitation")]
        public ActionResult<IdentityInvitationViewDTO> Getinvitation([FromQuery] string InvitationCode)
        {
            return Ok(IdentitiesService.GetInvitation(InvitationCode));
        }

        [HttpGet("isEmailUnique")]
        public ActionResult<bool> IsEmailUnique([FromQuery] string email)
        {
            return IdentitiesService.IsEmailAddressUnique(email);
        }

        [HttpPost("addEmail")]
        public ActionResult AddEmail([FromQuery] string email)
        {
            IdentitiesService.AddEmail(CurrentUser.IdentityId, email);
            return Ok();
        }

        [HttpPost("makeEmailPrimary")]
        public ActionResult MakeEmailPrimary([FromQuery] string email)
        {
            IdentitiesService.SetPrimaryEmail(CurrentUser.IdentityId, email);
            return Ok();
        }

        [HttpDelete("removeEmail")]
        public ActionResult RemoveEmail([FromQuery] string email)
        {
            IdentitiesService.RemoveEmail(CurrentUser.IdentityId, email);
            return Ok();
        }

        [HttpPost("changePassword")]
        public ActionResult ChangePassword([FromBody] IdentityChangePasswordDTO dto)
        {
            IdentitiesService.ChangePasswordWithSaveChange(CurrentUser.IdentityId, dto);
            return Ok();
        }

        #endregion
    }
}