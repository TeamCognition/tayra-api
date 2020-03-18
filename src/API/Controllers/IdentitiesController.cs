using System;
using System.Linq;
using Firdaws.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tayra.Common;
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
            //    Key = "devtenant.tayra.local",
            //    Name = "Dev Tenant",
            //    Timezone = "Europe Central",
            //    DatabaseServer = "sqlserver-tayra.database.windows.net",
            //    DatabaseName = "sqldb-tayra-tenant_cognition",
            //    TemplateConnectionString = "User ID = tyradmin; Password = Kr7N9#p!2AbR;Connect Timeout=100;Application Name=Tayra"
            //});

            //IdentitiesService.CreateInvitation(0, "devtenant.tayra.local", new IdentityInviteDTO
            //{
            //    EmailAddress = "haris+00@tayra.io",
            //    FirstName = "Haris",
            //    LastName = "Botic",
            //    Role = ProfileRoles.Admin
            //});
            //DbContext.SaveChanges();

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
            IdentitiesService.CreateInvitation(CurrentUser.ProfileId, TenantProvider.GetTenant().Key, dto);

            DbContext.SaveChanges();
            return Ok();
        }

        [HttpPost("searchInvitations")]
        public ActionResult<GridData<IdentityInvitationGridDTO>> SearchInvitation([FromBody] IdentityInvitationGridParams gridParams)
        {
            return IdentitiesService.GetInvitationsGridData(gridParams);
        }

        [AllowAnonymous, HttpGet("invitation")]
        public ActionResult<IdentityInvitationViewDTO> Getinvitation([FromQuery] string InvitationCode)
        {
            var invitation = IdentitiesService.GetInvitation(InvitationCode);
            DbContext.SaveChanges();
            return invitation;
        }

        [HttpDelete("invitation/{invitationId:int}")]
        public IActionResult Deleteinvitation([FromRoute] int invitationId)
        {
            IdentitiesService.DeleteInvitation(invitationId);

            DbContext.SaveChanges();
            return Ok();
        }


        [HttpPost("searchEmails")]
        public ActionResult<GridData<IdentityEmailsGridDTO>> GetIdentityEmails([FromBody] IdentityEmailsGridParams gridParams)
        {
            return IdentitiesService.GetIdentityEmailsGridData(CurrentUser.ProfileId, gridParams);
        }

        [HttpGet("isEmailUnique")]
        public ActionResult<bool> IsEmailUnique([FromQuery] string email)
        {
            return IdentitiesService.IsEmailAddressUnique(email);
        }

        [HttpPost("addEmail")]
        public ActionResult AddEmail([FromQuery] string email)
        {
            IdentitiesService.AddEmail(CurrentUser.IdentityId, CurrentUser.ProfileId, email);
            DbContext.SaveChanges();
            return Ok();
        }

        [HttpPost("makeEmailPrimary")]
        public ActionResult MakeEmailPrimary([FromQuery] string email)
        {
            IdentitiesService.SetPrimaryEmail(CurrentUser.IdentityId, CurrentUser.ProfileId, email);
            DbContext.SaveChanges();
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

        [HttpPost("manage/search")]
        public ActionResult<GridData<IdentityManageGridDTO>> ManageSearch([FromBody] IdentityManageGridParams gridParams)
        {
            if (gridParams.SegmentId.HasValue && !CurrentUser.SegmentsIds.Contains(gridParams.SegmentId.Value))
            {
                throw new FirdawsSecurityException("You don' have perrmission to segment " + gridParams.SegmentId);
            }

            return IdentitiesService.GetIdentityManageGridData(CurrentUser.ProfileId,CurrentUser.Role, gridParams);
        }

        [HttpGet("manage/assigns/{profileId:int}")]
        public ActionResult<IdentityManageAssignsDTO> GetManageTeamAssignData([FromRoute] int profileId)
        {
            if(profileId == CurrentUser.ProfileId)
            {
                throw new FirdawsSecurityException("You can't assign yourself to a team or segment");
            }

            return IdentitiesService.GetIdentityManageAssignsData(CurrentUser.SegmentsIds, profileId);
        }

        [HttpPut("manage/changeRole/{profileId:int}")]
        public IActionResult ChangeProfile([FromRoute] int profileId, [FromBody] ProfileRoles toRole)
        {
            IdentitiesService.ChangeProfileRole(CurrentUser.Role, profileId, toRole);
            DbContext.SaveChanges();

            return Ok();
        }

        [HttpDelete("manage/archive/{profileId:int}")]
        public IActionResult ArchiveProfile([FromRoute] int profileId)
        {
            IdentitiesService.ArchiveProfile(CurrentUser.Role, profileId);
            DbContext.SaveChanges();

            return Ok();
        }

        #endregion
    }
}