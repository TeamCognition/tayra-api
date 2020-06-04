using System;
using System.ComponentModel.DataAnnotations;
using Cog.DAL;

namespace Tayra.Models.Organizations
{
    public class CompetitorScore : ITimeStampedEntity
    {
        [Key]
        public int Id { get; set; }

        public int CompetitorId { get; set; }
        public virtual Competitor Competitor { get; set; }

        public int ProfileId { get; set; }
        public virtual Profile Profile { get; set; }

        public double Value { get; set; }

        public int? TeamId { get; set; }
        public virtual Team Team { get; set; }

        public int CompetitionId { get; set; }
        public virtual Competition Competition { get; set; }

        #region ITimeStampedEntity

        public DateTime Created { get; set; }
        public DateTime? LastModified { get; set; }

        #endregion
    }
}
