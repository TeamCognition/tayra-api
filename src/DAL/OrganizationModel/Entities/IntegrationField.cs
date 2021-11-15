using System;
using System.ComponentModel.DataAnnotations;
using Cog.DAL;

namespace Tayra.Models.Organizations
{
    public class IntegrationField : EntityGuidId, ITimeStampedEntity, IUserStampedEntity
    {
        public Integration Integration { get; set; }
        public Guid IntegrationId { get; set; }

        [MaxLength(50)]
        public string Key { get; set; }

        public string Value { get; set; }

        #region ITimeStampedEntity and IUserStampedEntity

        public DateTime Created { get; set; }
        public DateTime? LastModified { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid? LastModifiedBy { get; set; }

        #endregion
    }
}