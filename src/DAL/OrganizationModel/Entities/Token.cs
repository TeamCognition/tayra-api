using System;
using Firdaws.DAL;
using Tayra.Common;

namespace Tayra.Models.Organizations
{
    public class Token : IAuditedEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public string Symbol { get; set; }
        public TokenType Type { get; set; }

        public string SupplyAddress { get; set; }

        #region IAuditedEntity

        public DateTime Created { get; set; }
        public DateTime? LastModified { get; set; }
        public int CreatedBy { get; set; }
        public int? LastModifiedBy { get; set; }

        #endregion
    }
}