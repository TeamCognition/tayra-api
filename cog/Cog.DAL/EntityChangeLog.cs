using System;
using System.ComponentModel.DataAnnotations.Schema;
using Cog.Core;
using Microsoft.EntityFrameworkCore;

namespace Cog.DAL
{
    [Table("EntityChangeLogs"/*, Schema = "Cog"*/)]
    public class EntityChangeLog
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string EntityType { get; set; }

        public int EntityId { get; set; }

        public EntityState EntityState { get; set; }

        public AuditType AuditType { get; set; }

        public string ChangedValues { get; set; }

        public DateTime Modified { get; set; }

        public int ModifiedBy { get; set; }
    }
}