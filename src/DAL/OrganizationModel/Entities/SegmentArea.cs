using System;
using Cog.DAL;

namespace Tayra.Models.Organizations
{
    public class SegmentArea : IAuditedEntity
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
