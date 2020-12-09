using System;
using Cog.DAL;

namespace Tayra.Models.Organizations
{
    public class SegmentReportDaily : ITimeStampedEntity
    {
        public int DateId { get; set; }
        public int IterationCount { get; set; }

        public Guid SegmentId { get; set; }
        public virtual Segment Segment { get; set; }

        public int TaskCategoryId { get; set; }
        //public virtual TaskCategory TaskCategory { get; set; }

        public int ComplexityChange { get; set; }
        public int ComplexityTotal { get; set; }

        public float CompanyTokensEarnedChange { get; set; }
        public float CompanyTokensEarnedTotal { get; set; }

        public float CompanyTokensSpentChange { get; set; }
        public float CompanyTokensSpentTotal { get; set; }

        public float EffortScoreChange { get; set; }
        public float EffortScoreTotal { get; set; }

        public int PraisesGivenChange { get; set; }
        public int PraisesGivenTotal { get; set; }

        public int PraisesReceivedChange { get; set; }
        public int PraisesReceivedTotal { get; set; }

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

        public int ItemsBoughtChange { get; set; }
        public int ItemsBoughtTotal { get; set; }

        public int ItemsGiftedChange { get; set; }

        public int ItemsDisenchantedChange { get; set; }

        public int ItemsCreatedChange { get; set; }


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
