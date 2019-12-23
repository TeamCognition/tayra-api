using System;
using System.Linq;
using Firdaws.Core;
using Firdaws.DAL;
using Microsoft.EntityFrameworkCore;
using Tayra.Common;
using Tayra.Mailer;
using Tayra.Models.Catalog;
using Tayra.Models.Organizations;

namespace Tayra.Services
{
    public class IdentitiesService : BaseService<OrganizationDbContext>, IIdentitiesService
    {
        protected CatalogDbContext CatalogDb { get; set; }

        #region Constructor

        public IdentitiesService(CatalogDbContext catalogDb, OrganizationDbContext dbContext): base(dbContext)
        {
            CatalogDb = catalogDb;
        }

        #endregion

        #region Public Methods 

        //TODO: wrap with Transaction
        public void InternalCreateWithProfile(IdentityCreateDTO dto)
        {
            var emailTaken = CatalogDb.IdentityEmails.Any(x => x.Email == dto.Email);

            if (emailTaken)
            {
                throw new ApplicationException("Identity with same username already exists");
            }

            var salt = PasswordHelper.GenerateSalt();

            var identity = CatalogDb.Add(new Identity
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Salt = salt,
                Password = PasswordHelper.Hash(dto.Password, salt),
            }).Entity;

            CatalogDb.Add(new IdentityEmail
            {
                Email = dto.Email,
                IsPrimary = true,
                Identity = identity
            });

            CatalogDb.Add(new TenantIdentity
            {
                Identity = identity,
                TenantId = TenantUtilities.ConvertShardingKeyToTenantId(TenantUtilities.GenerateShardingKey(dto.TenantHost))
            }) ;

            //get identity Id
            CatalogDb.SaveChanges();

            var profile = DbContext.Add(new Profile
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Nickname = dto.Profile.Nickname,
                Role = dto.Profile.Role,
                Avatar = dto.Profile.Avatar,
                IdentityId = identity.Id
            }).Entity;

            //var project = DbContext.Projects.FirstOrDefault();
            //DbContext.Add(new ProjectMember
            //{
            //    Profile = profile,
            //    ProjectId = project.Id,

            //});

