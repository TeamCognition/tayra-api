using System;
using Firdaws.DAL;
using Tayra.Common;

namespace Tayra.Models.Organizations
{
    //Interrupts, Helps, Plusses are missing
    public class ProfileReportDaily : ITimeStampedEntity
    {
        public int DateId { get; set; }
        public int IterationCount { get; set; }
        public ProfileRoles ProfileRole { get; set; }

        public int ProfileId { get; set; }
        public virtual Profile Profile { get; set; }

        public int SegmentId { get; set; }
        public virtual Segment Segment { get; set; }

        public int TaskCategoryId { get; set; }
        public virtual TaskCategory TaskCategory { get; set; }

        public int ComplexityChange { get; set; }
        public int ComplexityTotal { get; set; }

        public float CompanyTokensEarnedChange { get; set; }
        public float CompanyTokensEarnedTotal { get; set; }

        public float CompanyTokensSpentChange { get; set; }
        public float CompanyTokensSpentTotal { get; set; }

        public float EffortScoreChange { get; set; }
        public float EffortScoreTotal { get; set; }

        public int OneUpsGivenChange { get; set; }
        public int OneUpsGivenTotal { get; set; }

        public int OneUpsReceivedChange { get; set; }
        public int OneUpsReceivedTotal { get; set; }

        public int AssistsChange { get; set; }
        public int AssistsTotal { get; set; }

        public int TasksCompletedChange { get; set; }
        public int TasksCompletedTotal { get; set; }

        public int TurnoverChange { get; set; }
        public int TurnoverTotal { get; set; }

        public float ErrorChange { get; set; }
        public float ErrorTotal { get; set; }

        public float ContributionChange { get; set; }
        public float ContributionTotal { get; set; }

        public int SavesChange { get; set; }
        public int SavesTotal { get; set; }

        public int TacklesChange { get; set; }
        public int TacklesTotal { get; set; }

        public int TasksCompletionTimeChange { get; set; }
        public int TasksCompletionTimeTotal { get; set; }

        public int InventoryCountTotal { get; set; }
        public float InventoryValueTotal { get; set; }

        public int ItemsBoughtChange { get; set; }
        public int ItemsBoughtTotal { get; set; }

        public int ItemsGiftedChange { get; set; }
        public int ItemsGiftedTotal { get; set; }

        public int ItemsDisenchantedChange { get; set; }
        public int ItemsDisenchantedTotal { get; set; }

        public int ItemsCreatedChange { get; set; }
        public int ItemsCreatedTotal { get; set; }

        public string ActivityChartJson { get; set; }

        #region ITimeStampedEntity

        public DateTime Created { get; set; }
        public DateTime? LastModified { get; set; }

        #endregion
    }
}
