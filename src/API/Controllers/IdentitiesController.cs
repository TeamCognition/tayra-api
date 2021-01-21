using System;
using System.Linq;
using Cog.Core;
using Finbuckle.MultiTenant;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tayra.Common;
using Tayra.Models.Catalog;
using Tayra.Models.Organizations;
using Tayra.Services;

namespace Tayra.API.Controllers
{
    public class IdentitiesController : BaseController
    {
        #region Constructor

        public IdentitiesController(OrganizationDbContext dbContext,
            IServiceProvider serviceProvider) : base(serviceProvider)
        { 
            DbContext = dbContext;
        }

        #endregion

        #region Properties
        
        public readonly OrganizationDbContext DbContext;

        #endregion

        #region Action Methods
        
        [AllowAnonymous, HttpPost("create")]
        public ActionResult Create([FromBody] IdentityCreateDTO dto)
        {
            // var o = Resolve<IOrganizationsService>();
            // o.Create(new OrganizationCreateDTO
            // {
            //     Key = "deverino.tayra.local",
            //     Name = "Tayra Haris",
            //     Timezone = "Central Europe Standard Time",
            //     DatabaseServer = "localhost",
            //     DatabaseName = "tayra_tenant-deverino",
            //     TemplateConnectionString = "User ID=sa;Password=strong!Password;Connect Timeout=100;Application Name=Tayra"
            // });

            // IdentitiesService.CreateInvitation("deverino.tayra.local", new IdentityInviteDTO
            // {
            //     EmailAddress = "androvana+fejkara@gmail.com",
            //     FirstName = "Haris",
            //     LastName = "Botuloza",
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
        
        [AllowAnonymous, HttpPost("join2")]
        public IActionResult Join([FromBody] IdentityJoinDTO dto)
        {
            IdentitiesService.InvitationJoinWithSaveChanges(dto);

            return Ok();
        }

        [HttpPost("invitation")]
        public IActionResult SendInvitation([FromBody] IdentityInviteDTO dto)
        {
            IdentitiesService.CreateInvitation(HttpContext.GetMultiTenantContext<Tenant>()?.TenantInfo.Identifier, dto);

            DbContext.SaveChanges();
            return Ok();
        }

        [HttpPost("resendInvitation/{invitationId:int}")]
        public IActionResult ResendInvitation([FromRoute] Guid invitationId)
        {
            IdentitiesService.ResendInvitation(HttpContext.GetMultiTenantContext<Tenant>()?.TenantInfo.Identifier, invitationId);

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
            Console.WriteLine(invitationCode);
            var invitation = IdentitiesService.GetInvitation(invitationCode);
            DbContext.SaveChanges();
            return invitation;
        }

        [HttpDelete("invitation/{invitationId:int}")]
        public IActionResult DeleteInvitation([FromRoute] Guid invitationId)
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

            return IdentitiesService.GetIdentityManageGridData(CurrentUser.ProfileId, CurrentUser.Role, gridParams);
        }

        [HttpGet("manage/assigns/{profileId:int}")]
        public ActionResult<IdentityManageAssignsDTO> GetManageTeamAssignData([FromRoute] Guid profileId)
        {
            if (profileId == CurrentUser.ProfileId)
            {
                throw new CogSecurityException("You can't assign yourself to a team or segment");
            }

            return IdentitiesService.GetIdentityManageAssignsData(CurrentUser.SegmentsIds, profileId);
        }

        [HttpPut("manage/changeRole/{profileId:int}")]
        public IActionResult ChangeProfile([FromRoute] Guid profileId, [FromBody] ProfileRoles toRole)
        {
            IdentitiesService.ChangeProfileRole(CurrentUser.Role, profileId, toRole);
            DbContext.SaveChanges();

            return Ok();
        }

        [HttpDelete("manage/archive/{profileId:int}")]
        public IActionResult ArchiveProfile([FromRoute] Guid profileId)
        {
            IdentitiesService.ArchiveProfile(CurrentUser.Role, profileId);
            DbContext.SaveChanges();

            return Ok();
        }

        #endregion
    }
}