            DbContext.SaveChanges();
            
        }

        public Identity GetByEmail(string email) //TODO: this should be used by Auth.ResourceOwnerValidator?
        {
            return CatalogDb.IdentityEmails
                .Include(x => x.Identity)
                .FirstOrDefault(x => x.Email == email)
                .Identity;
        }

        public void InvitationJoinWithSaveChanges(IdentityJoinDTO dto)
        {
            var invitation = DbContext.Invitations.FirstOrDefault(x => x.Code == Guid.Parse(dto.InvitationCode) && x.IsActive);

            invitation.EnsureNotNull(dto.InvitationCode);

            ValidateInvitationWithSaveChanges(invitation);

            if(!ProfilesService.IsUsernameUnique(DbContext, dto.Username))
            {
                throw new ApplicationException($"Username already exists");
            }

            if (!IdentityRules.IsPasswordValid(dto.Password))
            {
                throw new ApplicationException($"Invalid password");
            }

            var salt = PasswordHelper.GenerateSalt();

            var identity = CatalogDb.Add(new Identity
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Salt = salt,
                Password = PasswordHelper.Hash(dto.Password, salt),
            }).Entity;

            CatalogDb.Add(new IdentityEmail
            {
                Email = invitation.EmailAddress,
                IsPrimary = true,
                Identity = identity
            });

            CatalogDb.Add(new TenantIdentity
            {
                Identity = identity,
                TenantId = TenantUtilities.ConvertShardingKeyToTenantId(DbContext.OrganizationId)
            });

            invitation.Status = InvitationStatus.Accepted;
            //get identity Id
            CatalogDb.SaveChanges();

            var profile = DbContext.Add(new Profile
            {
                Avatar = dto.Avatar,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Nickname = dto.Username,
                JobPosition = dto.JobPosition,
                Role = invitation.Role,
                IdentityId = identity.Id
            }).Entity;

            if (invitation.ProjectId.HasValue)
            {
                DbContext.Add(new ProjectMember
                {
                    Profile = profile,
                    ProjectId = invitation.ProjectId.Value,
                });
            }

            if (invitation.TeamId.HasValue)
            {
                DbContext.Add(new TeamMember
                {
                    Profile = profile,
                    TeamId = invitation.TeamId.Value,
                });
            }

            DbContext.SaveChanges();
        }

        public void CreateInvitation(int profileId, string host, IdentityInviteDTO dto)
        {
            var invitation = new Invitation
            {
                Code = Guid.NewGuid(),
                EmailAddress = dto.EmailAddress,
                Role = dto.Role,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                ProjectId = dto.ProjectId,
                TeamId = dto.TeamId,
                Status = InvitationStatus.Sent
            };

            if(!IsEmailAddressUnique(dto.EmailAddress))
            {
                throw new ApplicationException("Email address already used");
            }

            if (dto.ProjectId.HasValue && !DbContext.Projects.Any(x => x.Id == dto.ProjectId && x.ArchivedAt == null))
            {
                throw new EntityNotFoundException<Project>(dto.ProjectId);
            }

            if (dto.TeamId.HasValue && !DbContext.Teams.Any(x => x.Id == dto.TeamId && x.ArchivedAt == null))
            {
                throw new EntityNotFoundException<Team>(dto.TeamId);
            }

            var resp = MailerService.SendEmail(dto.EmailAddress, new EmailInviteDTO(host, invitation.Code.ToString()));
            if(resp.StatusCode != System.Net.HttpStatusCode.Accepted)
            {
                throw new ApplicationException(dto.EmailAddress + " email not sent");
            }

            DbContext.Add(invitation);
        }

        public IdentityInvitationViewDTO GetInvitation(string InvitationCode)
        {
            var invitation = DbContext.Invitations.Where(x => x.Code == Guid.Parse(InvitationCode) && x.IsActive)
                        .Select(x => new IdentityInvitationViewDTO
                        {
                            EmailAddress = x.EmailAddress,
                            FirstName = x.FirstName,
                            LastName = x.LastName,
                            Status = x.Status
                        }).FirstOrDefault();

            invitation.EnsureNotNull(InvitationCode);

            if(invitation.Status == InvitationStatus.Sent)
            {
                invitation.Status = InvitationStatus.Viewed;
            }

            DbContext.SaveChanges();

            return invitation;
        }

        public GridData<IdentityInvitationGridDTO> GetInvitationsGridData(IdentityInvitationGridParams gridParams)
        {
            IQueryable<IdentityInvitationGridDTO> query = from i in DbContext.Invitations
                                                          where i.IsActive == !gridParams.ActiveStatusesOnly
                                                          select new IdentityInvitationGridDTO
                                                          {
                                                              EmailAddress =  i.EmailAddress,
                                                              Status = i.Status,
                                                              FirstName = i.FirstName,
                                                              LastName = i.LastName,
                                                              Created = i.Created
                                                          };

            GridData<IdentityInvitationGridDTO> gridData = query.GetGridData(gridParams);
            return gridData;
        }

        public GridData<IdentityEmailsGridDTO> GetIdentityEmailsGridData(int profileId, IdentityEmailsGridParams gridParams)
        {
            var identityId = DbContext.Profiles
                .Where(x => x.Id == profileId)
                .Select(x => x.IdentityId)
                .FirstOrDefault();

            var scope = CatalogDb.IdentityEmails
                .Where(x => x.IdentityId == identityId);

            IQueryable<IdentityEmailsGridDTO> query = from e in scope
                                                      select new IdentityEmailsGridDTO
                                                      {
                                                          EmailAddress = e.Email,
                                                          IsPrimary = e.IsPrimary,
                                                          AddedOn = e.Created
                                                      };

            GridData<IdentityEmailsGridDTO> gridData = query.GetGridData(gridParams);
            return gridData;
        }

        public bool IsEmailAddressUnique(string email)
        {
            return !CatalogDb.IdentityEmails.Any(x => x.Email == email && x.DeletedAt == null);
        }

        public void AddEmail(int identityId, string email)
        {
            var scope = CatalogDb.IdentityEmails.Where(x => x.DeletedAt != null);

            if (scope.Where(x => x.Email == email).Any())
            {
                throw new ApplicationException("Email already in use");
            }

            var emailEntry = new IdentityEmail
            {
                IdentityId = identityId,
                Email = email,
                IsPrimary = !scope.Where(x => x.IdentityId == identityId).Any()
            };

            CatalogDb.IdentityEmails.Add(emailEntry);
            CatalogDb.SaveChanges();
        }

        #region Email Methods 

        public void SetPrimaryEmail(int identityId, string email)
        {
            var scope = CatalogDb.IdentityEmails.Where(x => x.DeletedAt != null);

            var emails = scope
                .Where(x => x.IdentityId == identityId)
                .ToList();

            var emailEntry = emails.FirstOrDefault(x => x.Email == email);
            if (emailEntry == null)
            {
                throw new ApplicationException("Email " + email + " is not used");
            }

            emails.ForEach(x => x.IsPrimary = false);
            emailEntry.IsPrimary = true;

            CatalogDb.SaveChanges();
        }

        public bool RemoveEmail(int identityId, string email)
        {
            var scope = CatalogDb.IdentityEmails.Where(x => x.IdentityId == identityId && x.DeletedAt != null);
            var identityEmail = scope.Where(x => x.Email == email).FirstOrDefault();

            if(identityEmail.IsPrimary)
            {
                throw new ApplicationException("You can't remove primary email address");
            }

            if (scope.Count() <= 1)
            {
                throw new ApplicationException("You must have at least one email address");
            }

            identityEmail.DeletedAt = DateTime.UtcNow;
            var affectedRecords = CatalogDb.SaveChanges();
            return affectedRecords > 0;
        }

        #endregion

        #region Private methods

        private bool ValidateInvitationWithSaveChanges(Invitation invitation)
        {
            if (!invitation.IsActive)
                return false;

            if(!IsEmailAddressUnique(invitation.EmailAddress))
            {
                invitation.Status = InvitationStatus.Expired;
                DbContext.SaveChanges();
                return false;
            }

            return true;
        }

        #endregion

        #endregion
    }
}