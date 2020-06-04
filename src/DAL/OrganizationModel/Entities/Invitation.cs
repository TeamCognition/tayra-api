using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Cog.DAL;
using Tayra.Common;

namespace Tayra.Models.Organizations
{
    public class Invitation : IAuditedEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public Guid Code { get; set; }

        public InvitationStatus Status { get; set; } = InvitationStatus.Created;
        public ProfileRoles Role { get; set; }

        [Required, MaxLength(1000)]
        public string EmailAddress { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        public int? SegmentId { get; set; }
        public Segment Segment { get; set; }

        public int? TeamId { get; set; }
        public Team Team { get; set; }

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