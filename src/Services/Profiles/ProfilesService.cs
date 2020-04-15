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

        public Profile GetMemberByExternalId(string externalId, IntegrationType integrationType)
        {
            var pe = DbContext.ProfileExternalIds.Include(x => x.Profile).FirstOrDefault(x => x.ExternalId == externalId && x.IntegrationType == integrationType && x.Profile.Role == ProfileRoles.Member);

            //pe.EnsureNotNull(externalId, integrationType);
            if (pe == null)
            {
                return null;
            }

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

            LogsService.SendLog(dto.ProfileId, LogEvents.ProfileOneUpReceived, new EmailPraiseReceivedDTO(oneUpGiverUsername));

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
   
            var upsTokenId = DbContext.Tokens.Where(x => x.Type == TokenType.OneUp).Select(x => x.Id).First();
            var query = from p in scope
                        from title in DbContext.ProfileInventoryItems.Where(x => p.Id == x.ProfileId
                             && x.IsActive == true && x.ItemType == ItemTypes.TayraTitle).DefaultIfEmpty()
                        let tt = p.Tokens.Where(x => !x.ClaimRequired || x.ClaimedAt.HasValue).GroupBy( //TokenScope
                         x => x.Token,
                         x => x,
                         (t, tnxs) => new ProfileViewDTO.TokenDTO { Name = t.Name, Type = t.Type, Value = tnxs.Sum(x => x.Value) })
                        select new ProfileSummaryGridDTO
                        {
                            ProfileId = p.Id,
                            Name = p.FirstName + " " + p.LastName,
                            Username = p.Username,
                            Avatar = p.Avatar,
                            PersonalInfo = new ProfileSummaryGridDTO.PersonalData
                            {
                                JobPosition = p.JobPosition,
                                EmployedOn = p.EmployedOn,
                                JoinDate = p.Created
                            },
                            PlatformInfo = new ProfileSummaryGridDTO.PlatformData
                            {
                                TotalTokens = (float)Math.Round(tt.Where(x => x.Type == TokenType.CompanyToken).Select(x => x.Value).FirstOrDefault(), 2),
                                Praises = (int)p.Tokens.Where(x => x.TokenId == upsTokenId).Sum(x => x.Value),
                                CompletedChallenges = p.CompletedChallenges.Count()
                            },
                            Segments = p.Assignments.Select(x => new ProfileSummaryGridDTO.Segment
                            {
                                Key = x.Segment.Key,
                                Name = x.Segment.Name,
                                JoinDate = x.Created
                            }).ToArray(),
                            Teams = p.Assignments.Where(x => x.TeamId != null).Select(x => new ProfileSummaryGridDTO.Team
                            {
                                Name = x.Team.Name,
                                Key = x.Team.Key,
                                JoinDate = x.Created
                            }).ToArray(),
                            Integrations = p.Integrations.Select(x => new ProfileSummaryGridDTO.Integration
                            {
                                Type = x.Type,
                                IntegratedOn = x.Created
                            }).Distinct().ToArray(),
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
            var prd = (from r in DbContext.ProfileReportsDaily
                       where r.ProfileId == profileId
                       group r by 1 into g
                       select new
                       {
                           AssistsAverage = g.Average(x => x.AssistsChange),
                           TasksCompletedAverage = g.Average(x => x.TasksCompletedChange),
                           ComplexityAverage = g.Average(x => x.ComplexityChange)
                       }).FirstOrDefault();

            var tm = DbContext.ProfileAssignments.FirstOrDefault(x => x.ProfileId == profileId && x.TeamId.HasValue);

            var trw = new { AssistsAverage = 0d, TasksCompletedAverage = 0d, ComplexityAverage = 0d };
            if (tm != null)
            {
                trw = (from r in DbContext.TeamReportsWeekly
                       where r.TeamId == tm.TeamId
                       group r by 1 into g
                       select new
                       {
                           AssistsAverage = g.Average(x => x.AssistsChange),
                           TasksCompletedAverage = g.Average(x => x.TasksCompletedChange),
                           ComplexityAverage = g.Average(x => x.ComplexityChange)
                       }).FirstOrDefault();

            };

            return new ProfileRadarChartDTO
            {
                AssistsAverage = Math.Round(prd?.AssistsAverage ?? 0f, 2),
                AssistsTotal = 0,

                TasksCompletedAverage = Math.Round(prd?.TasksCompletedAverage ?? 0f, 2),
                TasksCompletedTotal = 0,

                ComplexityAverage = Math.Round(prd?.ComplexityAverage ?? 0f, 2),
                ComplexityTotal = 0,

                TeamAssistsAverage = Math.Round(trw?.AssistsAverage ?? 0f, 2),
                TeamComplexityAverage = Math.Round(trw?.ComplexityAverage ?? 0f, 2),
                TeamTasksCompletedAverage = Math.Round(trw?.TasksCompletedAverage ?? 0f, 2),
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
                                  Teams = p.Assignments.Select(x => new ProfileViewDTO.TeamDTO { Key = x.Team.Key, Name = x.Team.Name }).ToArray(),
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

            var lastWeeklyStats = (from r in DbContext.ProfileReportsWeekly
                                   where r.ProfileId == profileDto.ProfileId
                                   group r by r.DateId into g
                                   orderby g.Key descending
                                   select new
                                   {
                                       DateId = g.Select(x => x.DateId).First(),
                                       PowerAverage = g.Average(x => x.PowerAverage),
                                       SpeedAverage = g.Average(x => x.SpeedAverage),
                                       OImpactAverage = g.Average(x => x.OImpactAverage),
                                       HeatTrend = g.Select(x => Math.Round(x.Heat, 2)).Take(4).ToArray()
                                   }).FirstOrDefault();

            profileDto.Power = Math.Round(lastWeeklyStats?.PowerAverage ?? 0d, 2);
            profileDto.Speed = Math.Round(lastWeeklyStats?.SpeedAverage ?? 0d, 2);
            profileDto.OImpact = Math.Round(lastWeeklyStats?.OImpactAverage ?? 0d, 2);

            //var heatStats = DbContext.ProfileReportsWeekly.OrderByDescending(x => x.DateId).Where(x => x.ProfileId == profileDto.ProfileId).GroupBy(x => x.ProfileId)
            profileDto.Heat = lastWeeklyStats == null ? null : new ProfileViewDTO.HeatDTO
            {
                LastDateId = lastWeeklyStats.DateId,
                Values = lastWeeklyStats.HeatTrend
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

            foreach (var d in devices)
            {
                foreach (var sdto in dto.Settings)
                {
                    var s = d.Settings.Where(x => x.LogEvent == sdto.LogEvent).FirstOrDefault();
                    if (s == null)
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
            var oldestDateId = DateHelper2.ToDateId(DateTime.UtcNow.AddDays(-31));
            return (from r in DbContext.ProfileReportsDaily
                    where r.ProfileId == profileId && r.ActivityChartJson != null
                    where r.DateId >= oldestDateId
                    orderby r.DateId
                    select new
                    {
                        DateId = r.DateId,
                        ActivityChart = JsonConvert.DeserializeObject<ProfileActivityChartDTO>(r.ActivityChartJson)
                    }).ToArray()
                    .GroupBy(x => x.DateId)
                    .Select(g => new ProfileActivityChartDTO
                    {
                        DateId = g.Key,
                        AssistsData = new ProfileActivityChartDTO.AssistsDTO
                        {
                            Endorsed = g.SelectMany(x => x.ActivityChart.AssistsData?.Endorsed ?? new string[0]).ToArray(),
                            EndorsedBy = g.SelectMany(x => x.ActivityChart.AssistsData.EndorsedBy).ToArray()
                        },
                        DeliveryData = new ProfileActivityChartDTO.DeliveryDTO
                        {
                            TaskName = g.SelectMany(x => x.ActivityChart.DeliveryData.TaskName).ToArray(),
                            TokensGained = g.Sum(x => x.ActivityChart.DeliveryData.TokensGained),
                        },
                        ItemActivityData = new ProfileActivityChartDTO.ItemActivityDTO
                        {
                            Bought = g.SelectMany(x => x.ActivityChart.ItemActivityData?.Bought ?? new string[0]).ToArray(),
                            Disenchanted = g.SelectMany(x => x.ActivityChart.ItemActivityData?.Disenchanted ?? new string[0]).ToArray(),
                            GiftsReceived = g.SelectMany(x => x.ActivityChart.ItemActivityData?.GiftsReceived ?? new string[0]).ToArray(),
                            GiftsSent = g.SelectMany(x => x.ActivityChart.ItemActivityData?.GiftsSent ?? new string[0]).ToArray(),
                        }
                    }).ToArray();
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