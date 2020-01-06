using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Firdaws.DAL;

namespace Tayra.Models.Organizations
{
    public class Team : IAuditedEntity, IArchivableEntity
    {
        public int Id { get; set; }

        public int SegmentId { get; set; }
        public virtual Segment Segment { get; set; }

        /// <summary>
        /// null key means it's a team for unassigned memebers of a segment
        /// </summary>
        [MaxLength(50)]
        public string Key { get; set; }

        [Required]
        public string Name { get; set; }
        //public bool IsCompetitorOnly { get; set; }

        [MaxLength(50)]
        public string AvatarColor { get; set; }

        public virtual ICollection<TeamMember> Members { get; set; }
        public virtual ICollection<TeamReportDaily> ReportsDaily { get; set; }
        public virtual ICollection<TeamReportWeekly> ReportsWeekly { get; set; }

        #region IAuditedEntity

        public DateTime Created { get; set; }
        public DateTime? LastModified { get; set; }
        public int CreatedBy { get; set; }
        public int? LastModifiedBy { get; set; }

        #endregion
    }
}