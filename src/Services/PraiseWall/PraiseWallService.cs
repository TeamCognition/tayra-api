using Firdaws.Core;
using Firdaws.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using Tayra.Common;
using Tayra.Mailer;
using Tayra.Models.Catalog;
using Tayra.Models.Organizations;

namespace Tayra.Services
{
    public class PraiseWallService : BaseService<OrganizationDbContext>, IPraiseWallService
    {
        #region Constructor
        protected ILogsService LogsService { get; set; }

        public PraiseWallService(ITokensService tokensService, ILogsService logsService, CatalogDbContext catalogDb, OrganizationDbContext dbContext) : base(dbContext)
        {
            LogsService = logsService;
            TokensService = tokensService;
            //CatalogDb = catalogDb;
        }

        #endregion

        protected ITokensService TokensService;

        #region Public Methods

        public void PraiseMember(int profileId, PraiseWallPraiseDTO dto)
        {
            int? lastPraisedAt = (from u in DbContext.ProfilePraises
                                  where u.CreatedBy == profileId
                                  where u.ProfileId == dto.ProfileId
                                  orderby u.DateId descending
                                  select u.DateId
                                ).FirstOrDefault();


            if (!ProfileRules.CanPraiseProfile(profileId, dto.ProfileId, lastPraisedAt, dto.Message))
            {
                throw new ApplicationException("Profile already praised by same user today");
            }

            DbContext.Add(new ProfilePraise
            {
                PraiserProfileId = profileId,
                ProfileId = dto.ProfileId,
                Type = dto.Type,
                DateId = DateHelper2.ToDateId(dto.DemoDate ?? DateTime.UtcNow),
                Message = dto.Message
            });

            var praiseGiverUsername = DbContext.Profiles.Where(x => x.Id == profileId).Select(x => x.Username).FirstOrDefault();
            var praiseReceiverUsername = DbContext.Profiles.Where(x => x.Id == dto.ProfileId).Select(x => x.Username).FirstOrDefault();

            LogsService.LogEvent(new LogCreateDTO
            {
                Event = LogEvents.ProfilePraiseGiven,
                Data = new Dictionary<string, string>
                {
                    { "timestamp", (dto.DemoDate ?? DateTime.UtcNow).ToString() },
                    { "profileUsername", praiseGiverUsername },
                    { "receiverUsername", praiseReceiverUsername },
                    { "type", dto.Type.ToString() }
                },
                ProfileId = profileId,
            });

            LogsService.LogEvent(new LogCreateDTO
            {
                Event = LogEvents.ProfilePraiseReceived,
                Data = new Dictionary<string, string>
                {
                    { "timestamp", (dto.DemoDate ?? DateTime.UtcNow).ToString() },
                    { "profileUsername", praiseReceiverUsername },
                    { "giverUsername", praiseGiverUsername },
                    { "type", dto.Type.ToString() }
                },
                ProfileId = dto.ProfileId,
            });

            LogsService.SendLog(dto.ProfileId, LogEvents.ProfilePraiseReceived, new EmailPraiseReceivedDTO(praiseGiverUsername));
        }
        
        public GridData<PraiseGridDTO> SearchPraises(PraiseSearchGridParams gridParams)
        {
            var query = from pp in DbContext.ProfilePraises
                        select new PraiseGridDTO
                        {
                            PraiserUsername = DbContext.Profiles.Where(x => x.Id == pp.PraiserProfileId).Select(x => x.Username).FirstOrDefault(),
                            RecieverUsername = pp.Profile.Username,
                            DateId = pp.DateId,
                            Type = pp.Type,
                            Message = pp.Message
                        };

            GridData<PraiseGridDTO> gridData = query.GetGridData(gridParams);

            return gridData;
        }

        #endregion

    }
}
