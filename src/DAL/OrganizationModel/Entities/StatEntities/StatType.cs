using System;
using Firdaws.DAL;
using Tayra.Common;

namespace Tayra.Models.Organizations
{
    public class StatType : IAuditedEntity
    {
        public int Id { get; set; } //what about custom values from jira

        public string Name { get; set; }
        public IntegrationType IntegrationType { get; set; }

        #region IAuditedEntity

        public DateTime Created { get; set; }
        public DateTime? LastModified { get; set; }
        public int CreatedBy { get; set; }
        public int? LastModifiedBy { get; set; }

        #endregion
    }
}
