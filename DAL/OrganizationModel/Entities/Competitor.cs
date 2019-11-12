using System;
using System.ComponentModel.DataAnnotations;
using Firdaws.DAL;

namespace Tayra.Models.Organizations
{
    public class Competitor : ITimeStampedEntity, IUserStampedEntity
    {
        [Key]
        public int Id { get; set; }

        public int CompetitionId { get; set; }
        public virtual Competition Competition { get; set; }

        public int? ProfileId { get; set; }
        public virtual Profile Profile { get; set; }

        public int? TeamId { get; set; }
        public virtual Team Team { get; set; }

        public string DisplayName { get; set; }

        public double ScoreValue { get; set; }

        #region ITimeAndIUser

        public DateTime Created { get; set; }
        public DateTime? LastModified { get; set; }
        public int CreatedBy { get; set; }
        public int? LastModifiedBy { get; set; }

        #endregion
    }
}