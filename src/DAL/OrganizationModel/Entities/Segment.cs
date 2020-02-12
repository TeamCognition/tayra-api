using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Firdaws.DAL;

namespace Tayra.Models.Organizations
{
    public class Segment : IAuditedEntity, IArchivableEntity
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(50)]
        public string Key { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(2000)]
        public string Avatar { get; set; }

        [MaxLength(50)]
        public string Timezone { get; set; }

        [MaxLength(4000)]
        public string DataStore { get; set; }

        [MaxLength(4000)]
        public string DataWarehouse { get; set; }

        public virtual ICollection<Team> Teams { get; set; }
        public virtual ICollection<Challenge> Challenges { get; set; }
        public virtual ICollection<Integration> Integrations { get; set; }
        public virtual ICollection<ProfileAssignment> Members { get; set; }
        public virtual ICollection<ShopPurchase> ShopPurchases { get; set; }
        public virtual ICollection<ActionPointSegment> ActionPoints { get; set; }
        public virtual ICollection<SegmentReportDaily> ReportsDaily { get; set; }
        public virtual ICollection<SegmentReportWeekly> ReportsWeekly { get; set; }

        #region IAuditedEntity

        public DateTime Created { get; set; }
        public DateTime? LastModified { get; set; }
        public int CreatedBy { get; set; }
        public int? LastModifiedBy { get; set; }

        #endregion
    }
}