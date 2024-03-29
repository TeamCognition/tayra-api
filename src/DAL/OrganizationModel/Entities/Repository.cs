using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Cog.DAL;

namespace Tayra.Models.Organizations
{
    public class Repository : EntityGuidId, IAuditedEntity
    {
        public Guid? TeamId { get; set; }
        public virtual Team Team { get; set; }

        public string ExternalId { get; set; }
        [Required]
        public string IntegrationInstallationId { get; set; }
        public string Name { get; set; }

        public string NameWithOwner { get; set; }
        public string PrimaryLanguage { get; set; }
        public string ExternalUrl { get; set; }

        #region IAuditedEntity

        public DateTime Created { get; set; }
        public DateTime? LastModified { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid? LastModifiedBy { get; set; }

        #endregion
    }
}