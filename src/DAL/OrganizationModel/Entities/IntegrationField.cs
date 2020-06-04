using System;
using System.ComponentModel.DataAnnotations;
using Cog.DAL;

namespace Tayra.Models.Organizations
{
    public class IntegrationField : IAuditedEntity
    {
        [Key]
        public int Id { get; set; }

        public Integration Integration { get; set; }
        public int IntegrationId { get; set; }

        [MaxLength(50)]
        public string Key { get; set; }

        public string Value { get; set; }

        #region IAuditedEntity

        public DateTime Created { get; set; }
        public DateTime? LastModified { get; set; }
        public int CreatedBy { get; set; }
        public int? LastModifiedBy { get; set; }

        #endregion
    }
}