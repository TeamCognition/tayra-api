using System;
using System.Linq;
using Cog.Core;
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
             // var o = Resolve<IOrganizationsService>();
             // o.Create(new OrganizationCreateDTO
             // {
             //     Key = "micetribe.tayra.io",
             //     Name = "MICEtribe",
             //     Timezone = "Central Europe Standard Time",
             //     DatabaseServer = "tayra-sqlserver.czyjrarofbip.eu-central-1.rds.amazonaws.com",
             //     DatabaseName = "tayra-tenant_micetribe",
             //     TemplateConnectionString = "User ID = admin; Password = Kr7N9#p!2AbR;Connect Timeout=100;Application Name=Tayra"
             // });

            // IdentitiesService.CreateInvitation(0, "micetribe.tayra.io", new IdentityInviteDTO
            // {
            //     EmailAddress = "info@contactless.io",
            //     FirstName = "Ahmed",
            //     LastName = "Admin",
            //     Role = ProfileRoles.Admin
            // });
            // DbContext.SaveChanges();
            //
            // IdentitiesService.InternalCreateWithProfile(dto);
            // DbContext.SaveChanges();

            return Ok();
        }

        [AllowAnonymous, HttpPost("join")]
        public IActionResult SendInvitation([FromBody] IdentityJoinDTO dto)
        {
            IdentitiesService.InvitationJoinWithSaveChanges(dto);

            return Ok();
        }

        [HttpPost("invitation")]
        public IActionResult SendInvitation([FromBody] IdentityInviteDTO dto)
        {
            IdentitiesService.CreateInvitation(CurrentUser.ProfileId, TenantProvider.GetTenant().Key, dto);

            DbContext.SaveChanges();
            return Ok();
        }
        
        [HttpPost("resendInvitation/{invitationId:int}")]
        public IActionResult ResendInvitation([FromRoute] int invitationId)
        {
            IdentitiesService.ResendInvitation(TenantProvider.GetTenant().Key, invitationId);

            DbContext.SaveChanges();
            return Ok();
        }

        [HttpPost("searchInvitations")]
        public ActionResult<GridData<IdentityInvitationGridDTO>> SearchInvitation([FromBody] IdentityInvitationGridParams gridParams)
        {
            return IdentitiesService.GetInvitationsGridData(gridParams);
        }

        [AllowAnonymous, HttpGet("invitation")]
        public ActionResult<IdentityInvitationViewDTO> GetInvitation([FromQuery] string invitationCode)
        {
            var invitation = IdentitiesService.GetInvitation(invitationCode);
            DbContext.SaveChanges();
            return invitation;
        }

        [HttpDelete("invitation/{invitationId:int}")]
        public IActionResult DeleteInvitation([FromRoute] int invitationId)
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
                throw new CogSecurityException("You don' have permission to segment " + gridParams.SegmentId);
            }

            return IdentitiesService.GetIdentityManageGridData(CurrentUser.ProfileId,CurrentUser.Role, gridParams);
        }

        [HttpGet("manage/assigns/{profileId:int}")]
        public ActionResult<IdentityManageAssignsDTO> GetManageTeamAssignData([FromRoute] int profileId)
        {
            if(profileId == CurrentUser.ProfileId)
            {
                throw new CogSecurityException("You can't assign yourself to a team or segment");
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