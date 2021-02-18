using System;
using Cog.DAL;
using Tayra.Common;

namespace Tayra.Models.Organizations
{
    public class Log : EntityGuidId, ITimeStampedEntity
    {
        public string Data { get; set; }
        
        public bool IsAssistedByTayra { get; set; }

        public string Description { get; set; } 
        
        public string ExternalUrl { get; set; } 
        
        public LogEvents Event { get; set; }

        #region ITimeStampedEntity

        public DateTime Created { get; set; }
        public DateTime? LastModified { get; set; }

        #endregion
    }
}
