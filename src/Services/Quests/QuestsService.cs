using System;
using Cog.Core;
using Tayra.Common;
using Tayra.Models.Organizations;
using System.Linq;
using Cog.DAL;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using MoreLinq;

namespace Tayra.Services
{
    public class QuestsService : BaseService<OrganizationDbContext>, IQuestsService
    {
        #region Constructor

        protected ILogsService LogsService { get; set; }
        protected ITokensService TokensService { get; set; }

        public QuestsService(ITokensService tokensService, ILogsService logsService, OrganizationDbContext dbContext) : base(dbContext)
        {
            TokensService = tokensService;
            LogsService = logsService;
        }

        #endregion

        #region Public Methods

        public GridData<QuestViewGridDTO> GetQuestsGrid(Guid[] segmentIds, QuestViewGridParams gridParams)
        {
            IQueryable<QuestSegment> scope = DbContext.QuestSegments;

            if (gridParams.Segments != null && gridParams.Segments.Any())
            {
                scope = scope.Where(x => gridParams.Segments.Contains(x.SegmentId));
            }
            else
            {
                scope = scope.Where(x => segmentIds.Contains(x.SegmentId));
            }

            var query = from cs in scope
                        select new QuestViewGridDTO
                        {
                            QuestId = cs.Quest.Id,
                            Name = cs.Quest.Name,
                            Image = cs.Quest.Image,
                            Status = cs.Quest.Status,
                            RewardValue = cs.Quest.RewardValue,
                            CompletionsRemaining = cs.Quest.CompletionsRemaining,
                            Created = cs.Quest.Created,
                            ActiveUntil = cs.Quest.ActiveUntil,
                            EndedAt = cs.Quest.EndedAt
                        };

            GridData<QuestViewGridDTO> gridData = query.GetGridData(gridParams);

            gridData.Records = gridData.Records.DistinctBy(x => x.QuestId).ToList();

            return gridData;
        }

        public GridData<QuestCommitsGridDTO> GetQuestCommitsGrid(Guid profileId, QuestCommitsGridParams gridParams)
        {
            IQueryable<QuestCommit> scope = DbContext.QuestCommits.Where(x => x.QuestId == gridParams.QuestId);

            var query = from cc in scope
                        select new QuestCommitsGridDTO
                        {
                            ProfileId = cc.ProfileId,
                            Username = cc.Profile.Username,
                            Avatar = cc.Profile.Avatar,
                            CommittedOn = cc.Created
                        };

            GridData<QuestCommitsGridDTO> gridData = query.GetGridData(gridParams);

            return gridData;
        }

        public GridData<QuestCompletitionsGridDTO> GetQuestCompletitionsGrid(Guid profileId, QuestCompletitionsGridParams gridParams)
        {
            IQueryable<QuestCompletion> scope = DbContext.QuestCompletions.Where(x => x.QuestId == gridParams.QuestId);

            var query = from cc in scope
                        select new QuestCompletitionsGridDTO
                        {
                            ProfileId = cc.ProfileId,
                            Username = cc.Profile.Username,
                            Avatar = cc.Profile.Avatar,
                            CompletedAt = cc.Created
                        };

            GridData<QuestCompletitionsGridDTO> gridData = query.GetGridData(gridParams);

            return gridData;
        }


        public QuestViewDTO GetQuestViewDTO(Guid profileId, int questId)
        {
            var quest = (from c in DbContext.Quests
                         where c.Id == questId
                         select new QuestViewDTO
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
                             Rewards = c.Rewards.Select(x => new QuestViewDTO.RewardDTO
                             {
                                 ItemId = x.ItemId,
                                 Name = x.Item.Name,
                                 Image = x.Item.Image,
                                 Type = x.Item.Type,
                                 Rarity = x.Item.Rarity,
                                 Price = x.Item.Price
                             }).ToArray(),
                             Goals = c.Goals.Select(x => new QuestViewDTO.GoalDTO
                             {
                                 GoalId = x.Id,
                                 Title = x.Title,
                                 IsCommentRequired = x.IsCommentRequired,
                                 Comment = x.Completitions.Where(gc => gc.ProfileId == profileId).Select(gc => gc.Comment).FirstOrDefault(),
                                 IsCompleted = x.Completitions.Where(gc => gc.ProfileId == profileId).Any()
                             }).ToArray()
                         }).FirstOrDefault();

