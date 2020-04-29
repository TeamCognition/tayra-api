using System;
using System.Collections.Generic;
using System.Linq;
using Firdaws.Core;
using Firdaws.DAL;
using Microsoft.EntityFrameworkCore;
using MoreLinq;
using Tayra.Common;
using Tayra.Models.Organizations;

namespace Tayra.Services
{
    public class CompetitionsService : BaseService<OrganizationDbContext>, ICompetitionsService
    {
        #region Constructor

        public CompetitionsService(OrganizationDbContext dbContext) : base(dbContext)
        {
        }

        #endregion

        #region Public Methods

        public GridData<CompetitionViewGridDTO> GetSegmentCompetitionsGrid(int segmentId, CompetitionViewGridParams gridParams)
        {
            var query = from c in DbContext.Competitions
                        where c.SegmentId == segmentId
                        join twinner in DbContext.Competitors on c.WinnerId equals twinner.Id into g
                        from winner in g.DefaultIfEmpty()
                        select new CompetitionViewGridDTO
                        {
                            Id = c.Id,
                            IsIndividual = c.IsIndividual,
                            Name = c.Name,
                            Status = c.Status,
                            Created = c.Created,
                            EndedAt = c.EndedAt,
                            Winner = new CompetitionViewGridDTO.CompetitionWinner
                            {
                                Username = winner.DisplayName ?? winner.Profile.Username ?? winner.Team.Name,
                                Avatar = winner.Profile.Avatar ?? winner.Team.AvatarColor,
                                Score = winner.ScoreValue
                            }
                        };

            GridData<CompetitionViewGridDTO> gridData = query.GetGridData(gridParams);

            return gridData;
        }

        public List<CompetitionViewCompetitorDTO> GetActiveCompetitions(int profileId)
        {
            return (from c in DbContext.Competitors
                    let teamIds = (DbContext.ProfileAssignments.Where(x => x.ProfileId == profileId).Select(x => x.TeamId).ToList())
                    where c.Competition.Status == CompetitionStatus.Started
                    where c.ProfileId == profileId || (c.TeamId.HasValue && teamIds.Contains(c.TeamId.Value))

                    select new CompetitionViewCompetitorDTO
                    {
                        Id = c.CompetitionId,
                        IsIndividual = c.Competition.IsIndividual,
                        Name = c.Competition.Name,
                        Status = c.Competition.Status,
                        TeamName = c.Team.Name,
                        CompetitorName = c.Competition.Name ?? c.Profile.Username ?? c.Team.Name
                    }).ToList();
        }

        public GridData<CompetitionGridDTO> GetGridData(CompetitionGridParams gridParams)
        {
            var competition = DbContext.Competitions.FirstOrDefault(x => x.Id == gridParams.CompetitionId);

            var scope = DbContext.Competitors
                .Where(x => x.CompetitionId == gridParams.CompetitionId);

            IQueryable<CompetitionGridDTO> query;
            if (competition.IsIndividual)
            {
                query = from c in scope
                        from tag in DbContext.ProfileInventoryItems.Where(x => x.ProfileId == c.ProfileId && x.IsActive).DefaultIfEmpty()
                        select new CompetitionGridDTO
                        {
                            Avatar = c.Profile.Avatar,
                            Name = c.DisplayName ?? c.Profile.Username,
                            Subtitle = tag.Item.Name,
                            Points = Math.Round(c.ScoreValue, 2)
                        };
            }
            else
            {
                query = scope
                    .Select(x => new CompetitionGridDTO
                    {
                        TeamKey = x.Team.Key,
                        Avatar = x.Team.AvatarColor,
                        Name = x.DisplayName ?? x.Team.Name,
                        Subtitle = x.Team.Members.Count() + " Members",
                        Points = Math.Round(x.ScoreValue, 2),
                    });
            }

            GridData<CompetitionGridDTO> gridData = query.GetGridData(gridParams);

            return gridData;
        }

        public void Create(int segmentId, CompetitionCreateDTO dto)
        {
            var token = DbContext.Tokens.FirstOrDefault(x => x.Type == (dto.Token ?? TokenType.CompanyToken));
            var competition = new Competition
            {
                IsIndividual = dto.IsIndividual,
                Name = dto.Name,
                Status = dto.Status ?? CompetitionStatus.Draft,
                Type = dto.Type ?? CompetitionType.Leaderboard,
                Theme = dto.Theme ?? CompetitionTheme.LeaderboardClouds,
                TokenId = token.Id,
                SegmentId = segmentId
            };

            if (!CompetitionRules.IsScheduledEndAtValid(competition.StartedAt, competition.ScheduledEndAt))
            {
                throw new ApplicationException($"Invalid {nameof(Competition.ScheduledEndAt)}");
            }

            DbContext.Add(competition);
        }

