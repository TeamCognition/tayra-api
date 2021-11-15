using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using Cog.Core;
using Cog.DAL;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Tayra.Common;
using Tayra.Models.Catalog;
using Tayra.Models.Organizations;
using Tayra.Services;
using Tayra.Services._Models.Onboarding;
using Tayra.Services.Models.Profiles;

namespace Tayra.API.Features.IdentityManaging
{
    public class Join
    {
        public record Command : IRequest
        {
            public string InvitationCode { get; set; }
            public string Avatar { get; set; }
            public string Username { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }

            [MaxLength(100)]//this is not handled I think
            public string JobPosition { get; set; }
            public string Password { get; set; }
        }
        
        public class Handler : AsyncRequestHandler<Command>
        {
            private readonly OrganizationDbContext _db;
            private readonly CatalogDbContext _catalogDb;

            public Handler(OrganizationDbContext db, CatalogDbContext catalogDb) => (_db, _catalogDb) = (db, catalogDb);

            protected override async System.Threading.Tasks.Task Handle(Command msg, CancellationToken token)
            {
                var invitation = await _db.Invitations.FirstOrDefaultAsync(x => x.Code == Guid.Parse(msg.InvitationCode), token);

            invitation.EnsureNotNull(msg.InvitationCode);

            if (!invitation.IsActive())
            {
                throw new ApplicationException("Invitation already accepted.");
            }

            if (!await ValidateInvitationWithSaveChanges(_catalogDb, invitation))
            {
                throw new ApplicationException($"The invitation expired or is not valid anymore");
            }

            if (!invitation.IsActive())
            {
                throw new ApplicationException($"The invitation expired or is not valid anymore");
            }
            
            if (!new ProfilesService().IsUsernameUniqueNonAsync(_db, msg.Username))
            {
                throw new ApplicationException($"Username already exists");
            }

            if (!IdentityRules.IsPasswordValid(msg.Password))
            {
                throw new ApplicationException($"Invalid password");
            }

            var salt = PasswordHelper.GenerateSalt();

            var identity = _catalogDb.Add(new Identity
            {
                FirstName = msg.FirstName,
                LastName = msg.LastName,
                Salt = salt,
                Password = PasswordHelper.Hash(msg.Password, salt),
            }).Entity;

            _catalogDb.Add(new IdentityEmail
            {
                Email = invitation.EmailAddress,
                IsPrimary = true,
                Identity = identity,
                Created = DateTime.UtcNow
            });

            _catalogDb.Add(new TenantIdentity
            {
                Identity = identity,
                TenantId = _db.TenantInfo.Id
            });

            invitation.Status = InvitationStatus.Accepted;
            //get identity Id
            await _catalogDb.SaveChangesAsync(token);

            var profile = _db.Add(new Tayra.Models.Organizations.Profile
            {
                Avatar = msg.Avatar,
                FirstName = msg.FirstName,
                LastName = msg.LastName,
                Username = msg.Username,
                JobPosition = msg.JobPosition,
                Role = invitation.Role,
                IdentityId = identity.Id,
                IsAnalyticsEnabled = invitation.Role == ProfileRoles.Member
            }).Entity;

            _db.Add(new LogDevice
            {
                Profile = profile,
                Type = LogDeviceTypes.Email,
                Address = invitation.EmailAddress
            });

            if (profile.Role != ProfileRoles.Admin)
            {
                _db.Add(new ProfileAssignment
                {
                    Profile = profile,
                    SegmentId = invitation.SegmentId,
                    TeamId = invitation.TeamId,
                });
            }

            await _db.SaveChangesAsync(token);
            }
            
            private async System.Threading.Tasks.Task<bool> ValidateInvitationWithSaveChanges(CatalogDbContext catalogDb, Invitation invitation)
            {
                if (!invitation.IsActive())
                    return false;

                if (!await new Tayra.Services._Models.Identities.IdentitiesService().IsEmailAddressUnique(catalogDb, invitation.EmailAddress))
                {
                    invitation.Status = InvitationStatus.Expired;
                    catalogDb.SaveChanges();
                    return false;
                }

                return true;
            }
        }
    }
}