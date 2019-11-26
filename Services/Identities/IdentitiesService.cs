using System;
using System.Linq;
using Firdaws.Core;
using Firdaws.DAL;
using Microsoft.EntityFrameworkCore;
using Tayra.Models.Catalog;
using Tayra.Models.Organizations;


namespace Tayra.Services
{
    public class IdentitiesService : IIdentitiesService
    {
        protected CatalogDbContext CatalogDb { get; set; }

        #region Constructor

        public IdentitiesService(CatalogDbContext catalogDb)
        {
            CatalogDb = catalogDb;
        }

        #endregion

        #region Public Methods 

        public void InternalCreateWithProfile(IdentityCreateDTO dto)
        {
            var doesExist = CatalogDb.Identities.Any(x => x.Username == dto.Username);

            if (doesExist)
            {
                throw new ApplicationException("Identity with same username already exists");
            }

            var salt = PasswordHelper.GenerateSalt();

            var identity = CatalogDb.Add(new Identity
            {
                Username = dto.Username,
                Salt = salt,
                Password = PasswordHelper.Hash(dto.Password, salt),
            }).Entity;

            //get identity Id
            CatalogDb.SaveChanges();
            var tenant = CatalogDb.Tenants.FirstOrDefault();

            var shardingKey = TenantUtilities.GenerateShardingKey(tenant.Name);


            using (var DbContext = new OrganizationDbContext(NewSharding.ShardMap, shardingKey, "User ID = tyradmin; Password = Kr7N9#p!2AbR;Connect Timeout=100;Application Name=Tayra"))
            {
                var profile = DbContext.Add(new Profile
                {
                    FirstName = dto.Profile.FirstName,
                    LastName = dto.Profile.LastName,
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
        }

        public Identity GetByEmail(string email)
        {
            return CatalogDb.IdentityEmails
                .Include(x => x.Identity)
                .FirstOrDefault(x => x.Email == email)
                .Identity;
        }

        public GridData<IdentityEmailsGridDTO> GetIdentityEmailsGridData(int profileId, IdentityEmailsGridParams gridParams)
        {
            var DbContext = new OrganizationDbContext(NewSharding.ShardMap, 1, "");
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
            var scope = CatalogDb.IdentityEmails.Where(x => x.DeletedAt != null);
            var emailEntry = scope.Where(x => x.IdentityId == identityId && x.Email == email).FirstOrDefault();

            CatalogDb.IdentityEmails.Remove(emailEntry);
            var affectedRecords = CatalogDb.SaveChanges();
            return affectedRecords > 0;
        }

        #endregion

        #endregion
    }
}