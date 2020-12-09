using System;
using Cog.DAL;
using Tayra.Common;

namespace Tayra.Models.Organizations
{
    public class ProfileReportWeekly : ITimeStampedEntity
    {
        public int DateId { get; set; }
        public int IterationCount { get; set; }
        public ProfileRoles ProfileRole { get; set; }

        public Guid ProfileId { get; set; }
        public virtual Profile Profile { get; set; }

        public Guid SegmentId { get; set; }
        public virtual Segment Segment { get; set; }

        public int TaskCategoryId { get; set; }
        //public virtual TaskCategory TaskCategory { get; set; }

        public int ComplexityChange { get; set; }
        public float ComplexityTotalAverage { get; set; }

        public float CompanyTokensEarnedChange { get; set; }
        public float CompanyTokensEarnedTotalAverage { get; set; }

        public float CompanyTokensSpentChange { get; set; }
        public float CompanyTokensSpentTotalAverage { get; set; }

        public float EffortScoreChange { get; set; }
        public float EffortScoreTotalAverage { get; set; }

        public int PraisesGivenChange { get; set; }
        public float PraisesGivenTotalAverage { get; set; }

        public int PraisesReceivedChange { get; set; }
        public float PraisesReceivedTotalAverage { get; set; }

        public int AssistsChange { get; set; }
        public float AssistsTotalAverage { get; set; }

        public int TasksCompletedChange { get; set; }
        public float TasksCompletedTotalAverage { get; set; }

        public int TurnoverChange { get; set; }
        public float TurnoverTotalAverage { get; set; }

        public float ErrorChange { get; set; }
        public float ErrorTotalAverage { get; set; }

        public float ContributionChange { get; set; }
        public float ContributionTotalAverage { get; set; }

        public int SavesChange { get; set; }
        public float SavesTotalAverage { get; set; }

        public int TacklesChange { get; set; }
        public float TacklesTotalAverage { get; set; }

        public int TasksCompletionTimeChange { get; set; }
        public int TasksCompletionTimeAverage { get; set; }

        public int RangeChange { get; set; }
        public int RangeTotalAverage { get; set; }

        public int InventoryCountTotal { get; set; }
        public float InventoryValueTotal { get; set; }

        public int ItemsBoughtChange { get; set; }

        public int ItemsGiftedChange { get; set; }

        public int ItemsDisenchantedChange { get; set; }

        public int ItemsCreatedChange { get; set; }


        #region Weekly Only Stats

        public float OImpactAverage { get; set; }
        public float OImpactTotalAverage { get; set; }

        public float DImpactAverage { get; set; }
        public float DImpactTotalAverage { get; set; }

        public float PowerAverage { get; set; }
        public float PowerTotalAverage { get; set; }

        public float SpeedAverage { get; set; }
        public float SpeedTotalAverage { get; set; }

        public float Heat { get; set; }
        public float HeatIndex { get; set; }

        #endregion

        #region ITimeStampedEntity

        public DateTime Created { get; set; }
        public DateTime? LastModified { get; set; }

        #endregion
    }
}
