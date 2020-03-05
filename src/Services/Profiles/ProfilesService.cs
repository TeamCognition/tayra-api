using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Firdaws.Core;
using Firdaws.DAL;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Tayra.Common;
using Tayra.Mailer;
using Tayra.Models.Catalog;
using Tayra.Models.Organizations;

namespace Tayra.Services
{
    public class ProfilesService : BaseService<OrganizationDbContext>, IProfilesService
    {
        protected CatalogDbContext CatalogDb { get; set; }

        #region Constructor
        protected ILogsService LogsService { get; set; }

        public ProfilesService(ITokensService tokensService, ILogsService logsService, CatalogDbContext catalogDb, OrganizationDbContext dbContext) : base(dbContext)
        {
            LogsService = logsService;
            TokensService = tokensService;
            CatalogDb = catalogDb;
        }

        #endregion

        protected ITokensService TokensService;

        #region Public Methods

        public Profile GetByIdentityId(int identityId)
        {
            return DbContext.Profiles
                .FirstOrDefault(x => x.IdentityId == identityId);
        }

        public Profile GetByUsername(string username)
        {
            return DbContext.Profiles
                .FirstOrDefault(x => x.Username.ToLower() == username.ToLower());
        }

        public Profile GetByEmail(string email)
        {
            var ie = CatalogDb.IdentityEmails.FirstOrDefault(x => x.Email == email);

            ie.EnsureNotNull(email);

            return DbContext.Profiles.FirstOrDefault(x => x.IdentityId == ie.IdentityId);
        }

        public Profile GetByExternalId(string externalId, IntegrationType integrationType)
        {
            var pe = DbContext.ProfileExternalIds.Include(x => x.Profile).FirstOrDefault(x => x.ExternalId == externalId && x.IntegrationType == integrationType);

            pe.EnsureNotNull(externalId, integrationType);

            return pe.Profile;
        }

        public bool IsUsernameUnique(string username)
        {
            return IsUsernameUnique(DbContext, username);
        }

        public static bool IsUsernameUnique(OrganizationDbContext dbContext, string username)
        {
            return !dbContext.Profiles.Any(x => x.Username == username);
        }

        public int OneUpProfile(int profileId, ProfileOneUpDTO dto)
        {
            int? lastUppedAt = (from u in DbContext.ProfileOneUps
                                where u.CreatedBy == profileId
                                where u.UppedProfileId == dto.ProfileId
                                orderby u.DateId descending
                                select u.DateId
                                ).FirstOrDefault();


            if (!ProfileRules.CanUpProfile(profileId, dto.ProfileId, lastUppedAt))
            {
                throw new ApplicationException("Profile already upped by same user today");
            }

            DbContext.Add(new ProfileOneUp
            {
                UppedProfileId = dto.ProfileId,
                DateId = DateHelper2.ToDateId(DateTime.UtcNow)
            });

            var oneUpGiverUsername = DbContext.Profiles.Where(x => x.Id == profileId).Select(x => x.Username).FirstOrDefault();
            var oneUpReceiverUsername = DbContext.Profiles.Where(x => x.Id == dto.ProfileId).Select(x => x.Username).FirstOrDefault();
            LogsService.LogEvent(new LogCreateDTO
            {
                Event = LogEvents.ProfileOneUpGave,
                Data = new Dictionary<string, string>
                {
                    { "timestamp", DateTime.UtcNow.ToString() },
                    { "profileUsername", oneUpGiverUsername },
                    { "receiverUsername", oneUpReceiverUsername }
                },
                ProfileId = profileId,
            });

            LogsService.LogEvent(new LogCreateDTO
            {
                Event = LogEvents.ProfileOneUpReceived,
                Data = new Dictionary<string, string>
                {
                    { "timestamp", DateTime.UtcNow.ToString() },
                    { "profileUsername", oneUpReceiverUsername },
                    { "giverUsername", oneUpGiverUsername }
                },
                ProfileId = dto.ProfileId,
            });

            LogsService.SendLog(dto.ProfileId, LogEvents.ProfileOneUpReceived, new EmailOneUpReceivedDTO(oneUpGiverUsername));

            var totalUps = TokensService.CreateTransaction(TokenType.OneUp, dto.ProfileId, 1, TransactionReason.OneUpProfile, null);
            return Convert.ToInt32(totalUps);
        }

