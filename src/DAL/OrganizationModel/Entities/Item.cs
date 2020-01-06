using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Firdaws.DAL;
using Tayra.Common;

namespace Tayra.Models.Organizations
{
    public class Item : IAuditedEntity
    {
        public int Id { get; set; }

        [MaxLength(100)]
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public float WorthValue { get; set; }

        public bool IsActivable { get; set; }
        public bool IsDisenchantable { get; set; }
        public bool IsGiftable { get; set; }

        public bool IsQuantityLimited { get; set; }

        public ItemRarities Rarity { get; set; }
        public ItemTypes Type { get; set; }

        public virtual ICollection<ItemReservation> Reservations { get; set; }

        #region IAuditedEntity

        public DateTime Created { get; set; }
        public DateTime? LastModified { get; set; }
        public int CreatedBy { get; set; }
        public int? LastModifiedBy { get; set; }

        #endregion
    }
}