            return quest;
        }

        public void Create(QuestCreateDTO dto)
        {
            if (!QuestRules.IsActiveUntilValid(dto.ActiveUntil))
            {
                throw new ApplicationException($"Invalid {nameof(Quest.ActiveUntil)}");
            }

            var items = (from i in DbContext.Items
                         where dto.Rewards.Select(y => y.ItemId).Contains(i.Id)
                         select new
                         {
                             ItemId = i.Id,
                             Price = i.Price,
                             QuantityAvailable = i.QuestsQuantityRemaining,
                             QuantityToReserve = dto.Rewards.FirstOrDefault(y => y.ItemId == i.Id).Quantity * (dto.CompletionsLimit ?? 1)
                         }).ToList();

            foreach (var i in items)
            {
                if (!ItemRules.CanReserveQuantity(i.QuantityAvailable, i.QuantityToReserve))
                {
                    throw new ApplicationException($"There is not enough quantity for item {i.ItemId}");
                }

                if (!QuestRules.IsCompletionLimitValid(dto.CompletionsLimit))//, i.QuantityToReserve))
                {
                    throw new ApplicationException($"Invalid {nameof(Quest.CompletionsLimit)}");
                }

                var entity = DbContext.Items.FirstOrDefault(x => x.Id == i.ItemId);
                entity.QuestsQuantityRemaining -= i.QuantityToReserve;
            }

            DbContext.Add(new Quest
            {
                Name = dto.Name,
                Status = QuestStatuses.Active,
                Description = dto.Description,
                Image = dto.Image,
                CompletionsLimit = dto.CompletionsLimit,
                CompletionsRemaining = dto.CompletionsLimit,
                IsEasterEgg = dto.IsEasterEgg,
                ActiveUntil = dto.ActiveUntil,
                RewardValue = items.Sum(x => x.Price * x.QuantityToReserve),
                Segments = dto.Segments.Select(x => new QuestSegment { SegmentId = x }).ToArray(),
                Rewards = dto.Rewards.Select(x => new QuestReward { ItemId = x.ItemId, Quantity = x.Quantity }).ToArray(),
                Goals = dto.Goals.Select(x => new QuestGoal { Title = x.Title, IsCommentRequired = x.IsCommentRequired }).ToArray()
            });
        }

        public void Update(int questId, QuestUpdateDTO dto)
        {
            var quest = DbContext.Quests
                .Include(x => x.Segments)
                .Include(x => x.Goals)
                .Include(x => x.Rewards)
                .FirstOrDefault(x => x.Id == questId);

            quest.EnsureNotNull(questId);

            if (!QuestRules.IsActiveUntilValid(dto.ActiveUntil))
            {
                dto.AddPropertyError(x => x.ActiveUntil, "Must be a date in future");
                dto.Validate();
            }

            quest.Name = dto.Name;
            quest.Description = dto.Description;
            quest.Image = dto.Image;
            quest.ActiveUntil = dto.ActiveUntil;

            //Update Segments
            quest.Segments.ToList().RemoveAll(x => !dto.Segments.Contains(x.SegmentId));
            dto.Segments.RemoveAll(x => !quest.Segments.Select(dtos => dtos.SegmentId).Contains(x));
            dto.Segments.ForEach(sId => quest.Segments.Add(new QuestSegment { QuestId = quest.Id, SegmentId = sId }));

            //Update Goals
            quest.Goals.ToList().RemoveAll(x => !dto.Goals.Where(dtog => dtog.GoalId.HasValue).Select(y => y.GoalId).Contains(x.Id));
            dto.Goals.Where(x => x.GoalId.HasValue).ToList().ForEach(x =>
            {
                var cg = quest.Goals.First(g => g.Id == x.GoalId);
                cg.Title = x.Title;
                cg.IsCommentRequired = x.IsCommentRequired;
            });
            dto.Goals.Where(x => !x.GoalId.HasValue).ToList().ForEach(x =>
            {
                quest.Goals.Add(new QuestGoal { QuestId = quest.Id, Title = x.Title, IsCommentRequired = x.IsCommentRequired });
            });

            //Update Rewards
            quest.Rewards.ToList().RemoveAll(x => !dto.Rewards.Select(dtor => dtor.ItemId).Contains(x.ItemId));
            dto.Rewards.RemoveAll(x => !quest.Rewards.Select(r => r.ItemId).Contains(x.ItemId));
            dto.Rewards.ForEach(r => quest.Rewards.Add(new QuestReward { QuestId = quest.Id, ItemId = r.ItemId, Quantity = r.Quantity }));
        }

