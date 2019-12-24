using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Firdaws.DAL;

namespace Tayra.Models.Organizations
{
    public class Segment : IAuditedEntity
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(50)]
        public string Key { get; set; }

        [ForeignKey(nameof(Organization))]
        public int OrganizationId { get; set; }

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

        public DateTime? ArchivedAt { get; set; }

        public virtual Organization Organization { get; set; }

        public virtual ICollection<Challenge> Challenges { get; set; }
        public virtual ICollection<SegmentMember> Members { get; set; }
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