using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Firdaws.DAL;
using Tayra.Common;

namespace Tayra.Models.Organizations
{
    public class Integration : IAuditedEntity
    {
        [Key]
        public int Id { get; set; }

        public int ProjectId { get; set; }

        public IntegrationType Type { get; set; }

        public virtual ICollection<IntegrationField> Fields { get; set; }

        #region IAuditedEntity

        public DateTime Created { get; set; }
        public DateTime? LastModified { get; set; }
        public int CreatedBy { get; set; }
        public int? LastModifiedBy { get; set; }

        #endregion
    }
}