        public GridData<ProfileGridDTO> GetGridData(int profileId, ProfileGridParams gridParams)
        {
            IQueryable<Profile> scope = DbContext.Profiles.Where(x => x.Role == ProfileRoles.Member && x.Id != profileId);
            Expression<Func<Profile, bool>> byUsername = x => x.Username.Contains(gridParams.UsernameQuery.RemoveAllWhitespaces());
            Expression<Func<Profile, bool>> byName = x => (x.FirstName + x.LastName).Contains(gridParams.NameQuery.RemoveAllWhitespaces());

            if (gridParams.SegmentIdExclude.HasValue)
            {
                var profileIds = DbContext.ProfileAssignments.Where(x => x.SegmentId == gridParams.SegmentIdExclude).Select(x => x.ProfileId).ToList();
                scope = scope.Where(x => !profileIds.Contains(x.Id));
            }

            if (!string.IsNullOrEmpty(gridParams.UsernameQuery) && !string.IsNullOrEmpty(gridParams.NameQuery))
                scope = scope.Chain(ChainType.OR, byUsername, byName);
            else
            {
                if (!string.IsNullOrEmpty(gridParams.UsernameQuery))
                    scope = scope.Where(byUsername);

                if (!string.IsNullOrEmpty(gridParams.NameQuery))
                    scope = scope.Where(byName);
            }

            var query = from p in scope
                        select new ProfileGridDTO
                        {
                            ProfileId = p.Id,
                            Name = p.FirstName + " " + p.LastName,
                            Username = p.Username,
                            Avatar = p.Avatar,
                            Created = p.Created
                        };

            GridData<ProfileGridDTO> gridData = query.GetGridData(gridParams);

            return gridData;
        }

        public GridData<ProfileSummaryGridDTO> GetGridDataWithSummary(int profileId, ProfileSummaryGridParams gridParams)
        {
            IQueryable<Profile> scope = DbContext.Profiles.Where(x => x.Role == ProfileRoles.Member && x.Id != profileId);
            Expression<Func<Profile, bool>> byUsername = x => x.Username.Contains(gridParams.UsernameQuery.RemoveAllWhitespaces());
            Expression<Func<Profile, bool>> byName = x => (x.FirstName + x.LastName).Contains(gridParams.NameQuery.RemoveAllWhitespaces());

            //if (!string.IsNullOrEmpty(gridParams.SegmentKeyQuery))
            //{
            //    var segment = DbContext.Segments.FirstOrDefault(x => x.Key == gridParams.SegmentKeyQuery);
            //    var profileIds = DbContext.SegmentMembers.Where(x => x.SegmentId == segment.Id).Select(x => x.ProfileId).ToList();
            //    scope = scope.Where(x => profileIds.Contains(x.Id));
            //}

            if (!string.IsNullOrEmpty(gridParams.UsernameQuery) && !string.IsNullOrEmpty(gridParams.NameQuery))
                scope = scope.Chain(ChainType.OR, byUsername, byName);
            else
            {
                if (!string.IsNullOrEmpty(gridParams.UsernameQuery))
                    scope = scope.Where(byUsername);

                if (!string.IsNullOrEmpty(gridParams.NameQuery))
                    scope = scope.Where(byName);
            }

            var expTokenId = DbContext.Tokens.Where(x => x.Type == TokenType.Experience).Select(x => x.Id).First();
            var upsTokenId = DbContext.Tokens.Where(x => x.Type == TokenType.OneUp).Select(x => x.Id).First();
            var query = from p in scope
                        from prw in DbContext.ProfileReportsWeekly.Where(x => p.Id == x.ProfileId)
                            .OrderByDescending(x => x.DateId).Take(1).DefaultIfEmpty()
                        from title in DbContext.ProfileInventoryItems.Where(x => p.Id == x.ProfileId
                             && x.IsActive == true && x.ItemType == ItemTypes.TayraTitle).DefaultIfEmpty()
                        select new ProfileSummaryGridDTO
                        {
                            ProfileId = p.Id,
                            Name = p.FirstName + " " + p.LastName,
                            Username = p.Username,
                            Avatar = p.Avatar,
                            Title = title.Item.Name,
                            Teams = p.Teams.Select(x => new ProfileSummaryGridDTO.Team { Name = x.Team.Name, Key = x.Team.Key }).ToArray(),
                            OneUps = (int)p.Tokens.Where(x => x.TokenId == upsTokenId).Sum(x => x.Value),
                            CompletedChallenges = p.CompletedChallenges.Count(),
                            Speed = (float)Math.Round(prw.SpeedAverage, 2),
                            Power = (float)Math.Round(prw.PowerAverage, 2),
                            Impact = (float)Math.Round(prw.OImpactAverage, 2),
                            TokensTotal = (float)Math.Round(p.Tokens.Where(x => x.TokenId == expTokenId).Sum(x => x.Value), 2) //There might be a problem with this
                        };

            GridData<ProfileSummaryGridDTO> gridData = query.GetGridData(gridParams);

            return gridData;
        }

