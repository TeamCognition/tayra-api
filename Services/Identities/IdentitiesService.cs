using System;
using System.Collections.Generic;
using System.Linq;
using Firdaws.Core;
using Firdaws.DAL;
using Microsoft.EntityFrameworkCore;
using Tayra.Models.Organizations;


namespace Tayra.Services
{
    public class IdentitiesService : BaseService<OrganizationDbContext>, IIdentitiesService
    {
        #region Constructor

        public IdentitiesService(OrganizationDbContext dbContext) : base(dbContext)
        {
        }

        #endregion

        #region Public Methods 

        public void Create(IdentityCreateDTO dto)
        {
            var doesExist = DbContext.Identities.Any(x => x.Username == dto.Username);

            if(doesExist)
            {
                throw new ApplicationException("Identity with same username already exists");
            }

            var salt = PasswordHelper.GenerateSalt();

            var identity = DbContext.Add(new Identity
            {
                Username = dto.Username,
                Salt = salt,
                Password = PasswordHelper.Hash(dto.Password, salt),
                Profiles = new List<Profile>
                {
                    new Profile
                    {
                        FirstName = dto.Profile.FirstName,
                        LastName = dto.Profile.LastName,
                        Nickname = dto.Profile.Nickname,
                        Role = dto.Profile.Role,
                        Avatar = dto.Profile.Avatar
                    }
                }
            }).Entity;

            var project = DbContext.Projects.FirstOrDefault();
            DbContext.Add(new ProjectMember
            {
                Profile = identity.Profiles.First(),
                ProjectId = project.Id,
                
            });
        }

        public Identity GetById(int identityId)
        {
            return DbContext.Identities
                .Include(x => x.Profiles)
                .FirstOrDefault(x => x.Id == identityId);
        }

        public Identity GetByUsername(string username)
        {
            return DbContext.Identities
               .Include(x => x.Profiles)
               .FirstOrDefault(x => x.Username == username);
        }

        public Identity GetByEmail(string email)
        {
            return DbContext.IdentityEmails
                .Include(x => x.Identity)
                .FirstOrDefault(x => x.Email == email)
                .Identity;
        }

        public GridData<IdentityEmailsGridDTO> GetIdentityEmailsGridData(int profileId, IdentityEmailsGridParams gridParams)
        {
            var identity = DbContext.Profiles
                .Where(x => x.Id == profileId)
                .Select(x => x.Identity)
                .FirstOrDefault();

            var scope = DbContext.IdentityEmails
                .Where(x => x.IdentityId == identity.Id);

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

        public void AddEmail(int identityId, string email)
        {
            var scope = DbContext.IdentityEmails.Where(x => x.DeletedAt != null);

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

            DbContext.IdentityEmails.Add(emailEntry);
            DbContext.SaveChanges();
        }

        #region Email Methods 

        public void SetPrimaryEmail(int identityId, string email)
        {
            var scope = DbContext.IdentityEmails.Where(x => x.DeletedAt != null);

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

            DbContext.SaveChanges();
        }

        public bool RemoveEmail(int identityId, string email)
        {
            var scope = DbContext.IdentityEmails.Where(x => x.DeletedAt != null);
            var emailEntry = scope.Where(x => x.IdentityId == identityId && x.Email == email).FirstOrDefault();

            DbContext.IdentityEmails.Remove(emailEntry);
            var affectedRecords = DbContext.SaveChanges();
            return affectedRecords > 0;
        }

        #endregion

        #endregion
    }
}