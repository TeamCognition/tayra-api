using System;
using Firdaws.DAL;

namespace Tayra.Models.Organizations
{
    public class WebhookEventLog : ITimeStampedEntity
    {
        public int Id { get; set; }

        public string Data { get; set; }

        #region ITimeStampedEntity

        public DateTime Created { get; set; }
        public DateTime? LastModified { get; set; }

        #endregion
    }
}