        public GridData<ProfileCompletedChallengesGridDTO> GetCompletedChallengesGridDTO(ProfileCompletedChallengesGridParams gridParams)
        {
            var query = from cc in DbContext.ChallengeCompletions
                        where cc.ProfileId == gridParams.ProfileId
                        select new ProfileCompletedChallengesGridDTO
                        {
                            ChallengeId = cc.ChallengeId,
                            ChallengeName = cc.Challenge.Name,
                            ChallengeImage = cc.Challenge.Image,
                            CompletedAt = cc.Created
                        };

            GridData<ProfileCompletedChallengesGridDTO> gridData = query.GetGridData(gridParams);

            return gridData;
        }

        public GridData<ProfileCommittedChallengesGridDTO> GetCommittedChallengesGridDTO(ProfileCommittedChallengesGridParams gridParams)
        {
            var query = from cc in DbContext.ChallengeCommits
                        where cc.ProfileId == gridParams.ProfileId
                        select new ProfileCommittedChallengesGridDTO
                        {
                            ChallengeId = cc.ChallengeId,
                            Name = cc.Challenge.Name,
                            Image = cc.Challenge.Image,
                            Status = cc.Challenge.Status,
                            CompletedAt = cc.CompletedAt,
                            CommittedAt = cc.Created
                        };

            GridData<ProfileCommittedChallengesGridDTO> gridData = query.GetGridData(gridParams);

            return gridData;
        }

        public ProfileUpdateDTO GetUpdateProfileData(int profileId)
        {
            return DbContext.Profiles.Where(x => x.Id == profileId).Select(x => new ProfileUpdateDTO
            {
                Avatar = x.Avatar,
                FirstName = x.FirstName,
                LastName = x.LastName,
                JobPosition = x.JobPosition,
                BornOn = x.BornOn,
                EmployedOn = x.EmployedOn,
                Username = x.Username
            }).FirstOrDefault();
        }

        public void UpdateProfile(int profileId, ProfileUpdateDTO dto)
        {
            var profile = DbContext.Profiles.FirstOrDefault(x => x.Id == profileId);

            profile.EnsureNotNull(profileId);

            profile.Avatar = dto.Avatar;
            profile.FirstName = dto.FirstName;
            profile.LastName = dto.LastName;
            profile.JobPosition = dto.JobPosition;
            profile.BornOn = dto.BornOn;
            profile.EmployedOn = dto.EmployedOn;
            profile.Username = dto.Username;
        }

