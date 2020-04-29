using System;
using Firdaws.DAL;

namespace Tayra.Models.Organizations
{
    public class LogChange : IAuditedEntity
    {
        public int Id { get; set; }

        public int LogId { get; set; }
        public Log Log { get; set; }

        //change template?

        #region IAuditedEntity

        public DateTime Created { get; set; }
        public DateTime? LastModified { get; set; }
        public int CreatedBy { get; set; }
        public int? LastModifiedBy { get; set; }

        #endregion
    }
}
