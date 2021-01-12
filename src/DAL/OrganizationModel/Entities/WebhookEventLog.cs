using System;
using Cog.DAL;
using Tayra.Common;

namespace Tayra.Models.Organizations
{
    public class WebhookEventLog : EntityGuidId, ITimeStampedEntity
    {
        public string Data { get; set; }

        public IntegrationType IntegrationType { get; set; }

        #region ITimeStampedEntity

        public DateTime Created { get; set; }
        public DateTime? LastModified { get; set; }

        #endregion
    }
}
