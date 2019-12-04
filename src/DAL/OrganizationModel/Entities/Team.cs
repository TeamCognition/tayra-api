using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Firdaws.DAL;

namespace Tayra.Models.Organizations
{
    public class Team : IAuditedEntity
    {
        public int Id { get; set; }

        [MaxLength(50)] //TODO:required
        public string Key { get; set; }
        public string Name { get; set; }
        //public bool IsCompetitorOnly { get; set; }

        [MaxLength(2000)]
        public string Avatar { get; set; }

        public int ProjectId { get; set; }
        public virtual Project Project { get; set; }

        public DateTime? ArchivedAt { get; set; }

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