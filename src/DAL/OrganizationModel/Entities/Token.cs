using System;
using Cog.DAL;
using Tayra.Common;

namespace Tayra.Models.Organizations
{
    public class Token : Entity<Guid>, IAuditedEntity
    {
        public string Name { get; set; }
        public string Symbol { get; set; }
        public TokenType Type { get; set; }

        public string SupplyAddress { get; set; }

        #region IAuditedEntity

        public DateTime Created { get; set; }
        public DateTime? LastModified { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid? LastModifiedBy { get; set; }

        #endregion
    }
}