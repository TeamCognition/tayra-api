using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Cog.DAL;
using Tayra.Common;

namespace Tayra.Models.Organizations
{
    public class Invitation : EntityGuidId, IAuditedEntity
    {
        [Required]
        public Guid Code { get; set; }

        public InvitationStatus Status { get; set; } = InvitationStatus.Created;
        public ProfileRoles Role { get; set; }

        [Required, MaxLength(1000)]
        public string EmailAddress { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        public Guid? SegmentId { get; set; }
        public Segment Segment { get; set; }

        public Guid? TeamId { get; set; }
        public Team Team { get; set; }
        
        public bool IsActive() => 
            Status != InvitationStatus.Accepted &&
            Status != InvitationStatus.Cancelled &&
            Status != InvitationStatus.Expired;

        //Test if this works in LINQ queries
        public static System.Linq.Expressions.Expression<Func<Invitation, bool>> IsActive2() =>
            i => i.Status != InvitationStatus.Accepted &&
                 i.Status != InvitationStatus.Cancelled &&
                 i.Status != InvitationStatus.Expired;
        
        #region IAuditedEntity

        public DateTime Created { get; set; }
        public DateTime? LastModified { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid? LastModifiedBy { get; set; }

        #endregion
    }
}