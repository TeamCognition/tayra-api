using System;
using Firdaws.DAL;

namespace Tayra.Models.Organizations
{
    public class ProjectArea : IAuditedEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }

        #region IAuditedEntity

        public DateTime Created { get; set; }
        public DateTime? LastModified { get; set; }
        public int CreatedBy { get; set; }
        public int? LastModifiedBy { get; set; }

        #endregion
    }
}
