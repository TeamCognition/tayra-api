using System;
using System.Linq;
using System.Threading.Tasks;
using Cog.Core;
using Cog.DAL;
using Microsoft.EntityFrameworkCore;
using Tayra.Common;
using Tayra.Mailer;
using Tayra.Models.Catalog;
using Tayra.Models.Organizations;
using Task = System.Threading.Tasks.Task;

namespace Tayra.Services._Models.Identities
{
    public class IdentitiesService
    {
        public async Task SendInvitation(OrganizationDbContext db, CatalogDbContext catalogDb, string host, IdentityInviteDTO dto)
        {
            if (dto.TeamId.HasValue && !dto.SegmentId.HasValue)
            {
                throw new CogSecurityException("If teamId is sent, you must also send segmentId");
            }

            if (!await IsEmailAddressUnique(catalogDb, dto.EmailAddress))
            {
                throw new ApplicationException("Email address already used");
            }

            if (dto.SegmentId.HasValue && !db.Segments.Any(x => x.Id == dto.SegmentId))
            {
                throw new EntityNotFoundException<Segment>(dto.SegmentId);
            }

            if (dto.TeamId.HasValue && !db.Teams.Any(x => x.Id == dto.TeamId))
            {
                throw new EntityNotFoundException<Team>(dto.TeamId);
            }

            if (db.Invitations.Any(x => x.EmailAddress == dto.EmailAddress))
            {
                throw new ApplicationException("Active invitation with this email address already exists");
            }

            var invitation = new Invitation
            {
                Code = Guid.NewGuid(),
                EmailAddress = dto.EmailAddress,
                Role = dto.Role,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                SegmentId = dto.SegmentId,
                TeamId = dto.TeamId,
                Status = InvitationStatus.Sent
            };

            //invitation.TeamId = invitation.TeamId ?? DbContext.Teams.Where(x => x.SegmentId == invitation.SegmentId && x.Key == null).Select(x => x.Id).FirstOrDefault();
            //invitation.TeamId ??= DbContext.Teams.Where(x => x.SegmentId == invitation.SegmentId && x.Key == null).Select(x => x.Id).FirstOrDefault();

            var resp = MailerService.SendEmail(dto.EmailAddress, new EmailInviteDTO(host, invitation.Code.ToString()));
            if (resp.StatusCode != System.Net.HttpStatusCode.Accepted)
            {
                throw new ApplicationException(dto.EmailAddress + " email not sent");
            }

            db.Add(invitation);
        }
        
        public async Task<bool> IsEmailAddressUnique(CatalogDbContext catalogDb, string email)
        {
            return !(await catalogDb.IdentityEmails.AnyAsync(x => x.Email == email && x.DeletedAt == null));
        }
    }
}