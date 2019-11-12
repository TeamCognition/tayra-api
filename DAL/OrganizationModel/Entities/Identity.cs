using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Firdaws.DAL;

namespace Tayra.Models.Organizations
{
    public class Identity : IAuditedEntity
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(200)]
        public string Username { get; set; }

        [Required]
        public byte[] Password { get; set; }

        [Required]
        public byte[] Salt { get; set; }

        public virtual ICollection<Profile> Profiles { get; set; }
        public virtual ICollection<IdentityEmail> Emails { get; set; }

        #region IAuditedEntity

        public DateTime Created { get; set; }
        public DateTime? LastModified { get; set; }
        public int CreatedBy { get; set; }
        public int? LastModifiedBy { get; set; }

        #endregion
    }
}