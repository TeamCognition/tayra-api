using System;
using Firdaws.DAL;
using Tayra.Common;

namespace Tayra.Models.Organizations
{
    public class Log : ITimeStampedEntity
    {
        public int Id { get; set; }

        public string Data { get; set; }

        public LogEvents Event { get; set; }

        #region ITimeStampedEntity

        public DateTime Created { get; set; }
        public DateTime? LastModified { get; set; }

        #endregion
    }
}
