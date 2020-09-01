﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using Cog.Core;
using Cog.DAL;
using Microsoft.EntityFrameworkCore;
using MoreLinq;
using Newtonsoft.Json;
using RestSharp.Extensions;
using Tayra.Common;
using Tayra.Mailer;
using Tayra.Models.Catalog;
using Tayra.Models.Organizations;
 using DateRanges = Cog.Core.DateRanges;

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

            return pe?.Profile;
        }

        public bool IsUsernameUnique(string username)
        {
            return IsUsernameUnique(DbContext, username);
        }

        public static bool IsUsernameUnique(OrganizationDbContext dbContext, string username)
        {
            return !dbContext.Profiles.Any(x => x.Username == username);
        }

        public GridData<ProfileGridDTO> GetGridData(int profileId, ProfileGridParams gridParams)
        {
            IQueryable<Profile> scope = DbContext.Profiles.Where(x => x.Id != profileId);
            
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
   
            var query = from p in scope
                        from title in DbContext.ProfileInventoryItems.Where(x => p.Id == x.ProfileId
                             && x.IsActive == true && x.ItemType == ItemTypes.TayraTitle).DefaultIfEmpty()
                        let tt = p.Tokens.Where(x => !x.ClaimRequired || x.ClaimedAt.HasValue).GroupBy( //TokenScope
                         x => x.Token,
                         x => x,
                         (t, tnxs) => new ProfileViewDTO.TokenDTO { Type = t.Type, Value = tnxs.Sum(x => x.Value) })
                        select new ProfileSummaryGridDTO
                        {
                            ProfileId = p.Id,
                            Name = p.FirstName + " " + p.LastName,
                            Username = p.Username,
                            Avatar = p.Avatar,
                            PlatformInfo = new ProfileSummaryGridDTO.PlatformData
                            {
                                CompanyTokens = (float)Math.Round(tt.Where(x => x.Type == TokenType.CompanyToken).Select(x => x.Value).FirstOrDefault(), 2),
                                Praises = DbContext.ProfilePraises.Count(x => x.ProfileId == p.Id),
                                CompletedQuests = p.CompletedQuests.Count()
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
                            }).DistinctBy(x => x.Type).ToArray()
                        }; 

            GridData<ProfileSummaryGridDTO> gridData = query.GetGridData(gridParams);

            return gridData;
        }

        public GridData<ProfileCompletedQuestsGridDTO> GetCompletedQuestsGridDTO(ProfileCompletedQuestsGridParams gridParams)
        {
            var query = from cc in DbContext.QuestCompletions
                        where cc.ProfileId == gridParams.ProfileId
                        select new ProfileCompletedQuestsGridDTO
                        {
                            QuestId = cc.QuestId,
                            QuestName = cc.Quest.Name,
                            QuestImage = cc.Quest.Image,
                            CompletedAt = cc.Created
                        };

            GridData<ProfileCompletedQuestsGridDTO> gridData = query.GetGridData(gridParams);

            return gridData;
        }

        public GridData<ProfileCommittedQuestsGridDTO> GetCommittedQuestsGridDTO(ProfileCommittedQuestsGridParams gridParams)
        {
            var query = from cc in DbContext.QuestCommits
                        where cc.ProfileId == gridParams.ProfileId
                        select new ProfileCommittedQuestsGridDTO
                        {
                            QuestId = cc.QuestId,
                            Name = cc.Quest.Name,
                            Image = cc.Quest.Image,
                            Status = cc.Quest.Status,
                            CompletedAt = cc.CompletedAt,
                            CommittedAt = cc.Created
                        };

            GridData<ProfileCommittedQuestsGridDTO> gridData = query.GetGridData(gridParams);

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
        
        public void TogglePersonalAnalytics(int profileId)
        {
            var profile = DbContext.Profiles.FirstOrDefault(x => x.Id == profileId);

            profile.EnsureNotNull(profileId);

            profile.IsAnalyticsEnabled = !profile.IsAnalyticsEnabled;
        }

        public ProfileRadarChartDTO GetProfileRadarChartDTO(int profileId)
        {
            var prd = (from r in DbContext.ProfileReportsWeekly
                where r.ProfileId == profileId
                group r by r.DateId
                into g
                orderby g.Key descending
                select new
                {
                    DateId = g.Key,
                    Assists = g.Sum(x => x.AssistsChange),
                    TasksCompleted = g.Sum(x => x.TasksCompletedChange),
                    Complexity = g.Sum(x => x.ComplexityChange)
                }).Take(4).ToArray();

            var tm = DbContext.ProfileAssignments.FirstOrDefault(x => x.ProfileId == profileId && x.TeamId.HasValue);

            var trw = new {AssistsAverage = 0d, TasksCompletedAverage = 0d, ComplexityAverage = 0d};
            if (tm != null)
            {
                trw = (from r in DbContext.TeamReportsWeekly
                    where r.TeamId == tm.TeamId
                    group r by 1
                    into g
                    select new
                    {
                        AssistsAverage = g.Average(x => x.AssistsChange),
                        TasksCompletedAverage = g.Average(x => x.TasksCompletedChange),
                        ComplexityAverage = g.Average(x => x.ComplexityChange)
                    }).FirstOrDefault();

            }

            ;

            if (!prd.Any()) //Average throws exception if count is 0
                prd = null;

            return new ProfileRadarChartDTO
            {
                AssistsAverage = Math.Round(prd?.Average(x => x.Assists) ?? 0f, 2),
                AssistsTotal = 0,

                TasksCompletedAverage = Math.Round(prd?.Average(x => x.TasksCompleted) ?? 0f, 2),
                TasksCompletedTotal = 0,

                ComplexityAverage = Math.Round(prd?.Average(x => x.Complexity) ?? 0f, 2),
                ComplexityTotal = 0,

                TeamAssistsAverage = Math.Round(trw?.AssistsAverage ?? 0f, 2),
                TeamComplexityAverage = Math.Round(trw?.ComplexityAverage ?? 0f, 2),
                TeamTasksCompletedAverage = Math.Round(trw?.TasksCompletedAverage ?? 0f, 2),
            };
        }
        public ProfileViewDTO GetProfileViewDTO(int profileId, Expression<Func<Profile, bool>> condition)
        {
            var profileDto = (from p in DbContext.Profiles.Where(condition)
                              select new ProfileViewDTO
                              {
                                  ProfileId = p.Id,
                                  FirstName = p.FirstName,
                                  LastName = p.LastName,
                                  Username = p.Username,
                                  Role = p.Role,
                                  Avatar = p.Avatar,
                                  Segments = p.Assignments.Select(x => new ProfileViewDTO.SegmentDTO { Id = x.Segment.Id, Key = x.Segment.Key, Name = x.Segment.Name}).ToArray(),
                                  Teams = p.Assignments.Where(x => x.TeamId.HasValue).Select(x => new ProfileViewDTO.TeamDTO { Id = x.Team.Id, Key = x.Team.Key, Name = x.Team.Name }).ToArray(),
                                  Praises = p.Praises.GroupBy(x => x.Type).Select(x => new ProfileViewDTO.PraiseDTO{Type = x.Key, Count = x.Count()}).ToArray(),
                                  AssistantSummary = p.AssistantSummary,
                              }).FirstOrDefault();

            profileDto.EnsureNotNull();

            var tokens = (from tt in DbContext.TokenTransactions 
                where !tt.ClaimRequired || tt.ClaimedAt.HasValue
                where tt.ProfileId == profileDto.ProfileId 
                group tt by tt.Token.Type into g 
                select new ProfileViewDTO.TokenDTO
                {
                    Type = g.Key,
                    Value = g.Sum(x => x.Value)
                }).ToArray();
            
            profileDto.CompanyTokens = Math.Round(tokens.Where(x => x.Type == TokenType.CompanyToken).Select(x => x.Value).FirstOrDefault(), 2);
            profileDto.Experience = Convert.ToInt32(tokens.Where(x => x.Type == TokenType.Experience).Select(x => x.Value).FirstOrDefault()); 
            
            if (profileId != profileDto.ProfileId)
            {
                profileDto.LastUppedAt = (from u in DbContext.ProfilePraises
                                          where u.CreatedBy == profileId
                                          where u.ProfileId == profileDto.ProfileId
                                          orderby u.DateId descending
                                          select u.Created).FirstOrDefault();
            }
            var activeItems = GetProfileActiveItems(DbContext, profileDto.ProfileId);
            profileDto.Badges = activeItems.Badges;
            profileDto.Title = activeItems.Title;
            profileDto.Border = activeItems.Border;     

            profileDto.Pulse = GetProfilePulseDTO(profileDto.ProfileId);
            
            return profileDto;
        }
        
        public ProfileRawScoreDTO GetProfileRawScoreDTO(string username)
        {
            var profile = DbContext.Profiles.FirstOrDefault(x => x.Username == username);
            profile.EnsureNotNull(username);

            return (from r in DbContext.ProfileReportsDaily
                where r.ProfileId == profile.Id
                select new ProfileRawScoreDTO
                {
                    TasksCompleted = r.TasksCompletedTotal,
                    AssistsGained = r.AssistsTotal,
                    TimeWorked =  r.TasksCompletionTimeTotal,
                    TokensEarned =  r.CompanyTokensEarnedTotal,
                    TokensSpent =  r.CompanyTokensSpentTotal,
                    ItemsBought = r.ItemsBoughtTotal,
                    QuestsCompleted = r.QuestsCompletedTotal,
                    DaysOnTayra = EF.Functions.DateDiffDay(profile.Created, DateTime.UtcNow)
                }).LastOrDefault();
        }

        public void ModifyTokens(ProfileRoles profileRole, ProfileModifyTokensDTO dto)
        {
            if (profileRole != ProfileRoles.Admin)
            {
                throw new CogSecurityException("You are not allowed to perform this action!");
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
                    var s = d.Settings.FirstOrDefault(x => x.LogEvent == sdto.LogEvent);
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
                            EndorsedBy = g.SelectMany(x => x.ActivityChart.AssistsData?.EndorsedBy ?? new string[0]).ToArray()
                        },
                        DeliveryData = new ProfileActivityChartDTO.DeliveryDTO
                        {
                            TaskName = g.SelectMany(x => x.ActivityChart.DeliveryData?.TaskName ?? new string[0]).ToArray(),
                            TokensGained = g.Sum(x => x.ActivityChart.DeliveryData.TokensGained),
                        },
                        ItemActivityData = new ProfileActivityChartDTO.ItemActivityDTO
                        {
                            Bought = g.SelectMany(x => x.ActivityChart.ItemActivityData?.Bought ?? new string[0]).ToArray(),
                            Disenchanted = g.SelectMany(x => x.ActivityChart.ItemActivityData?.Disenchanted ?? new string[0]).ToArray(),
                            GiftsReceived = g.SelectMany(x => x.ActivityChart.ItemActivityData?.GiftsReceived ?? new string[0]).ToArray(),
                            GiftsSent = g.SelectMany(x => x.ActivityChart.ItemActivityData?.GiftsSent ?? new string[0]).ToArray(),
                        },
                        GitCommitData = g.Any(x => x.ActivityChart.GitCommitData != null) ?g.SelectMany(x => x.ActivityChart?.GitCommitData?.Select(c => 
                        new ProfileActivityChartDTO.GitCommitDTO 
                        {
                            Message = c?.Message ?? string.Empty,
                            ExternalUrl = c?.ExternalUrl ?? string.Empty
                        }))?.ToArray()
                            : new ProfileActivityChartDTO.GitCommitDTO[]{}
                    }).ToArray();
        }
        
        public ProfileStatsDTO GetProfileStatsData(int profileId)
        {
            var latestUpdateDateId = DateHelper.FindPeriod(DateRanges.Last4Week).FromId;
            
            var segments = from pa in DbContext.ProfileAssignments
                where pa.ProfileId == profileId
                select pa.SegmentId;

            var segmentsStats =
                DbContext.SegmentReportsWeekly
                    .Where(x => segments.Contains(x.SegmentId) && x.DateId >= latestUpdateDateId)
                    .ToLookup(x => x.SegmentId).ToDictionary(x => x.Key, x => new 
                    {
                        Impact = x.Select(r => r.OImpactAverage).ToArray(),
                        Speed = x.Select(r => r.SpeedAverage).ToArray(),
                        Power = x.Select(r => r.PowerAverage).ToArray(),
                        Heat = x.Select(r => r.HeatAverageTotal).ToArray(),
                        Assists = x.Select(r => (float) r.AssistsChange).ToArray(),
                        TaskCompletion = x.Select(r => (float) r.TasksCompletedChange).ToArray(),
                        Complexity = x.Select(r => (float) r.ComplexityChange).ToArray(),
                    });
            
            var teams = from pa in DbContext.ProfileAssignments
                where pa.ProfileId == profileId 
                select pa.TeamId;

            var teamsStats =
                DbContext.TeamReportsWeekly
                    .Where(x => teams.Contains(x.TeamId) && x.DateId >= latestUpdateDateId)
                    .ToLookup(x => x.TeamId).ToDictionary(x => x.Key, x => new 
                    {
                        Impact = x.Select(r => r.OImpactAverage).ToArray(),
                        Speed = x.Select(r => r.SpeedAverage).ToArray(),
                        Power = x.Select(r => r.PowerAverage).ToArray(),
                        Heat = x.Select(r => r.HeatAverageTotal).ToArray(),
                        Assists = x.Select(r => (float) r.AssistsChange).ToArray(),
                        TaskCompletion = x.Select(r => (float) r.TasksCompletedChange).ToArray(),
                        Complexity = x.Select(r => (float) r.ComplexityChange).ToArray(),
                    });
            
            return (from prw in DbContext.ProfileReportsWeekly
                where prw.ProfileId == profileId
                where prw.DateId >= latestUpdateDateId
                group prw by 1 into r
                select new ProfileStatsDTO
                {
                    LatestUpdateDateId = latestUpdateDateId,
                    Metrics = (new ProfileStatsDTO.ProfileMetricDTO[]
                    {
                        new ProfileStatsDTO.ProfileMetricDTO
                        {
                            Id = MetricTypes.OImpact,
                            SegmentsAverages = segmentsStats.Select(x => new ProfileStatsDTO.ProfileMetricDTO.AssignmentAveragesDTO
                            {
                                Id = x.Key,
                                Averages = x.Value.Impact,
                                TotalAverage = x.Value.Impact.Sum() / 4f
                            }).ToArray(),
                            TeamsAverages = teamsStats.Select(x => new ProfileStatsDTO.ProfileMetricDTO.AssignmentAveragesDTO
                            {
                                Id = x.Key ,
                                Averages = x.Value.Impact,
                                TotalAverage = x.Value.Impact.Sum() / 4f
                            }).ToArray(),
                            WeeklyAverages = r.Select(x => x.OImpactAverage).ToArray()
                        },
                        new ProfileStatsDTO.ProfileMetricDTO
                        {
                            Id = MetricTypes.Speed,
                            SegmentsAverages = segmentsStats.Select(x => new ProfileStatsDTO.ProfileMetricDTO.AssignmentAveragesDTO
                            {
                                Id = x.Key,
                                Averages = x.Value.Speed,
                                TotalAverage = x.Value.Speed.Sum() / 4f
                            }).ToArray(),
                            TeamsAverages = teamsStats.Select(x => new ProfileStatsDTO.ProfileMetricDTO.AssignmentAveragesDTO
                            {
                                Id = x.Key ,
                                Averages = x.Value.Speed,
                                TotalAverage = x.Value.Speed.Sum() / 4f
                            }).ToArray(),
                            WeeklyAverages = r.Select(x => x.SpeedAverage).ToArray()
                        },
                        new ProfileStatsDTO.ProfileMetricDTO
                        {
                            Id = MetricTypes.Power,
                            SegmentsAverages = segmentsStats.Select(x => new ProfileStatsDTO.ProfileMetricDTO.AssignmentAveragesDTO
                            {
                                Id = x.Key,
                                Averages = x.Value.Power,
                                TotalAverage = x.Value.Power.Sum() / 4f
                            }).ToArray(),
                            TeamsAverages = teamsStats.Select(x => new ProfileStatsDTO.ProfileMetricDTO.AssignmentAveragesDTO
                            {
                                Id = x.Key ,
                                Averages = x.Value.Power,
                                TotalAverage = x.Value.Power.Sum() / 4f
                            }).ToArray(),
                            WeeklyAverages = r.Select(x => x.PowerAverage).ToArray()
                        },
                        new ProfileStatsDTO.ProfileMetricDTO
                        {
                            Id = MetricTypes.Heat,
                            SegmentsAverages = segmentsStats.Select(x => new ProfileStatsDTO.ProfileMetricDTO.AssignmentAveragesDTO
                            {
                                Id = x.Key,
                                Averages = x.Value.Heat,
                                TotalAverage = x.Value.Heat.Sum() / 4f
                            }).ToArray(),
                            TeamsAverages = teamsStats.Select(x => new ProfileStatsDTO.ProfileMetricDTO.AssignmentAveragesDTO
                            {
                                Id = x.Key ,
                                Averages = x.Value.Heat,
                                TotalAverage = x.Value.Heat.Sum() / 4f
                            }).ToArray(),
                            WeeklyAverages = r.Select(x => x.Heat).ToArray()
                        },
                        new ProfileStatsDTO.ProfileMetricDTO
                        {
                            Id = MetricTypes.Complexity,
                            SegmentsAverages = segmentsStats.Select(x => new ProfileStatsDTO.ProfileMetricDTO.AssignmentAveragesDTO
                            {
                                Id = x.Key,
                                Averages =  x.Value.Complexity,
                                TotalAverage = x.Value.Complexity.Sum() / 4f
                            }).ToArray(),
                            TeamsAverages = teamsStats.Select(x => new ProfileStatsDTO.ProfileMetricDTO.AssignmentAveragesDTO
                            {
                                Id = x.Key ,
                                Averages = x.Value.Complexity,
                                TotalAverage = x.Value.Complexity.Sum() / 4f
                            }).ToArray(),
                            WeeklyAverages = r.Select(x => (float) x.ComplexityChange).ToArray()
                        },
                        new ProfileStatsDTO.ProfileMetricDTO
                        {
                            Id = MetricTypes.Assist,
                            SegmentsAverages = segmentsStats.Select(x => new ProfileStatsDTO.ProfileMetricDTO.AssignmentAveragesDTO
                            {
                                Id = x.Key,
                                Averages = x.Value.Assists,
                                TotalAverage = x.Value.Assists.Sum() / 4f
                            }).ToArray(),
                            TeamsAverages = teamsStats.Select(x => new ProfileStatsDTO.ProfileMetricDTO.AssignmentAveragesDTO
                            {
                                Id = x.Key ,
                                Averages = x.Value.Assists,
                                TotalAverage = x.Value.Assists.Sum() / 4f
                            }).ToArray(),
                            WeeklyAverages = r.Select(x => (float) x.AssistsChange).ToArray()
                        },
                        new ProfileStatsDTO.ProfileMetricDTO
                        {
                            Id = MetricTypes.TaskCompletion,
                            SegmentsAverages = segmentsStats.Select(x => new ProfileStatsDTO.ProfileMetricDTO.AssignmentAveragesDTO
                            {
                                Id = x.Key,
                                Averages = x.Value.TaskCompletion,
                                TotalAverage = x.Value.TaskCompletion.Sum() / 4f
                            }).ToArray(),
                            TeamsAverages = teamsStats.Select(x => new ProfileStatsDTO.ProfileMetricDTO.AssignmentAveragesDTO
                            {
                                Id = x.Key ,
                                Averages = x.Value.TaskCompletion,
                                TotalAverage = x.Value.TaskCompletion.Sum() / 4f
                            }).ToArray(),
                            WeeklyAverages = r.Select(x => (float) x.TasksCompletedChange).ToArray()
                        }
                    }).ToArray()
                }).FirstOrDefault();
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

        private ProfileViewDTO.PulseDTO GetProfilePulseDTO(int profileId)
        {
            var yesterdayDateId = DateHelper2.ToDateId(DateTime.UtcNow.AddDays(-1));
            var tasks = (from t in DbContext.Tasks
                        where t.AssigneeProfileId == profileId
                        where t.Status == TaskStatuses.InProgress || (t.Status == TaskStatuses.Done && t.LastModifiedDateId >= yesterdayDateId)
                        select new
                        {
                            DTO = new ProfileViewDTO.PulseDTO.Task
                            {
                                Status = t.Status,
                                Summary = t.Summary,
                                ExternalUrl = t.ExternalUrl    
                            },
                            LastModifiedDateId = t.LastModifiedDateId
                        }).ToArray();

            return new ProfileViewDTO.PulseDTO
            {
                InProgress = tasks.Select(x => x.DTO).Where(x => x.Status == TaskStatuses.InProgress).ToArray(),
                RecentlyDone = tasks.Select(x => x.DTO).Where(x => x.Status == TaskStatuses.Done).ToArray()
            };
        }
        
        #endregion
    }
}