        public void AddCompetitors(int competitionId, IList<CompetitionAddCompetitorDTO> dto)
        {
            var competition = DbContext.Competitions.FirstOrDefault(x => x.Id == competitionId);

            if (!CompetitionRules.IsModifyingCompetitorsAllowed(competition.Status))
            {
                throw new ApplicationException($"Competition already started or has ended");
            }

            foreach (var c in dto)
            {
                if (!CompetitionRules.IsCompetitorValid(competition.IsIndividual, c.ProfileId, c.TeamId))
                {
                    throw new ApplicationException($"Invalid competitor pId: {c.ProfileId} tId: {c.TeamId}");
                }

                DbContext.Competitors.Add(new Competitor
                {
                    CompetitionId = competitionId,
                    ProfileId = c.ProfileId,
                    TeamId = c.TeamId,
                    DisplayName = c.DisplayName
                });
            }
        }

        public void StartCompetition(int competitionId, CompetitionStartDTO dto)
        {
            var competition = DbContext.Competitions.FirstOrDefault(x => x.Id == competitionId);
            var competitorsCount = DbContext.Competitors.Count(x => x.CompetitionId == competitionId);

            if (!CompetitionRules.IsDurationValid(dto.Duration()))
            {
                throw new ApplicationException($"Competition duration must be at least 1 hour");
            }

            if (!CompetitionRules.IsStartingAllowed(competition.Status, competitorsCount))
            {
                throw new ApplicationException($"Competition already started or has ended or competitors count is less than 2");
            }

            competition.Status = CompetitionStatus.Started;
        }

        public void EndCompetition(int competitionId)
        {
            var competition = DbContext.Competitions
                                .Include(x => x.Competitors)
                                .FirstOrDefault(x => x.Id == competitionId);

            if (!CompetitionRules.IsEndingAllowed(competition.Status))
            {
                throw new ApplicationException($"Competition is not active");
            }

            var winner = competition.Competitors.MaxBy(x => x.ScoreValue).FirstOrDefault();

            competition.WinnerId = winner.Id;
            competition.Status = CompetitionStatus.Completed;
            competition.EndedAt = DateTime.UtcNow;

            var winnerProfiles = (from p in DbContext.CompetitorScores
                                  where p.CompetitorId == winner.Id
                                  group p by p.ProfileId into g
                                  select new
                                  {
                                      ProfileId = g.Key,
                                      Score = g.Sum(x => x.Value)
                                  }).ToList();

            var tokensService = new TokensService(DbContext);
            foreach (var p in winnerProfiles)
            {
                tokensService.CreateTransaction(
                    TokenType.CompanyToken,
                    p.ProfileId,
                    competition.TokenRewardValue / (winner.ScoreValue / p.Score),
                    TransactionReason.Manual,
                    ClaimBundleTypes.CompetitionWinner);
            }

            if (competition.RepeatWhenCompleted)
            {
                var repeatingComp = new Competition
                {
                    PreviousCompetitionId = competition.Id,
                    Status = CompetitionStatus.Started,
                    RepeatWhenCompleted = true,
                    RepeatCount = competition.RepeatCount + 1,
                    TokenRewardValue = competition.TokenRewardValue,
                    Name = competition.Name,
                    IsIndividual = competition.IsIndividual,
                    SegmentId = competition.SegmentId,
                    TokenId = competition.TokenId,
                    Type = competition.Type,
                    Theme = competition.Theme,
                    CreatedBy = competition.CreatedBy,
                    LastModifiedBy = competition.LastModifiedBy,
                    StartedAt = competition.ScheduledEndAt,
                    ScheduledEndAt = competition.ScheduledEndAt + (competition.ScheduledEndAt - competition.StartedAt)
                };

                repeatingComp.Competitors = new List<Competitor>();
                foreach (var c in competition.Competitors)
                {
                    repeatingComp.Competitors.Add(new Competitor
                    {
                        CompetitionId = c.CompetitionId,
                        DisplayName = c.DisplayName,
                        CreatedBy = c.CreatedBy,
                        LastModifiedBy = c.LastModifiedBy,
                        ProfileId = c.ProfileId,
                        TeamId = c.TeamId
                    });
                }
                DbContext.Competitions.Add(repeatingComp);
            }

        }

        public void StopCompetition(int competitionId)
        {
            var competition = DbContext.Competitions.FirstOrDefault(x => x.Id == competitionId);

            if (!CompetitionRules.IsStoppingAllowed(competition.Status))
            {
                throw new ApplicationException($"Competition is not active");
            }

            competition.Status = CompetitionStatus.Stopped;
            competition.EndedAt = DateTime.UtcNow;
        }

        #endregion

        #region Private Methods

        public void RefreshCompetitorsTokenValue(int competitionId)
        {
            List<Competitor> competitors = DbContext.Competitors
                .Include(x => x.Competition)
                .Include(x => x.Profile)
                .Where(x => x.CompetitionId == competitionId)
                .ToList();

            var competition = competitors.Select(x => x.Competition).FirstOrDefault();

            IQueryable<TokenTransaction> tokensScope = DbContext.TokenTransactions
                .Where(x => x.TokenId == competition.TokenId)
                .Where(x => x.Created >= competition.StartedAt)
                .Where(x => competitors.Select(c => c.ProfileId).Contains(x.ProfileId));

            if (competition.EndedAt.HasValue)
                tokensScope = tokensScope.Where(x => x.Created <= competition.EndedAt);

            var tokenBalances = from c in tokensScope
                                group c by c.ProfileId into g
                                select new { ProfileId = g.Key, TokenValue = g.Sum(x => x.Value) };

        }

        #endregion
    }
}
