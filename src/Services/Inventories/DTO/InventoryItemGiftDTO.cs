using System;

namespace Tayra.Services
{
    public class InventoryItemGiftDTO
    {
        public Guid InventoryItemId { get; set; }
        public Guid ReceiverId { get; set; }
        public bool ClaimRequired { get; set; } = true;
        public DateTime? DemoDate { get; set; }
    }
}
