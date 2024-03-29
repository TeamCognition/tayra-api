using System;
using System.Linq;
using System.Threading.Tasks;
using Cog.Core;
using Cog.DAL;
using Microsoft.EntityFrameworkCore;
using Tayra.Common;
using Tayra.Mailer;
using Tayra.Mailer.Templates.JoinTayra;
using Tayra.Models.Catalog;
using Tayra.Models.Organizations;
using Task = System.Threading.Tasks.Task;

namespace Tayra.Services._Models.Identities
{
    public class IdentitiesService
    {
        public async Task SendInvitation(OrganizationDbContext db, CatalogDbContext catalogDb, string host, IdentityInviteDTO dto, IMailerService mailerService)
        {
            if (!await IsEmailAddressUnique(catalogDb, dto.EmailAddress))
            {
                throw new ApplicationException("Email address already used");
            }

            if (!db.Segments.Any(x => x.Id == dto.SegmentId))
            {
                throw new EntityNotFoundException<Segment>(dto.SegmentId);
            }

            if (!db.Teams.Any(x => x.Id == dto.TeamId))
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

  //          var resp = EmailService.SendEmail(dto.EmailAddress, new EmailInviteDTO(host, invitation.Code.ToString()));
            var resp = mailerService.SendEmail(dto.EmailAddress, new TemplateModelJoinTayra("Join Tayra", dto.FirstName, host, invitation.Code.ToString(), dto.Role));

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