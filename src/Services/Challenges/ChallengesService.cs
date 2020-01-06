using System;
using Firdaws.Core;
using Tayra.Common;
using Tayra.Models.Organizations;
using System.Linq;
using Firdaws.DAL;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

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

        public GridData<ChallengeViewGridDTO> GetSegmentChallengesGrid(int segmentId, ChallengeViewGridParams gridParams)
        {
            IQueryable<Challenge> scope = DbContext.Challenges.Where(x => x.SegmentId == segmentId);
            if (gridParams.Statuses != null)
            {
                scope = scope.Where(x => gridParams.Statuses.Contains(x.Status));
            }

            var query = from c in scope
                        select new ChallengeViewGridDTO
                        {
                            Id = c.Id,
                            Name = c.Name,
                            Image = c.Image,
                            Status = c.Status,
                            RewardValue = c.RewardValue,
                            CompletionsRemaining = c.CompletionsRemaining,
                            Created = c.Created,
                            ActiveUntil = c.ActiveUntil,
                            EndedAt = c.EndedAt
                        };

            GridData<ChallengeViewGridDTO> gridData = query.GetGridData(gridParams);

            return gridData;
        }

        public ChallengeViewDTO GetChallengeViewDTO(int profileId, int challengeId)
        {
            var challenge = (from c in DbContext.Challenges
                             where c.Id == challengeId
                             select new ChallengeViewDTO
                             {
                                 Name = c.Name,
                                 Image = c.Image,
                                 Description = c.Description,
                                 Status = c.Status,
                                 RewardValue = c.RewardValue,
                                 CompletionsLimit = c.CompletionsLimit,
                                 CompletionsRemaining = c.CompletionsRemaining,
                                 Created = c.Created,
                                 ActiveUntil = c.ActiveUntil,
                                 EndedAt = c.EndedAt,
                                 Rewards = c.Rewards.Select(x => new ChallengeViewDTO.RewardDTO
                                 {
                                     ItemId = x.ItemId,
                                     Name = x.Item.Name,
                                     Image = x.Item.Image,
                                     Type = x.Item.Type,
                                     WorthValue = x.Item.WorthValue
                                 }).ToArray(),
                                 Goals = c.Goals.Select(x => new ChallengeViewDTO.GoalDTO
                                 {
                                     GoalId = x.Id,
                                     Title = x.Title,
                                     IsCommentRequired = x.IsCommentRequired,
                                     Comment = null,
                                     IsCompleted = false
                                 }).ToArray()
                             }).FirstOrDefault();

            return challenge;
        }

        public void Create(int segmentId, ChallengeCreateDTO dto)
        {
            if (!ChallengeRules.IsActiveUntilValid(dto.ActiveUntil))
            {
                throw new ApplicationException($"Invalid {nameof(Challenge.ActiveUntil)}");
            }

            var items = (from i in DbContext.Items
                        where dto.Rewards.Select(y => y.ItemId).Contains(i.Id)
                        select new
                        {
                            ItemId = i.Id,
                            Worth = i.WorthValue,
                            QuantityAvailable = i.IsQuantityLimited ? i.Reservations.Sum(x => x.QuantityChange) : (int?)null,
                            QuantityToReserve = dto.Rewards.FirstOrDefault(y => y.ItemId == i.Id).Quantity
                        }).ToList();

            foreach (var i in items)
            {
                if(!ItemRules.CanReserveQuantity(i.QuantityAvailable, i.QuantityToReserve))
                {
                    throw new ApplicationException($"Quantity too high for item {i.ItemId}");
                }

                if (!ChallengeRules.IsCompletionLimitValid(dto.CompletionsLimit, i.QuantityToReserve))
                {
                    throw new ApplicationException($"Invalid {nameof(Challenge.CompletionsLimit)}");
                }

                DbContext.Add(new ItemReservation
                {
                    ItemId = i.ItemId,
                    QuantityChange = i.QuantityToReserve
                });
            }

            DbContext.Add(new Challenge
            {
                Name = dto.Name,
                Status = ChallengeStatuses.Active,
                Description = dto.Description,
                Image = dto.Image,
                CompletionsLimit = dto.CompletionsLimit,
                CompletionsRemaining = dto.CompletionsLimit,
                IsEasterEgg = dto.IsEasterEgg,
                ActiveUntil = dto.ActiveUntil,
                SegmentId = segmentId,
                RewardValue = items.Sum(x => x.Worth * x.QuantityToReserve),
                Rewards = dto.Rewards.Select(x => new ChallengeReward { ItemId = x.ItemId, QuantityReserved = x.Quantity }).ToArray(),
                Goals = dto.Goals.Select(x => new ChallengeGoal { Title = x.Title, IsCommentRequired = x.IsCommentRequired }).ToArray()
            });
        }

        public void Update(int segmentId, ChallengeUpdateDTO dto)
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
            //challenge.CompletionsRemaining = dto.CompletionsRemaining;
            challenge.IsEasterEgg = dto.IsEasterEgg;
            challenge.ActiveUntil = dto.ActiveUntil;
            challenge.SegmentId = segmentId;
        }

        public void CompleteGoal(int profileId, ChallengeGoalCompleteDTO dto)
        {
            var profile = DbContext.Profiles.FirstOrDefault(x => x.Id == profileId);
            var goal = DbContext.ChallengeGoals.Include(x => x.Challenge).FirstOrDefault(x => x.Id == dto.GoalId);

            if (!ChallengeRules.CanGoalBeCompleted(goal.Challenge.Status))
            {
                throw new ApplicationException($"Challenge goal {goal.Title} can't be completed");
            }

            DbContext.Add(new ChallengeGoalCompletion
            {
                GoalId = dto.GoalId,
                ProfileId = profileId,
                Comment = dto.Comment
            });

            LogsService.LogEvent(new LogCreateDTO
            {
                Event = LogEvents.ChallengeGoalCompleted,
                Data = new Dictionary<string, string>
                {
                    { "timestamp", DateTime.UtcNow.ToString() },
                    { "profileUsername", profile.Username },
                    { "challengeName", goal.Challenge.Name },
                    { "goalTitle", goal.Title }
                },
                ProfileId = profile.Id,
            });
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
                    { "challengeRewardValue", challenge.RewardValue.ToString() }
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
