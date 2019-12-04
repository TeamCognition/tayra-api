using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Firdaws.DAL;

namespace Tayra.Models.Catalog
{
    public class Identity : IAuditedEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public byte[] Password { get; set; }

        [Required]
        public byte[] Salt { get; set; }

        public virtual ICollection<IdentityEmail> Emails { get; set; }

        #region IAuditedEntity

        public DateTime Created { get; set; }
        public DateTime? LastModified { get; set; }
        public int CreatedBy { get; set; }
        public int? LastModifiedBy { get; set; }

        #endregion
    }
}