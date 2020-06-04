using System;
using Cog.DAL;

namespace Tayra.Models.Organizations
{
    public class CompetitionReward : IAuditedEntity
    {
        public int Id { get; set; }

        public float TokenValue { get; set; }

        public int TokenId { get; set; }
        public virtual Token Token { get; set; }

        public int ItemId { get; set; }
        public virtual Item Item { get; set; }

        public int CompetitionId { get; set; }
        public virtual Competition Competition { get; set; }

        #region IAuditedEntity

        public DateTime Created { get; set; }
        public DateTime? LastModified { get; set; }
        public int CreatedBy { get; set; }
        public int? LastModifiedBy { get; set; }

        #endregion
    }
}