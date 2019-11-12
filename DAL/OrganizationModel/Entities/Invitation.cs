using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Firdaws.DAL;
using Tayra.Common;

namespace Tayra.Models.Organizations
{
    public class Invitation : IAuditedEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public Guid Code { get; set; }

        [Required, ForeignKey(nameof(Profile))]
        public int ProfileId { get; set; }

        public InvitationStatus Status { get; set; } = InvitationStatus.Created;

        [MaxLength(1000)]
        public string EmailId { get; set; }

        public virtual Profile Profile { get; set; }

        [NotMapped]
        public bool IsActive
        {
            get
            {
                return Status != InvitationStatus.Accepted &&
                       Status != InvitationStatus.Cancelled &&
                       Status != InvitationStatus.Expired;
            }
        }

        #region IAuditedEntity

        public DateTime Created { get; set; }
        public DateTime? LastModified { get; set; }
        public int CreatedBy { get; set; }
        public int? LastModifiedBy { get; set; }

        #endregion
    }
}