        public ProfileRadarChartDTO GetProfileRadarChartDTO(int profileId)
        {
            var prw = DbContext.ProfileReportsWeekly
                                    .OrderByDescending(x => x.DateId)
                                    .Where(x => x.ProfileId == profileId)
                                    .Take(4).ToList();

            var profileWeeklyDateId = prw?.FirstOrDefault()?.DateId ?? int.MaxValue;
            var prd = DbContext.ProfileReportsDaily
                                .OrderByDescending(x => x.DateId)
                                .Where(x => x.DateId <= profileWeeklyDateId)
                                .FirstOrDefault(x => x.ProfileId == profileId);

            var tm = DbContext.ProfileAssignments.FirstOrDefault(x => x.ProfileId == profileId && x.TeamId.HasValue);

            var trw = new List<TeamReportWeekly>();
            if (tm != null)
            {
                trw = DbContext.TeamReportsWeekly
                            .OrderByDescending(x => x.DateId)
                            .Where(x => x.DateId <= profileWeeklyDateId && x.TeamId == tm.TeamId)
                            .Take(4).ToList();

            }

            if (!prw.Any()) //Average throws exception if count is 0
                prw = null;

            if (!trw.Any()) //Average throws exception if count is 0
                trw = null;

            return new ProfileRadarChartDTO
            {
                AssistsAverage = Math.Round(prw?.Average(x => x.AssistsTotalAverage) ?? 0, 2),
                AssistsTotal = prd?.AssistsTotal,

                TasksCompletedAverage = Math.Round(prw?.Average(x => x.TasksCompletedChange) ?? 0, 2),
                TasksCompletedTotal = prd?.TasksCompletedTotal,

                ComplexityAverage = Math.Round((prw?.Sum(x => x.ComplexityChange) ?? 0) / ((float)Math.Max(prw?.Sum(x => x.TasksCompletedChange) ?? 0, 1)) , 2),
                ComplexityTotal = prd?.ComplexityTotal,

                TeamAssistsAverage = Math.Round(trw?.Average(x => x.AssistsAverage) ?? 0, 2),
                TeamComplexityAverage = Math.Round((trw?.Sum(x => x.ComplexityChange) ?? 0) / ((float)Math.Max(trw?.Sum(x => x.TasksCompletedChange) ?? 0, 1)), 2),
                TeamTasksCompletedAverage = Math.Round(trw?.Average(x => x.TasksCompletedAverage) ?? 0, 2)
            };
        }

        public ProfileViewDTO GetProfileViewDTO(int profileId, Expression<Func<Profile, bool>> condition)
        {
            var profileDto = (from p in DbContext.Profiles.Where(condition)
                              let tt = p.Tokens.Where(x => !x.ClaimRequired || x.ClaimedAt.HasValue).GroupBy( //TokenScope
                              x => x.Token,
                              x => x,
                              (t, tnxs) => new ProfileViewDTO.TokenDTO { Name = t.Name, Type = t.Type, Value = tnxs.Sum(x => x.Value) })
                              select new ProfileViewDTO
                              {
                                  ProfileId = p.Id,
                                  FirstName = p.FirstName,
                                  LastName = p.LastName,
                                  Username = p.Username,
                                  Role = p.Role,
                                  Avatar = p.Avatar,
                                  Teams = p.Teams.Select(x => new ProfileViewDTO.TeamDTO { Key = x.Team.Key, Name = x.Team.Name }).ToArray(),
                                  CompanyTokens = Math.Round(tt.Where(x => x.Type == TokenType.CompanyToken).Select(x => x.Value).FirstOrDefault(), 2),
                                  Experience = Convert.ToInt32(tt.Where(x => x.Type == TokenType.Experience).Select(x => x.Value).FirstOrDefault()),
                                  OneUps = Convert.ToInt32(tt.Where(x => x.Type == TokenType.OneUp).Select(x => x.Value).FirstOrDefault()),
                                  CustomTokens = tt.Where(x => x.Type == TokenType.Custom).ToList(),
                              }).FirstOrDefault();

            profileDto.EnsureNotNull();

            if (profileId != profileDto.ProfileId)
            {
                profileDto.LastUppedAt = (from u in DbContext.ProfileOneUps
                                          where u.CreatedBy == profileId
                                          where u.UppedProfileId == profileDto.ProfileId
                                          orderby u.DateId descending
                                          select u.Created).FirstOrDefault();
            }
            var activeItems = GetProfileActiveItems(DbContext, profileDto.ProfileId);
            profileDto.Badges = activeItems.Badges;
            profileDto.Title = activeItems.Title;
            profileDto.Border = activeItems.Border;

            var weeklyStats = DbContext.ProfileReportsWeekly.OrderByDescending(x => x.DateId).Where(x => x.ProfileId == profileDto.ProfileId).Take(4).ToArray();
            var lastWeeklyStats = weeklyStats.FirstOrDefault();
            profileDto.Power = lastWeeklyStats?.PowerAverage;
            profileDto.Speed = lastWeeklyStats?.SpeedAverage;
            profileDto.OImpact = lastWeeklyStats?.OImpactAverage;
            profileDto.Heat = lastWeeklyStats == null ? null : new ProfileViewDTO.HeatDTO
            {
                LastDateId = lastWeeklyStats.DateId,
                Values = weeklyStats.Select(x => x.Heat).ToArray()
            };

            return profileDto;
        }

