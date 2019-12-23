using System;
using Firdaws.Core;
using Tayra.Common;
using Tayra.Models.Organizations;
using System.Linq;
using Firdaws.DAL;
using System.Collections.Generic;

namespace Tayra.Services
{
    public class ChallengesService : BaseService<OrganizationDbContext>, IChallengesService
    {
        #region Constructor

        protected ILogsService LogsService { get; set; }
        protected ITokensService TokensService { get; set; }

        public ChallengesService(ITokensService tokensService, ILogsService logsService, OrganizationDbContext dbContext) : base(dbContext)
        {
            TokensService = tokensService;
            LogsService = logsService;
        }

        #endregion

        #region Public Methods

        public GridData<ChallengeViewGridDTO> GetProjectChallengesGrid(int projectId, ChallengeViewGridParams gridParams)
        {
            IQueryable<Challenge> scope = DbContext.Challenges.Where(x => x.ProjectId == projectId);
            if (gridParams.Statuses != null)
            {
                scope = scope.Where(x => gridParams.Statuses.Contains(x.Status));
            }

            var query = from c in scope
                        let cc = DbContext.ChallengeCompletions.Where(x => x.ChallengeId == c.Id)
                        select new ChallengeViewGridDTO
                        {
                            Id = c.Id,
                            Name = c.Name,
                            Image = c.Image,
                            Status = c.Status,
                            Description = c.Description,
                            CustomReward = c.CustomReward,
                            TokenRewardValue = c.TokenRewardValue,
                            CompletionsRemaining = c.CompletionsRemaining,
                            Created = c.Created,
                            ActiveUntil = c.ActiveUntil,
                            EndedAt = c.EndedAt,
                            TotalCompletions = cc.Count(),
                            Completions = cc.Select(x => new ChallengeViewGridDTO.Completion
                            {
                                ProfileId = x.ProfileId,
                                ProfileUsername = x.Profile.Username,
                                ProfileAvatar = x.Profile.Avatar,
                                CompletedAt = x.Created
                            }).ToList()
                        };

            GridData<ChallengeViewGridDTO> gridData = query.GetGridData(gridParams);

            return gridData;
        }

        public void Create(int projectId, ChallengeCreateDTO dto)
        {
            if (!ChallengeRules.IsActiveUntilValid(dto.ActiveUntil))
            {
                throw new ApplicationException($"Invalid {nameof(Challenge.ActiveUntil)}");
            }

            DbContext.Add(new Challenge
            {
                Name = dto.Name,
                Status = ChallengeStatuses.Active,
                Description = dto.Description,
                Image = dto.Image,
                CustomReward = dto.CustomReward,
                TokenRewardValue = dto.TokenRewardValue,
                CompletionsLimit = dto.CompletionsLimit,
                CompletionsRemaining = dto.CompletionsLimit,
                IsEasterEgg = dto.IsEasterEgg,
                ActiveUntil = dto.ActiveUntil,
                ProjectId = projectId
            });
        }

        public void Update(int projectId, ChallengeUpdateDTO dto)
        {
            var challenge = DbContext.Challenges.FirstOrDefault(x => x.Id == dto.ChallengeId);

            challenge.EnsureNotNull(dto.ChallengeId);

            if (!ChallengeRules.IsActiveUntilValid(dto.ActiveUntil))
            {
                dto.AddPropertyError(x => x.ActiveUntil, "Must be a date in future");
                dto.Validate();
            }

            challenge.Name = dto.Name;
            challenge.Description = dto.Description;
            challenge.Image = dto.Image;
            challenge.CustomReward = dto.CustomReward;
            challenge.TokenRewardValue = dto.TokenRewardValue;
            challenge.CompletionsRemaining = dto.CompletionsRemaining;
            challenge.IsEasterEgg = dto.IsEasterEgg;
            challenge.ActiveUntil = dto.ActiveUntil;
            challenge.ProjectId = projectId;
        }

        public void CompleteChallenge(ChallengeCompleteDTO dto)
        {
            var profile = DbContext.Profiles.FirstOrDefault(x => x.Id == dto.ProfileId);
            var challenge = DbContext.Challenges.FirstOrDefault(x => x.Id == dto.ChallengeId);

            if (!ChallengeRules.CanBeCompleted(challenge.ActiveUntil, challenge.CompletionsRemaining, challenge.Status))
            {
                throw new ApplicationException($"Challenge {challenge.Name} can't be completed");
            }

            challenge.CompletionsRemaining -= 1;

            TokensService.CreateTransaction(TokenType.CompanyToken, profile.Id, challenge.TokenRewardValue, TransactionReason.ChallengeCompleted, ClaimBundleTypes.ChallengeCompleted);

            DbContext.Add(new ChallengeCompletion
            {
                ChallengeId = dto.ChallengeId,
                ProfileId = profile.Id
            });

            LogsService.LogEvent(new LogCreateDTO
            {
                Event = LogEvents.ChallengeCompleted,
                Data = new Dictionary<string, string>
                {
                    { "timestamp", DateTime.UtcNow.ToString() },
                    { "profileUsername", profile.Username },
                    { "challengeName", challenge.Name },
                    { "challengeTokenReward", challenge.TokenRewardValue.ToString() },
                    { "challengeCustomReward", challenge.CustomReward }
                },
                ProfileId = profile.Id,
            });
        }

        public void EndChallenge(int profileId, int challengeId)
        {
            var challenge = DbContext.Challenges.FirstOrDefault(x => x.Id == challengeId);

            if (!ChallengeRules.CanBeEnded(challenge.Status))
            {
                throw new ApplicationException($"Challenge {challenge.Name} can't be ended");
            }

            challenge.Status = ChallengeStatuses.Ended;
            challenge.EndedAt = DateTime.UtcNow;
        }

        #endregion
    }
}
