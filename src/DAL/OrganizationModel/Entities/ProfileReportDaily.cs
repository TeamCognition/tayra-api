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

        public int TaskCategoryId { get; set; }
        public virtual TaskCategory TaskCategory { get; set; }

        public int ComplexityChange { get; set; }
        public int ComplexityTotal { get; set; }

        public float CompanyTokensChange { get; set; }
        public float CompanyTokensTotal { get; set; }

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

        #region ITimeStampedEntity

        public DateTime Created { get; set; }
        public DateTime? LastModified { get; set; }

        #endregion
    }
}