        public void ModifyTokens(ProfileRoles profileRole, ProfileModifyTokensDTO dto)
        {
            if (profileRole != ProfileRoles.Admin)
            {
                throw new FirdawsSecurityException("You are not allowed to perform this action!");
            }
            TokensService.CreateTransaction(TokenType.CompanyToken, dto.ProfileId, dto.TokenValue, TransactionReason.Manual, null);
        }

        public ProfileNotificationSettingsDTO GetNotificationSettings(int profileId)
        {
            var devices = DbContext.LogDevices.Where(x => x.ProfileId == profileId && x.Type == LogDeviceTypes.Email).Include(x => x.Settings).FirstOrDefault();

            return new ProfileNotificationSettingsDTO
            {
                Settings = devices.Settings.Select(x => new ProfileNotificationSettingsDTO.SettingDTO
                {
                    LogEvent = x.LogEvent,
                    IsEnabled = x.IsEnabled
                }).ToArray()
            };
        }

        public void UpdateNotificationSettings(int profileId, ProfileNotificationSettingsDTO dto)
        {
            var devices = DbContext.LogDevices.Where(x => x.ProfileId == profileId && x.Type == LogDeviceTypes.Email).Include(x => x.Settings).ToList();

            foreach(var d in devices)
            {
                foreach(var sdto in dto.Settings)
                { 
                    var s = d.Settings.Where(x => x.LogEvent == sdto.LogEvent).FirstOrDefault();
                    if(s == null)
                    {
                        s = new LogSetting
                        {
                            ProfileId = profileId,
                            LogDeviceId = d.Id,
                            LogEvent = sdto.LogEvent
                        };
                        d.Settings.Add(s);
                    }
                    s.IsEnabled = sdto.IsEnabled;
                }
            }
        }

        public ProfileActivityChartDTO[] GetProfileActivityChart(int profileId)
        {
            return DbContext.ProfileReportsDaily.Where(x => x.ProfileId == profileId && x.ActivityChartJson != null).OrderBy(x => x.DateId).Select(x => x.ActivityChartJson).Take(30).ToArray().Select(JsonConvert.DeserializeObject<ProfileActivityChartDTO>).ToArray();
        }


        #endregion

        #region Static methods

        public class ProfileActiveItemsDTO
        {
            public IList<ItemActiveDTO> Badges { get; set; }
            public ItemActiveDTO Title { get; set; }
            public ItemActiveDTO Border { get; set; }
        }

        public static ProfileActiveItemsDTO GetProfileActiveItems(OrganizationDbContext dbContext, int profileId)
        {
            var activeItems = (from ii in dbContext.ProfileInventoryItems
                               where ii.ProfileId == profileId
                               //where !ii.ClaimRequired || ii.ClaimedAt.HasValue
                               where ii.IsActive
                               select new ItemActiveDTO
                               {
                                   InventoryItemId = ii.Id,
                                   Name = ii.Item.Name,
                                   Image = ii.Item.Image,
                                   Type = ii.Item.Type,
                                   Rarity = ii.Item.Rarity
                               }).ToList();

            return new ProfileActiveItemsDTO
            {
                Badges = activeItems.Where(x => x.Type == ItemTypes.TayraBadge).ToList(),
                Title = activeItems.FirstOrDefault(x => x.Type == ItemTypes.TayraTitle),
                Border = activeItems.FirstOrDefault(x => x.Type == ItemTypes.TayraBorder)
            };
        }

        #endregion
    }
}