        public void CompleteGoal(Guid profileId, QuestGoalCompleteDTO dto)
        {
            var profile = DbContext.Profiles.FirstOrDefault(x => x.Id == profileId);
            var goal = DbContext.QuestGoals.Include(x => x.Quest).FirstOrDefault(x => x.Id == dto.GoalId);

            if (!QuestRules.CanGoalBeCompleted(goal.Quest.Status))
            {
                throw new ApplicationException($"Quest goal {goal.Title} can't be completed");
            }

            DbContext.Add(new QuestGoalCompletion
            {
                GoalId = dto.GoalId,
                ProfileId = profileId,
                Comment = dto.Comment
            });

            // LogsService.LogEvent(new LogCreateDTO
            // {
            //     Event = LogEvents.QuestGoalCompleted,
            //     Data = new Dictionary<string, string>
            //     {
            //         { "timestamp", DateTime.UtcNow.ToString() },
            //         { "profileUsername", profile.Username },
            //         { "questName", goal.Quest.Name },
            //         { "goalTitle", goal.Title }
            //     },
            //     ProfileId = profile.Id,
            // });
        }

        public void CommitToQuest(Guid profileId, QuestCommitDTO dto)
        {
            var quest = DbContext.Quests.FirstOrDefault(x => x.Id == dto.QuestId);

            quest.EnsureNotNull(dto.QuestId);

            DbContext.Add(new QuestCommit
            {
                ProfileId = profileId,
                QuestId = quest.Id
            });
        }

        public void CompleteQuest(QuestCompleteDTO dto)
        {
            var profile = DbContext.Profiles.FirstOrDefault(x => x.Id == dto.ProfileId);
            var quest = DbContext.Quests.FirstOrDefault(x => x.Id == dto.QuestId);

            if (!QuestRules.CanBeCompleted(quest.ActiveUntil, quest.CompletionsRemaining, quest.Status))
            {
                throw new ApplicationException($"Quest {quest.Name} can't be completed");
            }

            quest.CompletionsRemaining -= 1;

            DbContext.Add(new QuestCompletion
            {
                QuestId = dto.QuestId,
                ProfileId = profile.Id
            });

            var questCommit = DbContext.QuestCommits.Where(x => x.ProfileId == profile.Id && x.QuestId == quest.Id).FirstOrDefault();
            if (questCommit == null)
            {
                questCommit = DbContext.Add(new QuestCommit
                {
                    QuestId = dto.QuestId,
                    ProfileId = profile.Id
                }).Entity;
            }
            questCommit.CompletedAt = DateTime.UtcNow;

            // LogsService.LogEvent(new LogCreateDTO
            // {
            //     Event = LogEvents.QuestCompleted,
            //     Data = new Dictionary<string, string>
            //     {
            //         { "timestamp", DateTime.UtcNow.ToString() },
            //         { "profileUsername", profile.Username },
            //         { "questName", quest.Name },
            //         { "questRewardValue", quest.RewardValue.ToString() }
            //     },
            //     ProfileId = profile.Id,
            // });
        }

        public void EndQuest(Guid profileId, int questId)
        {
            var quest = DbContext.Quests.FirstOrDefault(x => x.Id == questId);

            if (!QuestRules.CanBeEnded(quest.Status))
            {
                throw new ApplicationException($"Quest {quest.Name} can't be ended");
            }

            quest.Status = QuestStatuses.Ended;
            quest.EndedAt = DateTime.UtcNow;
        }

        #endregion
    }
}
