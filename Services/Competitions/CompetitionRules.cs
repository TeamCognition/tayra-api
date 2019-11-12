using System;
using Tayra.Common;

namespace Tayra.Services
{
    public static class CompetitionRules
    {
        public static bool IsScheduledEndAtValid(DateTime? startedAt, DateTime? scheduledEndAt)
        {
            if(!startedAt.HasValue || !scheduledEndAt.HasValue)
            {
                return true;
            }

            return startedAt < scheduledEndAt;
        }

        public static bool IsModifyingCompetitorsAllowed(CompetitionStatus competitionStatus)
        {
            return competitionStatus == CompetitionStatus.Draft;
        }

        public static bool IsCompetitorValid(bool isIndividualCompetition, int? profileId, int? teamId)
        {
            if(isIndividualCompetition)
            {
                return profileId > 0 && teamId.HasValue == false;
            }

            return teamId > 0 && profileId.HasValue == false;
        }

        public static bool IsStartingAllowed(CompetitionStatus competitionStatus, int competitorsCount)
        {
            if(competitorsCount < 2)
            {
                return false;
            }

            return competitionStatus == CompetitionStatus.Draft;
        }

        public static bool IsEndingAllowed(CompetitionStatus competitionStatus)
        {
            return competitionStatus == CompetitionStatus.Started;
        }

        public static bool IsStoppingAllowed(CompetitionStatus competitionStatus)
        {
            return competitionStatus == CompetitionStatus.Started;
        }

        public static bool IsDurationValid(TimeSpan duration)
        {
            return duration >= new TimeSpan(1, 0, 0);
        }
    }
}
