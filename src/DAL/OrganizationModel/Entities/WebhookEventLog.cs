using System;
using Cog.DAL;
using Tayra.Common;

namespace Tayra.Models.Organizations
{
    public class WebhookEventLog : ITimeStampedEntity
    {
        public int Id { get; set; }

        public string Data { get; set; }

        public IntegrationType IntegrationType { get; set; }

        #region ITimeStampedEntity

        public DateTime Created { get; set; }
        public DateTime? LastModified { get; set; }

        #endregion
    }
}
