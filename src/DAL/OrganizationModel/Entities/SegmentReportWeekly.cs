using System;
using Cog.DAL;

namespace Tayra.Models.Organizations
{
    public class SegmentReportWeekly : ITimeStampedEntity
    {
        public int DateId { get; set; }
        public int IterationCount { get; set; }

        public int SegmentId { get; set; }
        public virtual Segment Segment { get; set; }

        public int TaskCategoryId { get; set; }
        //public virtual TaskCategory TaskCategory { get; set; }

        public int ComplexityChange { get; set; }
        public float ComplexityAverage { get; set; }

        public float CompanyTokensEarnedChange { get; set; }
        public float CompanyTokensEarnedAverage { get; set; }

        public float CompanyTokensSpentChange { get; set; }
        public float CompanyTokensSpentAverage { get; set; }

        public float EffortScoreChange { get; set; }
        public float EffortScoreAverage { get; set; }

        public int PraisesGivenChange { get; set; }
        public float PraisesGivenAverage { get; set; }

        public int PraisesReceivedChange { get; set; }
        public float PraisesReceivedAverage { get; set; }

        public int AssistsChange { get; set; }
        public float AssistsAverage { get; set; }

        public int TasksCompletedChange { get; set; }
        public float TasksCompletedAverage { get; set; }

        public int TurnoverChange { get; set; }
        public float TurnoverAverage { get; set; }

        public float ErrorChange { get; set; }
        public float ErrorAverage { get; set; }

        public float ContributionChange { get; set; }
        public float ContributionAverage { get; set; }

        public int SavesChange { get; set; }
        public float SavesAverage { get; set; }

        public int TacklesChange { get; set; }
        public float TacklesAverage { get; set; }

        public int TasksCompletionTimeChange { get; set; }
        public int TasksCompletionTimeAverage { get; set; }

        public int RangeChange { get; set; }
        public int RangeAverage { get; set; }


        public int ItemsBoughtChange { get; set; }

        public int ItemsGiftedChange { get; set; }

        public int ItemsDisenchantedChange { get; set; }

        public int ItemsCreatedChange { get; set; }

        #region Weekly Only Stats

        public float OImpactAverage { get; set; }
        public float OImpactAverageTotal { get; set; }

        public float DImpactAverage { get; set; }
        public float DImpactAverageTotal { get; set; }

        public float PowerAverage { get; set; }
        public float PowerAverageTotal { get; set; }

        public float SpeedAverage { get; set; }
        public float SpeedAverageTotal { get; set; }

        public float HeatAverageTotal { get; set; }

        #endregion

        #region Segment Stats

        public int MembersCountTotal { get; set; }
        public int NonMembersCountTotal { get; set; }

        #endregion

        #region ITimeStampedEntity

        public DateTime Created { get; set; }
        public DateTime? LastModified { get; set; }

        #endregion
    }
}
