using System;
using Firdaws.Core;
using Tayra.Common;
using Tayra.Models.Organizations;
using System.Linq;
using Firdaws.DAL;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using MoreLinq;

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

        public GridData<ChallengeViewGridDTO> GetChallengesGrid(int[] segmentIds, ChallengeViewGridParams gridParams)
        {
            IQueryable<ChallengeSegment> scope = DbContext.ChallengeSegments;

            if(gridParams.Segments != null && gridParams.Segments.Any())
            {
                scope = scope.Where(x => gridParams.Segments.Contains(x.SegmentId));
            }
            else
            {
                scope = scope.Where(x => segmentIds.Contains(x.SegmentId));
            }
            
            var query = from cs in scope
                        select new ChallengeViewGridDTO
                        {
                            ChallengeId = cs.Challenge.Id,
                            Name = cs.Challenge.Name,
                            Image = cs.Challenge.Image,
                            Status = cs.Challenge.Status,
                            RewardValue = cs.Challenge.RewardValue,
                            CompletionsRemaining = cs.Challenge.CompletionsRemaining,
                            Created = cs.Challenge.Created,
                            ActiveUntil = cs.Challenge.ActiveUntil,
                            EndedAt = cs.Challenge.EndedAt
                        };

            GridData<ChallengeViewGridDTO> gridData = query.GetGridData(gridParams);

            gridData.Records = gridData.Records.DistinctBy(x => x.ChallengeId).ToList();

            return gridData;
        }

        public GridData<ChallengeCommitteesGridDTO> GetChallengeCommitteesGrid(int profileId, ChallengeCommitteesGridParams gridParams)
        {
            IQueryable<ChallengeCommit> scope = DbContext.ChallengeCommits.Where(x => x.ChallengeId == gridParams.ChallengeId);

            var query = from cc in scope
                        select new ChallengeCommitteesGridDTO
                        {
                            ProfileId = cc.ProfileId,
                            Username = cc.Profile.Username,
                            Avatar = cc.Profile.Avatar,
                            CommittedOn = cc.Created
                        };

            GridData<ChallengeCommitteesGridDTO> gridData = query.GetGridData(gridParams);

            return gridData;
        }

        public GridData<ChallengeCompletitionsGridDTO> GetChallengeCompletitionsGrid(int profileId, ChallengeCompletitionsGridParams gridParams)
        {
            IQueryable<ChallengeCompletion> scope = DbContext.ChallengeCompletions.Where(x => x.ChallengeId == gridParams.ChallengeId);

            var query = from cc in scope
                        select new ChallengeCompletitionsGridDTO
                        {
                            ProfileId = cc.ProfileId,
                            Username = cc.Profile.Username,
                            Avatar = cc.Profile.Avatar,
                            CompletedAt = cc.Created
                        };

            GridData<ChallengeCompletitionsGridDTO> gridData = query.GetGridData(gridParams);

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
                                 CommittedOn = c.Commits.Where(x => x.ProfileId == profileId).Select(x => (DateTime?)x.Created).FirstOrDefault(),
                                 Segments = c.Segments.Select(x => x.SegmentId).ToArray(),
                                 Rewards = c.Rewards.Select(x => new ChallengeViewDTO.RewardDTO
                                 {
                                     ItemId = x.ItemId,
                                     Name = x.Item.Name,
                                     Image = x.Item.Image,
                                     Type = x.Item.Type,
                                     Rarity = x.Item.Rarity,
                                     WorthValue = x.Item.WorthValue
                                 }).ToArray(),
                                 Goals = c.Goals.Select(x => new ChallengeViewDTO.GoalDTO
                                 {
                                     GoalId = x.Id,
                                     Title = x.Title,
                                     IsCommentRequired = x.IsCommentRequired,
                                     Comment = x.Completitions.Where(gc => gc.ProfileId == profileId).Select(gc => gc.Comment).FirstOrDefault(),
                                     IsCompleted = x.Completitions.Where(gc => gc.ProfileId == profileId).Any()
                                 }).ToArray()
                             }).FirstOrDefault();

            return challenge;
        }

        public void Create(ChallengeCreateDTO dto)
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
                            QuantityToReserve = dto.Rewards.FirstOrDefault(y => y.ItemId == i.Id).Quantity * (dto.CompletionsLimit ?? 1)
                        }).ToList();

            foreach (var i in items)
            {
                if(!ItemRules.CanReserveQuantity(i.QuantityAvailable, i.QuantityToReserve))
                {
                    throw new ApplicationException($"Quantity too high for item {i.ItemId}");
                }

                if (!ChallengeRules.IsCompletionLimitValid(dto.CompletionsLimit))//, i.QuantityToReserve))
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
                RewardValue = items.Sum(x => x.Worth * x.QuantityToReserve),
                Segments = dto.Segments.Select(x => new ChallengeSegment { SegmentId = x }).ToArray(),
                Rewards = dto.Rewards.Select(x => new ChallengeReward { ItemId = x.ItemId, Quantity = x.Quantity }).ToArray(),
                Goals = dto.Goals.Select(x => new ChallengeGoal { Title = x.Title, IsCommentRequired = x.IsCommentRequired }).ToArray()
            });
        }

        public void Update(int challengeId, ChallengeUpdateDTO dto)
        {
            var challenge = DbContext.Challenges
                .Include(x => x.Segments)
                .Include(x => x.Goals)
                .Include(x => x.Rewards)
                .FirstOrDefault(x => x.Id == challengeId);

            challenge.EnsureNotNull(challengeId);

            if (!ChallengeRules.IsActiveUntilValid(dto.ActiveUntil))
            {
                dto.AddPropertyError(x => x.ActiveUntil, "Must be a date in future");
                dto.Validate();
            }

            challenge.Name = dto.Name;
            challenge.Description = dto.Description;
            challenge.Image = dto.Image;
            challenge.ActiveUntil = dto.ActiveUntil;

            //Update Segments
            challenge.Segments.ToList().RemoveAll(x => !dto.Segments.Contains(x.SegmentId));
            dto.Segments.RemoveAll(x => !challenge.Segments.Select(dtos => dtos.SegmentId).Contains(x));
            dto.Segments.ForEach(sId => challenge.Segments.Add(new ChallengeSegment { ChallengeId = challenge.Id, SegmentId = sId }));

            //Update Goals
            challenge.Goals.ToList().RemoveAll(x => !dto.Goals.Where(dtog => dtog.GoalId.HasValue).Select(y => y.GoalId).Contains(x.Id));
            dto.Goals.Where(x => x.GoalId.HasValue).ToList().ForEach(x =>
            {
                var cg = challenge.Goals.First(g => g.Id == x.GoalId);
                cg.Title = x.Title;
                cg.IsCommentRequired = x.IsCommentRequired;
            });
            dto.Goals.Where(x => !x.GoalId.HasValue).ToList().ForEach(x =>
            {
                challenge.Goals.Add(new ChallengeGoal { ChallengeId = challenge.Id, Title = x.Title, IsCommentRequired = x.IsCommentRequired });
            });

            //Update Rewards
            challenge.Rewards.ToList().RemoveAll(x => !dto.Rewards.Select(dtor => dtor.ItemId).Contains(x.ItemId));
            dto.Rewards.RemoveAll(x => !challenge.Rewards.Select(r => r.ItemId).Contains(x.ItemId));
            dto.Rewards.ForEach(r => challenge.Rewards.Add(new ChallengeReward { ChallengeId = challenge.Id, ItemId = r.ItemId, Quantity = r.Quantity }));
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

        public void CommitToChallenge(int profileId, ChallengeCommitDTO dto)
        {
            var challenge = DbContext.Challenges.FirstOrDefault(x => x.Id == dto.ChallengeId);

            challenge.EnsureNotNull(dto.ChallengeId);

            DbContext.Add(new ChallengeCommit
            {
                ProfileId = profileId,
                ChallengeId = challenge.Id
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
