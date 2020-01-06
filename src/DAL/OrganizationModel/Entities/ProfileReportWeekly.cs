﻿using System;
using Firdaws.DAL;

namespace Tayra.Models.Organizations
{
    public class ProfileReportWeekly : ITimeStampedEntity
    {
        public int DateId { get; set; }
        public int IterationCount { get; set; }

        public int ProfileId { get; set; }
        public virtual Profile Profile { get; set; }

        public int TaskCategoryId { get; set; }
        public virtual TaskCategory TaskCategory { get; set; }

        public int ComplexityChange { get; set; }
        public float ComplexityTotalAverage { get; set; }

        public float CompanyTokensChange { get; set; }
        public float CompanyTokensTotalAverage { get; set; }

        public float EffortScoreChange { get; set; }
        public float EffortScoreTotalAverage { get; set; }
        
        public int OneUpsGivenChange { get; set; }
        public float OneUpsGivenTotalAverage { get; set; }

        public int OneUpsReceivedChange { get; set; }
        public float OneUpsReceivedTotalAverage { get; set; }

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

        public int RangeChange { get; set; }
        public int RangeTotalAverage { get; set; }

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