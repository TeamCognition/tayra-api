using System;
using System.Collections.Generic;
using System.Linq;
using Cog.Core;
using Cog.DAL;
using Tayra.Common;
using Tayra.Mailer;
using Tayra.Mailer.MailerTemplateModels;
using Tayra.Models.Organizations;

namespace Tayra.Services
{
    public class PraiseService : BaseService<OrganizationDbContext>, IPraiseService
    {
        #region Constructor
        protected ILogsService LogsService { get; set; }

        public PraiseService(ITokensService tokensService, ILogsService logsService, OrganizationDbContext dbContext) : base(dbContext)
        {
            LogsService = logsService;
            TokensService = tokensService;
        }

        #endregion

        protected ITokensService TokensService;

        #region Public Methods

        public void PraiseProfile(Guid profileId, PraiseProfileDTO dto)
        {
            int? lastPraisedAt = (from u in DbContext.ProfilePraises
                                  where u.CreatedBy == profileId
                                  where u.ProfileId == dto.ProfileId
                                  orderby u.DateId descending
                                  select u.DateId
                                ).FirstOrDefault();


            if (!CanPraiseProfile(profileId, dto.ProfileId, lastPraisedAt, dto.Message))
            {
                throw new ApplicationException("Profile already praised by same user today");
            }

            bool CanPraiseProfile(Guid upperId, Guid profileToUpId, int? lastUppedAt, string message)
            {
                return upperId != profileToUpId
                       && (!lastUppedAt.HasValue || DateHelper2.ToDateId(DateTime.UtcNow) > lastUppedAt)
                       && (string.IsNullOrEmpty(message) || message.Length <= 140);
            }
            
            DbContext.Add(new ProfilePraise
            {
                PraiserProfileId = profileId,
                ProfileId = dto.ProfileId,
                Type = dto.Type,
                DateId = DateHelper2.ToDateId(dto.DemoDate ?? DateTime.UtcNow),
                Message = dto.Message
            });

            var praiseGiver = DbContext.Profiles.FirstOrDefault(x => x.Id == profileId);
            var praiseReceiver = DbContext.Profiles.FirstOrDefault(x => x.Id == dto.ProfileId);
            
            LogsService.LogEvent(new LogCreateDTO
            (
                eventType: LogEvents.ProfilePraiseGiven,
                timestamp: dto.DemoDate ?? DateTime.UtcNow,
                description: null,
                externalUrl: null,
                data: new Dictionary<string, string>
                {
                    { "receiverUsername", praiseReceiver?.Username },
                    { "receiverName", praiseReceiver?.FirstName + " " + praiseReceiver?.LastName},
                    { "praiseType", dto.Type.ToString() }
                },
                profileId: profileId
            ));
            
            LogsService.LogEvent(new LogCreateDTO
            (
                eventType: LogEvents.ProfilePraiseReceived,
                timestamp: dto.DemoDate ?? DateTime.UtcNow,
                description: null,
                externalUrl: null,
                data: new Dictionary<string, string>
                {
                    { "giverUsername", praiseGiver?.Username },
                    { "giverName", praiseGiver?.FirstName + " " + praiseGiver?.LastName},
                    { "praiseType", dto.Type.ToString() }
                },
                profileId: profileId
            ));

            LogsService.SendLog(dto.ProfileId, LogEvents.ProfilePraiseReceived, 
                new PraiseReceivedTemplateModel("Praise Recieved",
                    praiseReceiver?.Username,
                    praiseGiver?.Username, "put link here",
                    dto.Type));
        }

        public GridData<PraiseSearchGridDTO> SearchPraises(PraiseGridParams gridParams)
        {
            IQueryable<ProfilePraise> scope;

            if (gridParams.ProfileId.HasValue)
                scope = DbContext.ProfilePraises.Where(x => x.ProfileId == gridParams.ProfileId);
            else
                scope = DbContext.ProfilePraises;

            var query = from pp in scope
                        select new PraiseSearchGridDTO
                        {
                            PraiserUsername = DbContext.Profiles.Where(x => x.Id == pp.PraiserProfileId).Select(x => x.Username).FirstOrDefault(),
                            RecieverUsername = pp.Profile.Username,
                            RecieverAvatar = pp.Profile.Avatar,
                            Created = pp.Created,
                            Type = pp.Type,
                            Message = pp.Message
                        };

            GridData<PraiseSearchGridDTO> gridData = query.GetGridData(gridParams);

            return gridData;
        }

        public GridData<PraiseSearchProfilesDTO> SearchProfiles(PraiseProfileSearchGridParams gridParams)
        {

            var query = from p in DbContext.Profiles
                        where p.Username.Contains(gridParams.UsernameQuery) || (p.FirstName + p.LastName).Contains(gridParams.NameQuery)

                        select new PraiseSearchProfilesDTO
                        {
                            ProfileId = p.Id,
                            Name = p.FirstName + " " + p.LastName,
                            Username = p.Username,
                            Avatar = p.Avatar
                        };

            GridData<PraiseSearchProfilesDTO> gridData = query.GetGridData(gridParams);

            return gridData;
        }

        #endregion
    }
}
