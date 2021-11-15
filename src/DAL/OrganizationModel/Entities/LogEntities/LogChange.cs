using System;
using Cog.DAL;

namespace Tayra.Models.Organizations
{
    public class LogChange : IAuditedEntity
    {
        public int Id { get; set; }

        public Guid LogId { get; set; }
        public Log Log { get; set; }

        //change template?

        #region IAuditedEntity

        public DateTime Created { get; set; }
        public DateTime? LastModified { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid? LastModifiedBy { get; set; }

        #endregion
    }
}
