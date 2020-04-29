using System;
using Firdaws.DAL;

namespace Tayra.Models.Organizations
{
    public class ChallengeReward : ITimeStampedEntity
    {       
        public int ChallengeId { get; set; }
        public virtual Challenge Challenge { get; set; }

        public int ItemId { get; set; }
        public virtual Item Item { get; set; }

        public int Quantity { get; set; }

        #region ITimeStampedEntity

        public DateTime Created { get; set; }
        public DateTime? LastModified { get; set; }

        #endregion
    }
}